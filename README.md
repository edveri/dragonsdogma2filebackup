A small convenience app to make regular backup of my Dragon's Dogma 2 backups.

1. Download the compiled files here: https://github.com/edveri/dragonsdogma2filebackup/releases/download/Version3.0/DragonsDogma2FileBackup_v_3_1.zip 
2. Extract the archive
3. Open appsettings.json and adjust interval and destination for backups, if needed
4. Run FileBackupWorker.exe. This is the only time you should need to run this manually. You can close the console window once launced.
   It should tell you that a .bat file has been created. 
 
When you launch Steam, the application should be launched together with the game.

By default, backups of the Steam backups will be done every 15 minutes. If you want to change this, edit appsettings.json, as described above.
