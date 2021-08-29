using System;
using System.Threading.Tasks;
using Autofac;
using Companies.Domain;
using Companies.Infrastructure;
using Shared.Application;
using Shared.Domain;

namespace ConsoleApp
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await using var container = ConfigureServices();
            await using var scope = container.BeginLifetimeScope();
            
            var companyRepository = scope.Resolve<ICompanyRepository>();
            var domainInvoker = scope.Resolve<DomainInvoker>();
            
            var companyOne = new Company("Some company", "9785615346");
            var companyTwo = new Company("Another company", "7577261187");
            
            await companyRepository.Create(companyOne);
            await companyRepository.Create(companyTwo);
            
            // should be duplicated
            var resultOne = await domainInvoker.Do(
                () => companyOne.SetNip("7577261187"));

            Console.WriteLine(resultOne.GetType().Name);
            
            // should be unique
            var resultTwo = await domainInvoker.Do(
                () => companyOne.SetNip("5123504798"));

            Console.WriteLine(resultTwo.GetType().Name);
        }
        
        private static IContainer ConfigureServices()
        {
            var containerBuilder = new ContainerBuilder();
            
            containerBuilder
                .RegisterType<CompanyRepository>()
                .As<ICompanyRepository>()
                .SingleInstance();
            
            containerBuilder
                .RegisterType<DomainInvoker>()
                .AsSelf()
                .InstancePerDependency();
            
            containerBuilder
                .Register<BusinessRulesFactory>(context =>
                {
                    var c = context.Resolve<IComponentContext>();
                    return t => c.Resolve(t);
                });
            
            containerBuilder
                .RegisterAssemblyTypes(typeof(Company).Assembly)
                .AssignableTo<BusinessRule>()
                .AsSelf()
                .InstancePerDependency();
            
            return containerBuilder.Build();
        }
    }
}