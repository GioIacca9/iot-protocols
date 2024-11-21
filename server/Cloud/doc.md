# Documentazione dei Servizi Cloud
## Panoramica
Questo progetto C# implementa un servizio cloud che si connette a un broker MQTT, si sottoscrive a un topic specifico e scrive i dati ricevuti in un database InfluxDB. Il progetto è composto da due classi principali: CloudServices e MqttProvider.

## Funzionalità
1) L'applicazione inizia creando un'istanza di CloudServices e chiamando il suo metodo Start().
2) Il metodo Start() inizializza il client MQTT, si connette al broker su "iot.scuola.iacca.ml" sulla porta 1883 e si sottoscrive al topic "iot/casetta/data".
3) Quando viene ricevuto un messaggio sul topic sottoscritto, viene attivato il metodo MessageReceived().
4) Il messaggio ricevuto viene deserializzato in un oggetto Data.
5) Viene creato un client InfluxDB e viene costruito un punto dati con i valori di temperatura e umidità ricevuti.
6) Il punto dati viene scritto nel database InfluxDB denominato "iotprotocol" nell'organizzazione "its".

## Dipendenze
InfluxDB.Client: Utilizzato per connettersi e scrivere dati su InfluxDB.
MQTTnet.Client: Utilizzato per la comunicazione MQTT.
System.Text.Json: Utilizzato per la deserializzazione JSON.

## Note
Il metodo Main() entra in un ciclo infinito dopo l'avvio dei servizi cloud, mantenendo l'applicazione in esecuzione.