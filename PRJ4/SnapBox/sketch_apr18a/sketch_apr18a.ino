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
char i2cbuffer[20];
bool isOn = false;
byte arrIndex = 0;

void setup()
{
  Wire.begin(0,2); // Sets I2C pins to pin 0 and pin 2
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
  return;
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

    String clientId = "SnapBox";
    if (mqClient.connect(clientId.c_str(), "simon", "simon"))
    {
      Serial.println("Connected to message broker");
      //Publish announcement that you are here
      mqClient.publish("SnapBox/Hello", "Hello World");
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

  Wire.beginTransmission(0x10);
  Wire.write(0x10);
  Wire.endTransmission();

  delay(2);
  pinMode(2, INPUT);
  pinMode(0, INPUT);
  pinMode(2, OUTPUT);
  pinMode(0, OUTPUT);
  delayMicroseconds(10);
  pinMode(2, INPUT);
  pinMode(0, INPUT);
  pinMode(2, INPUT_PULLUP);
  pinMode(0, INPUT_PULLUP);
  delay(2);
  Wire.begin(0,2);
  delay(2);
  
  Wire.requestFrom(0x10, 1);
  delay(2);
  if(Wire.available())
  {
    i2cbuffer[0] = Wire.read();
  }

  if (i2cbuffer[0] & 0b10000000)
  {
    digitalWrite(LED_BUILTIN, LOW);
  }
  else
  {
    digitalWrite(LED_BUILTIN, HIGH);
  }
  
  if (i2cbuffer[0] != i2cbuffer[1] )
  {
    i2cbuffer[1] = i2cbuffer[0];
    
    char arr[16];
    
    sprintf(arr, "mail: %u : %u", (i2cbuffer[1] >> 7), (i2cbuffer[1] & 0b01111111));
    
    mqClient.publish("SnapBox/Update", arr);
  }

  delay(200);
  
}
