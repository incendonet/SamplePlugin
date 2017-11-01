# Sample SpeechBridge Plugin

This solution contains a complete SpeechBridge demo application that uses a plugin to retrieve information from the local PostgreSQL database by the caller's caller-id, a simple SRGS grammar for speech recognition, DTMF input, and reading it back to the caller with text-to-speech.  It was built in Visual Studio 2017 and targeting .NET Standard 2.0 for use on SpeechBridge Cloud Native Edition (CNE), but the same code should run on SpeechBridge 7 (AKA "Classic") when retarged for .NET 3.5.

The solution includes:

* The plugin DLL (`SamplePlug.dll`), which has a dependency on Npgsql.dll (from NuGet)
* A SQL initialization script (`tblCustomers_pgsql.sql`) to create the database and insert a sample row
* A VoiceXML application (`CheckBalanceDemo.vxml.xml`) with javascript to use the plugin

## Setting it Up

1. Build the DLL in Visual Studio  (You can download the free community edition of 2017 [here](https://www.visualstudio.com/))
1. Edit the sql file to add extensions that are relevant on your phone system
1. Copy the script to `/tmp/`
1. SSH to the host and run the script:
#### &nbsp;&nbsp;&nbsp;&nbsp;SpeechBridge Classic:
```
cd /opt/speechbridge/bin
mono sbdbutils.exe --run-sqlscript-commands /tmp/tblCustomers_pgsql.sql
```
#### &nbsp;&nbsp;&nbsp;&nbsp;SpeechBridge CNE Onsite

```
cd /usr/local/opt/incendonet/startup
source speechbridge.env.appliance.sh
source speechbridge.env.secrets.sh
cd ../services/
docker-compose \
    -f sbcore.docker-compose.yml -f sbcore-nethost-prod.docker-compose.yml \
    run --rm \
	-v /tmp:/tmp \
    -e SBLICENSESERVER_IP=${SBLICENSESERVER_IP} \
    -e SBLICENSESERVER_PORT=${SBLICENSESERVER_PORT} \
    -e POSTGRES_SBDB=${POSTGRES_SBDB} \
    -e POSTGRES_SBUSER=${POSTGRES_SBUSER} \
    -e POSTGRES_SBPASSWORD=${POSTGRES_SBPASSWORD} \
    -e DB_CKEY=${DB_CKEY} \
    sbdotnet \
        bash -c "
            /opt/speechbridge/bin/envsub.sh < /opt/speechbridge/templates/sbdbutils.exe.config.TEMPLATE > /opt/speechbridge/config/sbdbutils.exe.config &&
            /usr/bin/mono /opt/speechbridge/bin/sbdbutils.exe --run-sqlscript-commands /tmp/tblCustomers_pgsql.sql"
```

4. Copy the VoiceXML to the VoiceDocStore folder.  On SpeechBridge Classic this is `/opt/speechbridge/VoiceDocStore/`, and on CNE Onsite this is `/usr/local/opt/incendonet/volumes/opt-speechbridge/VoiceDocStore/`.
1. Copy the plugin to the plugins folder.  On SpeechBridge Classic this is `/opt/speechbridge/bin/`, and on CNE this is `/usr/local/opt/incendonet/volumes/opt-speechbridge/plugins/`.
1. Set a DID Mapping on your SpeechBridge to point to `CheckBalanceDemo.vxml.xml` on the Telephony page of the admin website.

That's it!  We'd love to get your feedback, either here or http://incendonet.com/contact-us/
