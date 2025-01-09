# AMQP

AMQP viene implementato utilizzando i seguenti parametri:

- **Exchange**: direct
- **Coda**: `states`
- **Autodelete**: no


I parametri di connessione (hostname, virtual host, username e password) sono da inserire nel file `appsettings.json` nel seguente formato:

```json
{
  "AMQP": {
    "Hostname": "",
    "VirtualHost": "/",
    "Username": "",
    "Password": ""
  }
}
```

I messaggi vengono pubblicati dal client nella coda `states` e vengono consumati dal server immediatamente. L'auto ACK sul server è abilitato.
