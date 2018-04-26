#include <ArduinoJson.h>
#include <ESP8266HTTPClient.h>
#include <ESP8266WiFi.h>
#include <Wire.h>
#include <PubSubClient.h>

//Wifi related
const char * ssid = "JonasAP"; // SSID
const char * password = "Fkt73gss"; // Password
String host = ""; // IP for raspberry. Hentes fra fwps.azurewebsites.net/api/ip/1
WiFiClient espClient;
PubSubClient mqClient(espClient);
char buffer[20];
bool isOn = false;

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

		mqClient.publish("MorningSun/ModuleOn", "Turned on " + (int)s);

		return;
	}
	else if (strcmp(topic, "MorningSun/CmdOff") == 0) //Received 'off'
	{
		Wire.beginTransmission(0x10);
		Wire.write('F');
		byte s = Wire.endTransmission();
		
		mqClient.publish("MorningSun/ModuleOff", "Turned off " + (int)s);

		return;
	}
	else if (strcmp(topic, "MorningSun/CmdStatus") == 0)
	{
		Wire.beginTransmission(0x10);
		Wire.write('L');
		byte s = Wire.endTransmission();
	}

	Serial.println("Unknown command received");
}

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
			mqClient.subscribe("MorningSun/CmdStatus");
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

void loop()
{
	if(!mqClient.connected())
	{
		reconnect(); //Reconnect if not connected
	}
	mqClient.loop(); //Loop MQTT client

	/*
	int i = 0;
	
	Wire.requestFrom(0x10, 1);

	while (Wire.available())
	{
		buffer[i] = Wire.read();
		++i;
	}
	i = 0;


	//Serial.println("Received following from Morning sun: ");
	for (int j = 0; j < i; ++j)
	{
		Serial.print(buffer[j]);
	}

	
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

	delay(200);
	*/
}
