@ECHO OFF
:Loop
IF "%1"=="" GOTO Continue
   nuget pack SleepingBarber.Persistence.RavenDB.nuspec -version %1
   nuget pack SleepingBarber.Logging.nuspec -version %1
   nuget pack SleepingBarber.Logging.RavenDB.nuspec -version %1
   nuget pack SleepingBarber.nuspec -version %1
SHIFT
GOTO Loop
:Continue
