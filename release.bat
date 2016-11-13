@ECHO OFF

MSBuild.exe SleepingBarber.sln /t:Build /p:Configuration=Release
if /I "%ERRORLEVEL%" NEQ "0" (
   echo MSBUILD Task failed %ERRORLEVEL%
   exit /b %ERRORLEVEL%
)

nunit3-console.exe ".\SleepingBarber.Tests\bin\Release\SleepingBarber.Tests.dll" --output=result.xml
if /I "%ERRORLEVEL%" NEQ "0" (
   echo NUNIT Task failed %ERRORLEVEL%
   exit /b %ERRORLEVEL%
)

:Loop
IF "%1"=="" GOTO Continue
   nuget pack SleepingBarber.Persistence.RavenDB.nuspec -version %1
   nuget pack SleepingBarber.Logging.nuspec -version %1
   nuget pack SleepingBarber.Logging.RavenDB.nuspec -version %1
   nuget pack SleepingBarber.nuspec -version %1
SHIFT
GOTO Loop
:Continue

