// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Reflection;
using AuctionBlock.Registrys;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using StructureMap;
using Configuration = NHibernate.Cfg.Configuration;

namespace AuctionBlock.DependencyResolution {
    public static class IoC {

        private const string ConnectionString = "Server=.;Database=AuctionBlock;Trusted_Connection=True;";

        public static IContainer Initialize() {
            ObjectFactory.Initialize(
                x =>
                {
                    x.AddRegistry<DomainRegistry>();
                    x.AddRegistry<AutomapperRegistry>();
                    x.AddRegistry<InfrastructureRegistry>();
                    x.AddRegistry<DataAccessRegistry>();
                    x.For<ISessionFactory>().Singleton().Use(GetSessionFactory);
                    x.For<ISession>().HttpContextScoped()   
                        .Use(c => c.GetInstance<ISessionFactory>().OpenSession());
                });
            return ObjectFactory.Container;
        }

        public static ISessionFactory GetSessionFactory()
        {
            Configuration config = Fluently.Configure().
                Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.Is(ConnectionString))).
                Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())).
                CurrentSessionContext<ThreadStaticSessionContext>().
                BuildConfiguration();

            new SchemaUpdate(config).Execute(false, true);

            return config.BuildSessionFactory();
        }
    }
}