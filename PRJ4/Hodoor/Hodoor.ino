/**@file Hodoor.ino */

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
const char * ssid = "JonasAP"; //!< SSID to the WiFiNetwork
const char * password = "Fkt73gss"; //!< Password to the WiFiNetwork
String host = ""; //!< IP for raspberry. collected from fwps.azurewebsites.net/api/ip/1
WiFiClient espClient;
PubSubClient mqClient(espClient);
// Timers
Ticker ticker;
Ticker ticker2;
Ticker ticker3;


// Variables
MFRC522 mfrc522(SS_PIN, RST_PIN);  // Create MFRC522 instance
char lastID[4] = {0, 0, 0, 0};
byte valueReturned = 0;
byte timeout = 0;

enum DoorState { DOOROPEN, ISOPENING, DOORCLOSED };
DoorState state = DOOROPEN;

/////////////////////////////////////////////////
/// Sets up the ESP chip. Will try to connect
/// to the network until it succedes.
/// Will get IP for Master Unit and connect to it.
/////////////////////////////////////////////////
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


/////////////////////////////////////////////////
/// Loop/Run function. Will try to read a card
/// if the door is closed. If it reads a card, it 
/// will try to validate the card.
/// Will keep the door unlocked as long as it's 
/// open. Will lock the door once its closed.
/////////////////////////////////////////////////
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

  delay(200);
}

/////////////////////////////////////////////////
/// Handles the card input and will try to 
/// validate it. Will timeout after 5 seconds.
/////////////////////////////////////////////////
void HandleInput(MFRC522::Uid *uid)
{
  for(byte i = 0; i < 4; i++)
  {
    lastID[i] =  uid->uidByte[i];
  }

  char arr[16];

  sprintf(arr, "%x%x%x%x", lastID[0],lastID[1],lastID[2],lastID[3]);

  Serial.println("Sending request");
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
    delay(100);
  }
}


/////////////////////////////////////////////////
/// Checks if the door is open.
/////////////////////////////////////////////////
bool IsDoorOpen()
{
  delay(100);
  int val = analogRead(DOORSENSPIN);
  return val > 350;
}

/////////////////////////////////////////////////
/// Locks the door
/////////////////////////////////////////////////
void LockDoor()
{
  digitalWrite(DOORLOCKPIN, LOW);

  // CHECK LATER WITH OPENDOOR
  mqClient.publish("Hodoor/ModuleLock", "");
  state = DOORCLOSED;
}

/////////////////////////////////////////////////
/// Display to User that the card is denied.
/////////////////////////////////////////////////
void AccessDenied()
{
  digitalWrite(DOORLOCKPIN, LOW);
  state = DOORCLOSED;
  digitalWrite(REDLED, HIGH);
  delay(500);
  digitalWrite(REDLED, LOW);
  delay(500);
  digitalWrite(REDLED, HIGH);
  delay(500);
  digitalWrite(REDLED, LOW);
}

/////////////////////////////////////////////////
/// Unlocks the door
/////////////////////////////////////////////////
void OpenDoor()
{
  digitalWrite(REDLED, LOW);
  digitalWrite(GREENLED, HIGH);
  digitalWrite(DOORLOCKPIN, HIGH);
  timeout = 0;
  ticker3.once(2, GreenLight);
  ticker.detach();
  ticker.once(10, TimeoutFunc);

  // CHECK LATER WITH LOCKDOOR
  mqClient.publish("Hodoor/ModuleUnlock", "");
  
  while( !IsDoorOpen() && timeout == 0) 
  {
    delay(100);
  } 
  state = ISOPENING;
}

/////////////////////////////////////////////////
/// Handles the timeout from HandleInput()
/////////////////////////////////////////////////
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
    StaticJsonBuffer < 200 > jsonBuffer;

    JsonObject & root = jsonBuffer.parseObject(http.getString());

    String payload = root["ip"];
    host = payload;
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
    String clientId = "Hodoor";
    if (mqClient.connect(clientId.c_str(), "simon", "simon"))
    {
      //Subscribe to MorningSun
      mqClient.subscribe("Hodoor/CmdModLock");
      mqClient.subscribe("Hodoor/CmdModUnlock");
      mqClient.subscribe("Hodoor/CmdModDenied");
    }
    else
    {
      delay(5000);
    }
  }
}

/////////////////////////////////////////////////
/// Callback to handle Messages received by the
/// Master Unit.
/////////////////////////////////////////////////
void callback(char * topic, byte* payload, unsigned int length)
{
  Serial.println(topic);
  //Different handlers for different topics
  if (strcmp(topic, "Hodoor/CmdModLock") == 0) //Received 'on'
  {
    valueReturned = 1;
    LockDoor();
    return;
  }
  else if (strcmp(topic, "Hodoor/CmdModUnlock") == 0) //Received 'off'
  {
    valueReturned = 1;
    OpenDoor();
    return;
  }
    else if (strcmp(topic, "Hodoor/CmdModDenied") == 0) //Received 'accessDenied'
  {
    valueReturned = 1;
    AccessDenied();
    return;
  }
}


