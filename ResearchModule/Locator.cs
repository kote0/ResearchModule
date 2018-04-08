using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule
{
    public static class Locator
    {
        static Dictionary<Type, object> servicesDictionary = new Dictionary<Type, object>();

        static void Register<T>(T service)
        {
            servicesDictionary[typeof(T)] = service;
        }

        static void  Get<T>()
        {

        }

        public static T GetService<T>()
        {
            T instance = default(T);

            if (servicesDictionary.ContainsKey(typeof(T)) == true)
            {
                instance = (T)servicesDictionary[typeof(T)];
            }
            else
            {
                var type = Startup.ServiceProvider.GetService(typeof(T));
                instance = (T)type;
                Register(instance);
            }

            return instance;
        }
    }
}
