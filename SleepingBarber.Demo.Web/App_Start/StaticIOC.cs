using Microsoft.Practices.Unity;

namespace SleepingBarber.Demo.Web.App_Start
{
    public static class StaticIOC
    {
        public static IUnityContainer Container { get; set; }
    }
}