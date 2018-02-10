using Autofac;

namespace Recipes.DI
{
    public class AutofacSamples
    {
        public interface IService { string Name { get; } }
        public interface IContext { string Name { get; } }
        public interface IRepository { string Name { get; } }


        public class Context : IContext
        {
            public string Name => "Context";
        }


        public class Repository : IRepository
        {
            public string Name => "Repository";
        }


        public class Service : IService
        {
            public string Name => "TEST";
            private readonly IRepository _repository;
            private readonly IContext _context;

            public Service(IRepository repository, IContext context)
            {
                _repository = repository;
                _context = context;
            }
        }


        public static void PassCustomParametersToResolve()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Repository>().As<IRepository>();
            builder.RegisterType<Service>().As<IService>();

            IContainer container = builder.Build();

            // context instance has not been registered in DI container
            var item = container.Resolve<IService>(new TypedParameter(typeof(IContext), new Context()));
        }








    }
}
