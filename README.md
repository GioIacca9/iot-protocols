# Branch MQTT

In questo branch viene implementato l'uso del protocollo di comunicazione MQTT, un protocollo di messaggistica leggero e basato sul modello publish/subscribe, progettato per dispositivi con risorse limitate e reti con larghezza di banda ridotta.

# Topic 

Per la costruzione dei topic è stata decisa una gerarchia del tipo "progetto / versione / idDispositivo / (dati \ comandi)". In questo modo possiamo mantenere una struttura gerarchica, consentiamo futuri sviluppi introducendo una versione, riusciamo a tenere separato l'invio e la ricezione di comandi e dati per ogni dispositivo.
