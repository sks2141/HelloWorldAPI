using HelloWorld.Utilities;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using HelloWorld.ViewModel;
using HelloWorld.Model;

namespace HelloWorld.Services
{
    public class ServiceModule : IServiceModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IService, Service>();
        }

        public void Configure()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<ResponseStatsViewModel, ResponseEntity>()
                      .ForMember(r => r.Id, opt => opt.MapFrom(r => r.ResponseId))
                      .ForMember(r => r.Message, opt => opt.MapFrom(r => r.Message))
                      .ReverseMap();

                config.CreateMap<ResponseStatsViewModel, StatsEntity>()
                      .ForMember(r => r.IpAddress, opt => opt.MapFrom(r => r.IpAddress))
                      .ForMember(r => r.ClientDetails, opt => opt.MapFrom(r => r.ClientDetails))
                      .ForMember(r => r.ResponseId, opt => opt.MapFrom(r => r.ResponseId))
                      .ReverseMap();
            });
        }
    }
}