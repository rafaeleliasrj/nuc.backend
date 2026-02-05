using NautiHub.Infrastructure.Services.Ibge.Servers.Models.Responses;
using Refit;

namespace NautiHub.Infrastructure.Services.Ibge.Servers.Refit;

public interface IIbgeRefit
{
    [Get("/api/v1/localidades/estados?view=nivelado")]
    public Task<ApiResponse<IList<StateIbgeResponse>>> GetStates();

    [Get("/api/v1/localidades/estados/{uf}/municipios?view=nivelado")]
    public Task<ApiResponse<IList<CityIbgeResponse>>> GetMunicipalities(string uf);
}
