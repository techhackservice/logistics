using Autofac;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.UseCases;

namespace Web.Api.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthUseCases>().As<IAuthUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyUseCases>().As<ICompanyUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyBranchUseCases>().As<ICompanyBranchUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<OrderUseCases>().As<IOrderUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerUseCases>().As<ICustomerUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<ProductUseCases>().As<IProductUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<TrackerUseCases>().As<ITrackerUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<InvoiceUseCases>().As<IInvoiceUseCases>().InstancePerLifetimeScope();
        }
    }
}
