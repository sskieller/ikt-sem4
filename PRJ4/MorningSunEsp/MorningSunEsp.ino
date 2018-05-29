#include <ArduinoJson.h>
#include <ESP8266HTTPClient.h>
#include <ESP8266WiFi.h>
#include <Wire.h>
#include <PubSubClient.h>

//Wifi related
const char * ssid = "JonasAP"; //!< SSID to the WiFiNetwork
const char * password = "Fkt73gss"; //!< Password to the WiFiNetwork
String host = ""; //!< IP for raspberry. collected from fwps.azurewebsites.net/api/ip/1
WiFiClient espClient;
PubSubClient mqClient(espClient);
char buffer[20];
bool isOn = false;

/////////////////////////////////////////////////
/// Sets up the ESP chip. Will try to connect
/// to the network until it succedes.
/// Will get IP for Master Unit and connect to it.
/////////////////////////////////////////////////
void setup()
{
	Wire.begin(2,0); // Sets I2C pins to pin 0 and pin 2
	Serial.begin(115200);
	Serial.print("Connecting to ");
	Serial.println(ssid);

	pinMode(LED_BUILTIN, OUTPUT); // Initialize the LED_BUILTIN pin as an output
	delay(1000);
	digitalWrite(LED_BUILTIN, LOW);
	delay(1000);
	digitalWrite(LED_BUILTIN, HIGH);
	WiFi.mode(WIFI_STA);
	WiFi.begin(ssid, password);
	//Connect to Wifi
	while (WiFi.status() != WL_CONNECTED)
	{
		Serial.print('.');
		delay(500);
	}

	Serial.println("");
	Serial.println("WiFi connected");
	Serial.println("IP address: ");
	Serial.println(WiFi.localIP());

	while (host == "")
	{
		getRaspberryIp();
	}


	Serial.println("Setting up client");
	
	mqClient.setServer(host.c_str(), 1883);
	mqClient.setCallback(callback);
	Serial.println("Done setting up mqClient");

}

/////////////////////////////////////////////////
/// Callback to handle Messages received by the
/// Master Unit.
/////////////////////////////////////////////////
void callback(char * topic, byte* payload, unsigned int length)
{
	Serial.print("Message recived on topic: \"");
	Serial.print(topic);
	Serial.print("\": ");
	for (int i = 0; i < length; ++i)
	{
		Serial.print((char)payload[i]);
	}
	Serial.println();

	//Different handlers for different topics
	if (strcmp(topic, "MorningSun/CmdOn") == 0) //Received 'on'
	{
		Wire.beginTransmission(0x10);
		Wire.write('T');
		byte s = Wire.endTransmission();

		char chrarr[16];

    sprintf(chrarr, "Turned on %u", s);
    
    mqClient.publish("MorningSun/ModuleOn", chrarr);

		return;
	}
	else if (strcmp(topic, "MorningSun/CmdOff") == 0) //Received 'off'
	{
		Wire.beginTransmission(0x10);
		Wire.write('F');
		byte s = Wire.endTransmission();

    char chrarr[16];

    sprintf(chrarr, "Turned off %u", s);
    
		mqClient.publish("MorningSun/ModuleOff", chrarr);

		return;
	}

	Serial.println("Unknown command received");
}

/////////////////////////////////////////////////
/// Will get the current Master Unit IP from the
/// WebApi.
/////////////////////////////////////////////////
void getRaspberryIp()
{
	HTTPClient http;
	http.begin("http://fwps.azurewebsites.net/api/ip/1");
	int code = http.GET();

	if (code > 0)
	{
		Serial.print("HTTP code = ");
		Serial.println(code);
		StaticJsonBuffer < 200 > jsonBuffer;

		JsonObject & root = jsonBuffer.parseObject(http.getString());

		String payload = root["ip"];
		host = payload;
		Serial.println(payload);
	}
}

/////////////////////////////////////////////////
/// Will try to reconnect to the Master Unit.
/// If fails, retries every 5 seconds.
/////////////////////////////////////////////////
void reconnect()
{
	while (!mqClient.connected())
	{
		Serial.print("Trying to connect to message broker on ip: ");
		Serial.print(host);

		String clientId = "MorningSun";
		if (mqClient.connect(clientId.c_str(), "simon", "simon"))
		{
			Serial.println("Connected to message broker");
			//Publish announcement that you are here
			mqClient.publish("MorningSun/Hello", "Hello World");
			//Subscribe to MorningSun
			mqClient.subscribe("MorningSun/CmdOn");
			mqClient.subscribe("MorningSun/CmdOff");
		}
		else
		{
			Serial.print("failed, rc=");
			Serial.print(mqClient.state());
			Serial.println("Trying again in 5 sec");
			delay(5000);
		}
	}
}

/////////////////////////////////////////////////
/// Loop/Run function. Will read the status from
/// MorningSun and if the status has changed,
/// then it will alert the Master Unit.
/////////////////////////////////////////////////
void loop()
{
	if(!mqClient.connected())
	{
		reconnect(); //Reconnect if not connected
	}
	mqClient.loop(); //Loop MQTT client

	
	int i = 0;
	
	Wire.requestFrom(0x10, 1);
  delay(1);

  buffer[0] = Wire.read();
	
	//WIP
	if (buffer[0] == 0)
	{
		//Do nothing
	}
	else if (buffer[0] == 'F')
	{
		mqClient.publish("MorningSun/ModuleOff", "Status off");
	}
	else if (buffer[0] == 'T')
	{
		mqClient.publish("MorningSun/ModuleOn", "Status on");
	}

	delay(100);
	
}
