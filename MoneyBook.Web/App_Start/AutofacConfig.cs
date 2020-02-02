using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using MoneyBook.Repositories;

namespace MoneyBook.Web {
    public static class AutofacConfig {
        public static void Register() {

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            RegisterDataModels(builder);

            RegisterAutoMapper(builder);

            // 建立容器
            IContainer container = builder.Build();

            // 解析容器內的型別
            AutofacDependencyResolver resolver = new AutofacDependencyResolver(container);

            // 建立相依解析器
            DependencyResolver.SetResolver(resolver);
        }

        private static void RegisterDataModels(ContainerBuilder builder) {
            builder.RegisterType<DbContext>()
                            .WithParameter("nameOrConnectionString", "MoneyBookContext")
                            .AsSelf()
                            .InstancePerRequest();

            builder.RegisterGeneric(typeof(GenericRepository<>))
                .As(typeof(IRepository<>));

            builder.RegisterAssemblyTypes(Assembly.Load("MoneyBook.Services"))
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }

        private static void RegisterAutoMapper(ContainerBuilder builder) {
            builder.Register(ctx => AutoMapperConfig.Create());

            builder.Register(ctx => {
                var config = ctx.Resolve<MapperConfiguration>();
                config.AssertConfigurationIsValid();
                return config.CreateMapper();
            }).As<IMapper>()
            .InstancePerLifetimeScope();
        }
    }
}
