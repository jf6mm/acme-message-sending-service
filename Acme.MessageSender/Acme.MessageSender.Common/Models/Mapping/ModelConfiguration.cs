using Acme.MessageSender.Common.Models.Mapping.Profiles;
using AutoMapper;

namespace Acme.MessageSender.Common.Models.Mapping
{
	public static class ModelConfiguration
	{
        public static MapperConfiguration CreateMapperConfiguration()
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<EmployeeProfile>();
            });
            return configuration;
        }
    }
}
