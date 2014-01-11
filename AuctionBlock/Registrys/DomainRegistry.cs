using System.Configuration;
using System.Reflection;
using AuctionBlock.DataAccess.Commands;
using AuctionBlock.DataAccess.Queries;
using AuctionBlock.Domain.Services;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using StructureMap.Configuration.DSL;
using Configuration = NHibernate.Cfg.Configuration;

namespace AuctionBlock.Registrys
{
    public class DomainRegistry : Registry
    {
        public DomainRegistry()
        {
            For<IAuctionBlockService>()
                .Singleton().Use<AuctionBlockService>();
        }
    }

    public class DataAccessRegistry : Registry
    {
        public DataAccessRegistry()
        {
            For<IGetActiveAuctionsQuery>()
                .Use<GetActiveAuctionsQuery>();
            For<IGetAuctionQuery>()
                .Use<GetAuctionQuery>();
            For<IStartAuctionCommand>()
                .Use<StartAuctionCommand>();
            For<ISessionFactory>().Singleton().Use(GetSessionFactory);
            For<ISession>().HttpContextScoped()
                .Use(c => c.GetInstance<ISessionFactory>().OpenSession());
        }

        public static ISessionFactory GetSessionFactory()
        {
            Configuration config 
                = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008
                        .ConnectionString(c => 
                            c.Is(GetConnectionString())))
                    .Mappings(
                        m => m.FluentMappings
                            .AddFromAssembly(Assembly.GetExecutingAssembly()))
                    .CurrentSessionContext<ThreadStaticSessionContext>()
                    .BuildConfiguration();

            new SchemaUpdate(config).Execute(false, true);

            return config.BuildSessionFactory();
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings["ApiConnectionString"].ConnectionString;
        }
    }
}