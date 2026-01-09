using AutoMapper;

using Models;

using TModel = Transport.Models;

namespace Transport.Mapping;

/// <summary>
/// Auto mapper profile for mapping Model layer to DTO.
/// </summary>
public sealed class HealthCheckMappingProfile : Profile
{
    /// <summary>
    /// Creates <see cref="HealthCheckMappingProfile"/>.
    /// </summary>
    public HealthCheckMappingProfile()
    {
        _ = CreateMap<HealthCheckReport, TModel.HealthCheckReport>()
            .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created))
            .ForMember(dest => dest.ReportItems, opt => opt.MapFrom(src => src.ReportItems));

        _ = CreateMap<HealthCheckReportItem, TModel.HealthCheckReportItem>()
            .ForMember(dest => dest.ResourceName, opt => opt.MapFrom(src => src.ResourceName.Value))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
    }
}