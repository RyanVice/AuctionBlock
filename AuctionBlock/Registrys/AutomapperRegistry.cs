using System.Collections.Generic;
using AutoMapper;
using AutoMapper.Mappers;
using StructureMap.Configuration.DSL;

namespace AuctionBlock.Registrys
{
    public class AutomapperRegistry : Registry
    {
        public AutomapperRegistry()
        {
            For<ITypeMapFactory>().Use<TypeMapFactory>();
            For<IMappingEngine>().Use(() => Mapper.Engine);
            For<ConfigurationStore>().Singleton().Use<ConfigurationStore>()
                                     .Ctor<IEnumerable<IObjectMapper>>().Is(MapperRegistry.AllMappers());
            For<IConfigurationProvider>().Use(ctx => ctx.GetInstance<ConfigurationStore>());
            For<IConfiguration>().Use(ctx => ctx.GetInstance<ConfigurationStore>());
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AddAllTypesOf<Profile>();
            });
        }
   }
}