using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agrin2.Config.AutoMapper
{
    public static class AutoMapperConfig
    {
        private static MapperConfigurationExpression _configuration = new MapperConfigurationExpression();
        private static void LoadCustomMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where
                            typeof(IHaveCustomMappings).IsAssignableFrom(t) && !t.IsAbstract &&
                            !t.IsInterface
                        select (IHaveCustomMappings)Activator.CreateInstance(t)).ToArray();
            foreach (var map in maps)
            {
                map.CreateMappings(_configuration);
            }
        }
        public static void Configure(params string[] assemblies)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(x => assemblies.Contains(x.GetName().Name))
                        .SelectMany(x => x.DefinedTypes);
            _configuration.CreateMissingTypeMaps = true;            
            LoadCustomMappings(types);
            ignoreUnMappedProperties();
            Mapper.Initialize(_configuration);
        }

        private static void ignoreUnMappedProperties()
        {
            _configuration.ForAllMaps((map, expr) =>
            {
                foreach (var prop in map.GetUnmappedPropertyNames())
                {
                    expr.ForMember(prop, opt => opt.Ignore());
                }
            });
        }
    }
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
}
