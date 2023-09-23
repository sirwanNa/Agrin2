using AutoMapper;

namespace Agrin2.Config.AutoMapper
{
    public interface IHaveCustomMappings
    {
        void CreateMappings(IMapperConfigurationExpression configuration);
    }
}
