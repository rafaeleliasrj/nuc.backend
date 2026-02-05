using NautiHub.Domain.Services.InfrastructureService.ServidorIbge.Models.Responses;

namespace NautiHub.Domain.Services.InfrastructureService.Ibge;

public interface IIbgeService
{
    public Task<IEnumerable<StateIbgeResponse>> GetStatesAsync();
    public Task<IEnumerable<CityIbgeResponse>> GetCitiesAsync(string uf);
}
