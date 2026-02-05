using Microsoft.Extensions.Logging;
using NautiHub.Core.Resources;
using NautiHub.Domain.Services.InfrastructureService.Ibge;
using NautiHub.Domain.Services.InfrastructureService.ServidorIbge.Models.Responses;
using NautiHub.Infrastructure.Services.Ibge.Servers.Refit;

namespace NautiHub.Infrastructure.Services.Comum.ServidorIbge;

public class ServidorIbgeService : IIbgeService
{
    private readonly ILogger<ServidorIbgeService> _logger;
    private readonly IIbgeRefit _ibgeRefit;
    private readonly MessagesService _messagesService;

    public ServidorIbgeService(ILogger<ServidorIbgeService> logger, IIbgeRefit ibgeRefit, MessagesService messagesService)
    {
        _logger = logger;
        _ibgeRefit = ibgeRefit;
        _messagesService = messagesService;
    }

    public async Task<IEnumerable<StateIbgeResponse>> GetStatesAsync()
    {
        _logger.LogInformation("[IBGE] Iniciando consulta de estados");

        try
        {
            var responseIbge = await _ibgeRefit.GetStates();

            if (!responseIbge.IsSuccessStatusCode || responseIbge.Content == null)
            {
                _logger.LogWarning(_messagesService.Ibge_No_States_Found);
                return null;
            }

            return responseIbge.Content.Select(estado => new StateIbgeResponse
            {
                Codigo = estado.Code.ToString(),
                Uf = estado.Uf,
                Descricao = estado.Name
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _messagesService.Ibge_Error_States);
            throw;
        }
    }

    public async Task<IEnumerable<CityIbgeResponse>> GetCitiesAsync(string uf)
    {
        _logger.LogInformation("[IBGE] Iniciando consulta de municípios. UF: {Uf}", uf);

        try
        {
            var responseIbge = await _ibgeRefit.GetMunicipalities(uf);

            if (!responseIbge.IsSuccessStatusCode || responseIbge.Content == null)
            {
                _logger.LogWarning(_messagesService.Ibge_No_Cities_Found, uf);
                return null;
            }

            return responseIbge.Content.Select(municipio => new CityIbgeResponse
            {
                Uf = uf,
                Codigo = municipio.Code.ToString(),
                Descricao = municipio.Name
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _messagesService.Ibge_Error_Cities, uf);
            throw;
        }
    }
}
