using AutoMapper;
using Practice.Service.EmployeeAPI.Model;
using Practice.Service.EmployeeAPI.Model.Dto;

namespace Practice.Service.EmployeeAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<EmployeeDto, Employee>();
                config.CreateMap<Employee, EmployeeDto>();
            });
            return mappingConfig;
        }
    }
}
