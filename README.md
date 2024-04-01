A small convenience app to make regular backup of my Dragon's Dogma 2 backups.
Windows only for now. 

1. Clone the repo or download it as zip file: https://github.com/edveri/dragonsdogma2filebackup/archive/refs/heads/main.zip 
2. Extract the zip file and navigate to the **build** folder. 

_Optional: Open appsettings.json in Notedpad and adjust interval and destination for backups, if needed._

3. Run **FileBackupWorker.exe**. This is the only time you should need to run this manually. You can close the console window once launced.
    
When you launch Steam, the application should be launched together with the game.

By default, backups of the Steam backups will be done every 15 minutes. If you want to change this, edit appsettings.json, as described above.
