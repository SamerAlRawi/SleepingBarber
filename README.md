### SleepingBarber
a lightweight **in-process** queue for .net
  - concurrent and fault tolerant.
  - support for logging.
  - persistance using RavenDB.
  - support for events.

![Alt text](https://github.com/sameralrawi/sleepingbarber/raw/364ba9a945b703bc7e667f10b6bfdf007f49a648/diagram.jpg)

> SleepingBarber can be hosted as part of ASP.net application.

> Or as part of a windows service(recommended).

> Or part of any console or windows application.


#### Demo projects
* Real time dashboard for monitoring queue progress [AngularJS, SignalR]
* InMemory repository example.
* Persistance using RavenDB

#### Examples
[Installation - step by step example](#install)

[Persistance using RavenDB](#persistance)

[Logging](#logging)


#### Installation<a name="install"></a>
Create a new console application then install SleepingBarber nuget package
```sh
Install-Package SleepingBarber
```
after installing SleepingBarber nuget package as shown above
implement the `ICustomer` interface
```csharp
public class Customer : ICustomer
{
    private int _id;
    public Customer(int id)
    {
        _id = id;
    }
    public string Id
    {
        get { return _id.ToString(); }
        set { }
    }
}
```
Implement your server class and Implement the `IServer<T>` interface
```csharp
public class Server<T> : IServer<T> where T : ICustomer
{
    public void Serve(T customer)
    {
        Thread.Sleep(1000);
        WriteLine($"Customer {customer.Id} Got his haircut!");
    }
}
```
you can implement `Customer` specific server instead of the generic server shown above

example:
```csharp
public class CustomerServer : IServer<Customer>
{
    public void Serve(Customer customer)
    {
        Thread.Sleep(1000);
        Console.WriteLine($"Customer {customer.Id} Got his haircut!");
    }
}
```

Add the following code to `Program.cs`:
```csharp
using SleepingBarber;

class Program
{
    private static CustomersQueue<Customer> _queue;
    private static SleepingBarber<Customer> _sleepingBarber;
    private static IServer<Customer> _server;

    static void Main(string[] args)
    {
        bool exitRequested = false;
        _queue = new CustomersQueue<Customer>();
        _server = new Server();
        _sleepingBarber = new SleepingBarber<Customer>(_queue, _server);
        _sleepingBarber.GoingToSleep += BarberWentToSleep;
        WriteLine("Enter customers range to add then press ENTER");
        WriteLine("Enter Q to Exit");
        
        while (!exitRequested)
        {
            var input = ReadLine();
            exitRequested = input.ToUpper() == "Q";
            AddCustomers(input);
        }
    }

    private static void BarberWentToSleep(object sender, DateTime e)
    {
        WriteLine($"Barber went to sleep @ {e}");
    }

    private static void AddCustomers(string input)
    {
        try
        {
            var count = int.Parse(input, 0);
            for (int i = 0; i < count; i++)
            {
                _queue.Enqueue(new Customer(i+1));
            }
        }
        catch (Exception ex)
        {
            WriteLine(ex.Message);
            WriteLine("Press Q to Exit....");
        }
    }
}
```

#### Persistance using RavenDB <a name="persistance"></a>
After following the steps for running sample console app

Add `SleepingBarber.Persistance.RavenDB` package to your console app
```sh
Install-Package SleepingBarber.Persistance.RavenDB
```
Copy the following code to your `Program.cs>>(void Main)`

```csharp
using SleepingBarber;
using Raven.Client.Document;
using SleepingBarber.Persistance.RavenDB;

class Program
{
    static void Main(string[] args)
    {
        /* Can use any RavenDb implementation
         * EmbeddableDocumentStore, 
         * RavenDbDocumentStore,
         * DocumentStore
         */
        var ravenDbdocumentStore = new DocumentStore
        {
            Url = "http://localhost:8080",
            DefaultDatabase = "Customers",
            //ConnectionStringName = "RavenDBDatabaseConnection"//to use a connection string from the App.config
        };
        //Initialize raven db repository
        var repository = new RavenDBcustomerRepository<Customer>(ravenDbdocumentStore);
        //Initialize queue
        var queue = new PersistanceCustomersQueue<Customer>(repository);
        //Our custom server
        var server = new CustomerServer();
        //Initialize barber instance
        var barber = new SleepingBarber<Customer>(queue, server);
        //Subscribe to customer server event
        barber.CustomerServed += CustomerServed;
        //Subscribe to sleep event
        barber.GoingToSleep += BarberWentToSleep;
        //Queue some messages
        for (int i = 0; i <50; i++)
        {
            var name = $"Customer{i}";
            queue.Enqueue(new Customer { Id = name, Name = name, DateCreate = DateTime.Now });
            Console.WriteLine($"{i} was added to database");
        }
        //Wait as the barber serve all customers
        //no action needed at this point
        //barber will keep serving all customers until queue is empty
        //adding new customer to queue will wake up the barber to begin serving again
        Console.ReadLine();
    }

    private static void BarberWentToSleep(object sender, DateTime e)
    {
        Console.WriteLine("Barber went to sleep Zzzz, \ncustomers queue is empty.");
    }

    private static void CustomerServed(object sender, string customerId)
    {
        Console.WriteLine($"Barber says Customer {customerId} is served.");
    }
}
```

#### Logging <a name="logging"></a>

TODO add some logging examples here

License
----

MIT

**Free Software, Hell Yeah!**

