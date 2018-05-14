/*
 * --------------------------------------------------------------------------------------------------------------------
 * Example sketch/program showing how to read data from a PICC to serial.
 * --------------------------------------------------------------------------------------------------------------------
 * This is a MFRC522 library example; for further details and other examples see: https://github.com/miguelbalboa/rfid
 * 
 * Example sketch/program showing how to read data from a PICC (that is: a RFID Tag or Card) using a MFRC522 based RFID
 * Reader on the Arduino SPI interface.
 * 
 * When the Arduino and the MFRC522 module are connected (see the pin layout below), load this sketch into Arduino IDE
 * then verify/compile and upload it. To see the output: use Tools, Serial Monitor of the IDE (hit Ctrl+Shft+M). When
 * you present a PICC (that is: a RFID Tag or Card) at reading distance of the MFRC522 Reader/PCD, the serial output
 * will show the ID/UID, type and any data blocks it can read. Note: you may see "Timeout in communication" messages
 * when removing the PICC from reading distance too early.
 * 
 * If your reader supports it, this sketch/program will read all the PICCs presented (that is: multiple tag reading).
 * So if you stack two or more PICCs on top of each other and present them to the reader, it will first output all
 * details of the first and then the next PICC. Note that this may take some time as all data blocks are dumped, so
 * keep the PICCs at reading distance until complete.
 * 
 * @license Released into the public domain.
 * 
 * Typical pin layout used:
 * -----------------------------------------------------------------------------------------
 *             MFRC522      Arduino       Arduino   Arduino    Arduino          Arduino
 *             Reader/PCD   Uno/101       Mega      Nano v3    Leonardo/Micro   Pro Micro
 * Signal      Pin          Pin           Pin       Pin        Pin              Pin
 * -----------------------------------------------------------------------------------------
 * RST/Reset   RST          9             5         D9         RESET/ICSP-5     RST
 * SPI SS      SDA(SS)      10            53        D10        10               10
 * SPI MOSI    MOSI         11 / ICSP-4   51        D11        ICSP-4           16
 * SPI MISO    MISO         12 / ICSP-1   50        D12        ICSP-1           14
 * SPI SCK     SCK          13 / ICSP-3   52        D13        ICSP-3           15
 */


#define SS_PIN 4  //D2
#define RST_PIN 5 //D1

#define DOORSENSPIN A0
#define DOORLOCKPIN 2
#define REDLED 0
#define GREENLED 15

#include <ArduinoJson.h>
#include <Ticker.h>
#include <SPI.h>
#include <MFRC522.h>
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
// Timers
Ticker ticker;
Ticker ticker2;
Ticker ticker3;


// Variables
MFRC522 mfrc522(SS_PIN, RST_PIN);  // Create MFRC522 instance
char lastID[4] = {0, 0, 0, 0};
char validID[] = {0x2B, 0xD6, 0x18, 0xFD};
byte valueReturned = 0;
byte timeout = 0;

enum DoorState { DOOROPEN, ISOPENING, DOORCLOSED };
DoorState state = DOOROPEN;

void setup() {
	Serial.begin(115200);		// Initialize serial communications with the PC
	SPI.begin();			// Init SPI bus
	mfrc522.PCD_Init();		// Init MFRC522
  pinMode(DOORLOCKPIN, OUTPUT);
  pinMode(REDLED, OUTPUT);
  pinMode(GREENLED, OUTPUT);
  digitalWrite(DOORLOCKPIN, LOW);

  if (IsDoorOpen())
  {
    digitalWrite(DOORLOCKPIN, HIGH);
    state = DOOROPEN;
  }
  else
  {
    digitalWrite(DOORLOCKPIN, LOW);
    state = DOORCLOSED;
  }

  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  //Connect to Wifi
  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
  }
  while (host == "")
  {
    getRaspberryIp();
  }
  
  mqClient.setServer(host.c_str(), 1883);
  mqClient.setCallback(callback);
}

