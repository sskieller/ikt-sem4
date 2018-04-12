#include <ArduinoJson.h>
#include <ESP8266HTTPClient.h>
#include <ESP8266WiFi.h>
#include <Wire.h>
#include <PubSubClient.h>

//Wifi related
const char * ssid = "ap88v0"; // SSID
const char * password = "thang6troimua"; // Password
String host = ""; // IP for raspberry. Hentes fra fwps.azurewebsites.net/api/ip/1 
WiFiClient *espClient = new WiFiClient();
PubSubClient mqClient(*espClient);

void setup()
{
	Wire.begin(); // Sets I2C pins to pin 0 and pin 2
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
		digitalWrite(LED_BUILTIN, LOW); // Turn the LED on (Note that LOW is the voltage level
		Wire.beginTransmission(0x10);
		Wire.write('a');
		Wire.endTransmission();

		return;
	}
	else if (strcmp(topic, "MorningSun/CmdOff") == 0) //Received 'off'
	{
		digitalWrite(LED_BUILTIN, HIGH); // Turn the LED on (Note that LOW is the voltage level
		Wire.beginTransmission(0x10);
		Wire.write('b');
		Wire.endTransmission();

		return;
	}
	else if (strcmp(topic, "MorningSun/CmdStatus") == 0)
	{
		int i = 0;
		char buffer[20];
		Wire.requestFrom(0x10, 1);

		while (Wire.available())
		{
			buffer[i] = Wire.read();
			++i;
		}

		Serial.println("Received following from Morning sun: ");
		
		bool isOn = false;
		String response = "";
		//WIP
		if (isOn)
		{
			mqClient.publish("MorningSun/ModuleOn", response.c_str());
		}
		else
		{
			mqClient.publish("MorningSun/ModuleOff", response.c_str());
		}
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
		reconnect();
	}
	mqClient.loop();
	
	

}
