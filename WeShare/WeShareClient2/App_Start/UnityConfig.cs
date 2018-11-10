using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using WeShareClient2.ServiceReference1;
using Unity.Injection;

namespace WeShareClient2
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            container.RegisterType<IWeShareService, WeShareServiceClient>(new InjectionConstructor());
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}