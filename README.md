# SleepingBarber
A lightweight queue for .net
  - concurrent and fault tolerant.
  - support for logging.
  - with persistance.
  - support for events.

![Alt text](https://gitlab.com/sameralrawi/sleepingbarber/raw/364ba9a945b703bc7e667f10b6bfdf007f49a648/diagram.jpg)

> SleepingBarber can be hosted as part of ASP.net application.

> Or as part of a windows service(recommended).

> Or part of any console or windows application.


### Tech and Demos
* Real time dashboard for monitoring queue progress [AngularJS, SignalR]
* InMemory repository example.
* Persistance using RavenDB


### Installation
```sh
Install-Package SleepingBarber
```
For Persistance using RavenDB
```sh
Install-Package SleepingBarber.Persistance.RavenDB
```

### Future work
* Windows service package with WCF OWIN support and TopShelf.


License
----

MIT


**Free Software, Hell Yeah!**

