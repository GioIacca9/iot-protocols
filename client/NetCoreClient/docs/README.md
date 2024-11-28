# Documentazione Client

## Panoramica

Questo progetto è un client MQTT basato su .NET Core che interagisce con un broker MQTT per monitorare i dati di un sensore DHT11 (temperatura e umidità) e ricevere comandi dal broker. L'applicazione è progettata per leggere periodicamente i dati dal sensore, inviarli al broker MQTT e ascoltare i comandi ricevuti su uno specifico topic.


## Funzionalità

1. Connessione al broker MQTT:
   - Il client si connette al broker MQTT specificato usando le credenziali definite.
   - Supporta la sottoscrizione a topic specifici per ricevere comandi.
2. Pubblicazione di dati del sensore:
   - Legge i dati dal sensore DHT11 e li invia al topic configurato.
3. Gestione dei comandi MQTT:
   - Riceve e gestisce comandi inviati al topic specificato.

## Configurazioni

- Broker MQTT:
  - Host: `iot.scuola.iacca.ml`
  - Porta: `1883`
  - ClientID: mqtt-01
- Topic:
  - Pubblicazione dati: `iot/0001/data`
  - Comandi: `iot/0001/commands/#`