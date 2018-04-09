#include <ArduinoJson.h>
#include <ESP8266HTTPClient.h>
#include <ESP8266WiFi.h>
#include <Wire.h>


const char* ssid     = "";      // SSID
const char* password = "";      // Password
String host = "";  // IP for raspberry. Hentes fra fwps.azurewebsites.net/api/ip/1 
const int   port = 9000;            // Port serveur - Server Port
const int   watchdog = 1000;        // Fréquence du watchdog - Watchdog frequency
unsigned long previousMillis = millis(); 

void setup() {
  Wire.begin(); // Sets I2C pins to pin 0 and pin 2
  Serial.begin(115200);
  Serial.print("Connecting to ");
  Serial.println(ssid);

  pinMode(LED_BUILTIN, OUTPUT);     // Initialize the LED_BUILTIN pin as an output
  delay(1000);
  digitalWrite(LED_BUILTIN, LOW);
  delay(1000);
  digitalWrite(LED_BUILTIN, HIGH);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    Serial.print('.');
    delay(500);
  }

  Serial.println("");
  Serial.println("WiFi connected");  
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  HTTPClient http;

  http.begin("http://fwps.azurewebsites.net/api/ip/1");
  int code = http.GET();

  if (code > 0)
  {
    Serial.print("HTTP code = ");
    Serial.println(code);
    StaticJsonBuffer<200> jsonBuffer;

    JsonObject& root = jsonBuffer.parseObject(http.getString());

    String payload = root["ip"];
    host = payload;
    Serial.println(payload);
  }
}

void loop() {
  unsigned long currentMillis = millis();

  if ( currentMillis - previousMillis > watchdog ) {
    previousMillis = currentMillis;
    WiFiClient client;
    long zeroTime = millis();
    if (!client.connect(host, port)) {
      Serial.println("connection failed");
      return;
    }

    String url = "/watchdog?command=watchdog&uptime=";
    url += String(millis());
    url += "&ip=";
    url += WiFi.localIP().toString();
    
    // Envoi la requete au serveur - This will send the request to the server
    client.print("Ples update me \r\n");
    unsigned long timeout = millis();
    while (client.available() == 0) {
      if (millis() - timeout > 5000) {
        Serial.println(">>> Client Timeout !");
        client.stop();
        return;
      }
    }
  
    // Read all the lines of the reply from server and print them to Serial
    while(client.available()){
      String line = client.readStringUntil('\r');
      Serial.println(line);
      Serial.println((millis() - zeroTime));
      client.stop();
      if(line == "on")
      {
        digitalWrite(LED_BUILTIN, LOW);   // Turn the LED on (Note that LOW is the voltage level
        Wire.beginTransmission(0x10);
        Wire.write('a');
        Wire.endTransmission();
      }
      else if(line == "off")
      {
        digitalWrite(LED_BUILTIN, HIGH);   // Turn the LED on (Note that LOW is the voltage level
        Wire.beginTransmission(0x10);
        Wire.write('b');
        Wire.endTransmission();
      }
    }
  }
}