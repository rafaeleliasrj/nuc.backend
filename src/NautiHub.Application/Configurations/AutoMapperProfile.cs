using AutoMapper;
using NautiHub.Domain.Entities;
using NautiHub.Application.UseCases.Models.Responses;

namespace NautiHub.Application.Configurations;

/// <summary>
/// Classe de configuração do AutoMapper com perfis de mapeamento do projeto.
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Inicializa uma nova instância do perfil de mapeamento.
    /// </summary>
    public AutoMapperProfile()
    {
        // Mapeamento de User para UserResponse
        CreateMap<User, UserResponse>();

        // Mapeamento de Boat para BoatResponse
        CreateMap<Boat, BoatResponse>()
            .ForMember(dest => dest.ImageCount, opt => opt.MapFrom(src => src.Images.Count));

        // Mapeamento de Booking para BookingResponse  
        CreateMap<Booking, BookingResponse>()
            .ForMember(dest => dest.PaymentCount, opt => opt.MapFrom(src => src.Payments.Count))
            .ForMember(dest => dest.TotalPaid, opt => opt.MapFrom(src => src.GetTotalPaid()));

        // Mapeamento de Review para ReviewResponse
        CreateMap<Review, ReviewResponse>();

        // Mapeamento de Payment para PaymentResponse
        CreateMap<Payment, PaymentResponse>();
    }
}