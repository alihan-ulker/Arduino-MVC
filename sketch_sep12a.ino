//DHT11 Sensorunun arti(+) ucu, Arduino EthernetShield uzerindeki 5V pinine takilir.
//DHT11 Sensorunun eksi(-) ucu, Arduino EthernetShield uzerindeki GND pinine takilir.
//DHT11 Sensorunun out/signal(cikis/sinyal) ucu, Arduino EthernetShield uzerindeki A0 pinine takilir.

#include <SPI.h>
#include <Ethernet.h>
#include <DHT.h>
#include "DHT.h"
#define DHTPIN A0
#define DHTTYPE DHT11
DHT dht(DHTPIN, DHTTYPE);
byte mac[] = {0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED }; 
IPAddress ip(10,100,1,140);
EthernetServer server(80);
  
void setup() {
  
  dht.begin();
  Ethernet.begin(mac, ip);
  server.begin();
  Serial.print("Sunucu Var");
  Serial.println(Ethernet.localIP());
}

void loop() {
  delay(1000);
  float h = dht.readHumidity();
  float t = dht.readTemperature();
  int a=0;
  EthernetClient client = server.available();
  
    if (client) {
    Serial.println("Yeni Alici");
    boolean currentLineIsBlank = true;
            
    while (client.connected()) {
        if (client.available()) {
        char c = client.read();
        Serial.write(c);
        if (c == '\n' && currentLineIsBlank) {
            client.println("HTTP/1.1 200 OK");
            client.println("Status: 200");
            client.println("Content-Type: text/xml");
            client.println("</?xml version = \"1.0\" ?>");
            client.println("<!DOCTYPE HTML>");
            client.println("<Connection: keep-alive>");
            client.println("Refresh: 5");
            client.println();
            client.println("<inputs>");
            client.println("<sicaklik>");
            client.println(t);
            client.println("</sicaklik>");
            client.println("<nem>");
            client.println(h);
            client.println("</nem>");
            client.println("</inputs>");
          break;
        }
        if (c == '\n') {
          currentLineIsBlank = true;
        }
        else if (c != '\r') {
          currentLineIsBlank = false;
        }
      }
    }
    delay(5);
    client.stop();
    Serial.println("Alici Baglantisi Kesildi");
  }
}
