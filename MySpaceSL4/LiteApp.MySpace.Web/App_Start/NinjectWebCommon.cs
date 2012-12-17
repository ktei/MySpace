[assembly: WebActivator.PreApplicationStartMethod(typeof(LiteApp.MySpace.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(LiteApp.MySpace.Web.App_Start.NinjectWebCommon), "Stop")]

namespace LiteApp.MySpace.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using LiteApp.MySpace.Web.DataAccess;
    using LiteApp.MySpace.Web.DataAccess.Mongo;
    using LiteApp.MySpace.Web.Helpers;
    using LiteApp.MySpace.Web.Shared;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            DI.Initialize(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IAlbumRepository>().To<AlbumRepository>();
            kernel.Bind<IPhotoRepository>().To<PhotoRepository>();
            kernel.Bind<IPhotoUploadTicketPool>().To<DefaultPhotoUploadTicketPool>();
        }        
    }
}