void loop() {
  if(!mqClient.connected())
  {
    reconnect(); //Reconnect if not connected
  }
  mqClient.loop(); //Loop MQTT client

  switch(state)
  {
    case DOORCLOSED:
    	if ( ! mfrc522.PICC_IsNewCardPresent()) {
    		return;
    	}
     
      for(byte i = 0; i < 4; i++)
      {
        lastID[i] = 0;
      } 
    	// Select one of the cards
    	if ( ! mfrc522.PICC_ReadCardSerial()) {
    		return;
    	}
    
    	// Dump debug info about the card; PICC_HaltA() is automatically called
      HandleInput(&(mfrc522.uid));
      mfrc522.PICC_HaltA();
      return;

    case DOOROPEN:
        while(IsDoorOpen())
        {
          delay(100);
        }

        LockDoor();
        
        return;

    case ISOPENING:
        ticker.detach();
        delay(10);
        if (IsDoorOpen())
        {
          state = DOOROPEN;
        }
        else
        {
          state = DOORCLOSED;
          LockDoor();
        }
        return;
  }
}

// HANDLE RFID INPUT
void HandleInput(MFRC522::Uid *uid)
{
  for(byte i = 0; i < 4; i++)
  {
    lastID[i] =  uid->uidByte[i];
  }

  char arr[16];

  sprintf(arr, "%x%x%x%x", lastID[0],lastID[1],lastID[2],lastID[3]);
  
  mqClient.publish("Hodoor/EntryRequest", arr);
  timeout = 0;
  valueReturned = 0;
  ticker.once(5, TimeoutFunc);
  
  while(!timeout && !valueReturned)
  {
    if(!mqClient.connected())
    {
      reconnect(); //Reconnect if not connected
    }
    mqClient.loop(); //Loop MQTT client
  }
}


// DOOR FUNCTIONALITY
bool IsDoorOpen()
{
  int val = analogRead(DOORSENSPIN);
  Serial.println(val);
  return val > 350;
}

void LockDoor()
{
  digitalWrite(DOORLOCKPIN, LOW);
  state = DOORCLOSED;
}

void OpenDoor()
{
  digitalWrite(REDLED, LOW);
  digitalWrite(GREENLED, HIGH);
  digitalWrite(DOORLOCKPIN, HIGH);
  timeout = 0;
  ticker3.once(2, GreenLight);
  ticker.detach();
  ticker.once(5, TimeoutFunc);
  
  while( !IsDoorOpen() && timeout == 0) 
  {
    delay(100);
  } 
  state = ISOPENING;
}

void TimeoutFunc()
{
  digitalWrite(DOORLOCKPIN, LOW);
  timeout = 1;
}

void GreenLight()
{
  digitalWrite(GREENLED, LOW);
}


// RABBITMQ AND WIFI METHODS

void getRaspberryIp()
{
  HTTPClient http;
  http.begin("http://fwps.azurewebsites.net/api/ip/1");
  int code = http.GET();

  if (code > 0)
  {
    StaticJsonBuffer < 200 > jsonBuffer;

    JsonObject & root = jsonBuffer.parseObject(http.getString());

    String payload = root["ip"];
    host = payload;
  }
}

void reconnect()
{
  while (!mqClient.connected())
  {
    String clientId = "Hodoor";
    if (mqClient.connect(clientId.c_str(), "simon", "simon"))
    {
      //Publish announcement that you are here
      mqClient.publish("Hodoor/Hello", "Hello World");
      //Subscribe to MorningSun
      mqClient.subscribe("Hodoor/CmdLock");
      mqClient.subscribe("Hodoor/CmdUnlock");
    }
    else
    {
      delay(5000);
    }
  }
}

void callback(char * topic, byte* payload, unsigned int length)
{
  //Different handlers for different topics
  if (strcmp(topic, "Hodoor/CmdLock") == 0) //Received 'on'
  {
    valueReturned = 1;
    LockDoor();
    return;
  }
  else if (strcmp(topic, "Hodoor/CmdUnlock") == 0) //Received 'off'
  {
    valueReturned = 1;
    OpenDoor();
    return;
  }
}


