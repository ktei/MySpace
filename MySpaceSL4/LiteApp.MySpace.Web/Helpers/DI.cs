using Ninject;
using System;
using Ninject.Parameters;

namespace LiteApp.MySpace.Web.Helpers
{
    public static class DI
    {
        static IKernel _kernel;

        public static void Initialize(IKernel kernel)
        {
            if (_kernel != null)
                throw new NotSupportedException("DI has already been initialized.");
            _kernel = kernel;
        }

        public static void Inject(object instance, params IParameter[] parameters)
        {
            _kernel.Inject(instance, parameters);
        }
    }
}