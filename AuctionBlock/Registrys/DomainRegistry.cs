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
        }
    }
}