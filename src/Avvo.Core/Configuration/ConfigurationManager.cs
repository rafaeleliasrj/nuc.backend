using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Avvo.Core.Commons.Exceptions;
using Avvo.Core.Configuration.Interfaces;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Avvo.Core.Configuration;

/// <summary>
/// Gerencia provedores e modificadores de configuração, carregando e validando configurações.
/// </summary>
public class ConfigurationManager : IConfigurationManager
{
    private readonly List<IConfigurationProvider> _providers;
    private readonly List<IConfigurationModifier> _modifiers;
    private readonly ILogger _logger;
    private readonly IDeserializer _yamlDeserializer;

    /// <summary>
    /// Obtém a lista de provedores registrados.
    /// </summary>
    public IReadOnlyList<IConfigurationProvider> Providers => _providers.AsReadOnly();

    /// <summary>
    /// Obtém a lista de modificadores registrados.
    /// </summary>
    public IReadOnlyList<IConfigurationModifier> Modifiers => _modifiers.AsReadOnly();

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ConfigurationManager"/>.
    /// </summary>
    /// <param name="logger">O logger para registro de eventos.</param>
    public ConfigurationManager(ILogger logger)
    {
        _providers = new List<IConfigurationProvider>();
        _modifiers = new List<IConfigurationModifier>();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
    }

    /// <summary>
    /// Adiciona um provedor de configuração.
    /// </summary>
    /// <param name="provider">O provedor de configuração a ser adicionado.</param>
    /// <exception cref="ArgumentNullException">Lançada se o provedor for nulo.</exception>
    public void AddProvider(IConfigurationProvider provider)
    {
        if (provider == null)
            throw new ArgumentNullException(nameof(provider));

        _logger.LogInformation("[{Manager}] Adicionando provedor: {Provider}", nameof(ConfigurationManager), provider.GetType().Name);
        _providers.Add(provider);
    }

    /// <summary>
    /// Adiciona um modificador de configuração pelo tipo.
    /// </summary>
    /// <typeparam name="T">O tipo do modificador a adicionar.</typeparam>
    /// <exception cref="ServiceException">Lançada se a criação do modificador falhar.</exception>
    public void AddModifier<T>() where T : IConfigurationModifier
    {
        try
        {
            var modifier = Activator.CreateInstance<T>();
            AddModifier(modifier);
        }
        catch (Exception ex)
        {
            throw new ServiceException($"Erro ao criar modificador do tipo {typeof(T).Name}.", ex);
        }
    }

    /// <summary>
    /// Adiciona um modificador de configuração.
    /// </summary>
    /// <param name="modifier">O modificador a adicionar.</param>
    /// <exception cref="ArgumentNullException">Lançada se o modificador for nulo.</exception>
    public void AddModifier(IConfigurationModifier modifier)
    {
        if (modifier == null)
            throw new ArgumentNullException(nameof(modifier));

        _logger.LogInformation("[{Manager}] Adicionando modificador: {Modifier}", nameof(ConfigurationManager), modifier.GetType().Name);
        _modifiers.Add(modifier);
    }

    /// <summary>
    /// Carrega configurações de todos os provedores registrados.
    /// </summary>
    /// <param name="profile">O perfil de configuração para validação (opcional).</param>
    /// <returns>Um resultado indicando o sucesso ou falha do carregamento.</returns>
    public void Load(object? profile = null)
    {
        try
        {
            var allParameters = new List<ConfigurationProviderParameter>();
            foreach (var provider in _providers)
            {
                var result = provider.LoadAsync(_logger).Result;
                if (result == null || !result.Any())
                {
                    _logger.LogError("[{Manager}] Erro ao carregar configurações do provedor {Provider}: {Count} parâmetros carregados.", nameof(ConfigurationManager), provider.GetType().Name, result?.Count ?? 0);
                    return;
                }
                allParameters.AddRange(result);
            }

            HydrateEnvironment(allParameters);
            if (profile != null)
                Validate(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[{nameof(ConfigurationManager)}] Erro ao carregar configurações.");
            throw new ServiceException("Erro ao carregar configurações.", ex);
        }
    }

    /// <summary>
    /// Valida as configurações atuais usando o perfil fornecido.
    /// </summary>
    /// <param name="profile">O perfil de configuração a validar.</param>
    /// <exception cref="ConfigurationValidationException">Lançada se a validação falhar.</exception>
    public void Validate(object profile)
    {
        if (profile == null)
            throw new ArgumentNullException(nameof(profile));

        var context = new ValidationContext(profile);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(profile, context, results, true))
        {
            var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
            throw new ConfigurationValidationException($"Falha na validação da configuração: {errors}");
        }
    }

    /// <summary>
    /// Executa todos os modificadores de configuração registrados.
    /// </summary>
    public void ApplyModifiers()
    {
        foreach (var modifier in _modifiers)
        {
            if (modifier.IsValid())
            {
                _logger.LogInformation("[{Manager}] Executando modificador: {Modifier}", nameof(ConfigurationManager), modifier.GetType().Name);
                modifier.Execute();
            }
            else
            {
                _logger.LogWarning("[{Manager}] Modificador {Modifier} não é válido e será ignorado.", nameof(ConfigurationManager), modifier.GetType().Name);
            }
        }
    }

    public Dictionary<string, string> GetValues()
    {
        var result = new Dictionary<string, string>();
        foreach (var provider in Providers)
        {
            var parameterProviderValue = provider.LoadAsync(_logger).Result;
            var values = GetValues(parameterProviderValue);
            foreach (var param in values)
                if (result.ContainsKey(param.Key))
                    throw new ArgumentException($"Configuração duplicada '{param.Key}'. Provider: {provider.GetType().FullName}");
                else
                    result.Add(param.Key, param.Value);
        }
        return result;
    }

    private IEnumerable<KeyValuePair<string, string>> GetValues(IReadOnlyList<ConfigurationProviderParameter> parameters)
    {
        foreach (var param in parameters)
        {
            switch (param.Format)
            {
                case ParameterFormat.Yaml:
                    var yamlValues = _yamlDeserializer.Deserialize<Dictionary<string, string>>(param.Value ?? string.Empty);
                    foreach (var kvp in yamlValues)
                        yield return kvp;
                    break;
                case ParameterFormat.Json:
                    var jsonValues = JsonSerializer.Deserialize<Dictionary<string, string>>(param.Value ?? string.Empty)
                        ?? new Dictionary<string, string>();
                    foreach (var kvp in jsonValues)
                        yield return kvp;
                    break;
                default:
                    yield return new KeyValuePair<string, string>(param.Name, param.Value ?? string.Empty);
                    break;
            }
        }
    }

    private void HydrateEnvironment(IReadOnlyList<ConfigurationProviderParameter> parameters)
    {
        foreach (var param in GetValues(parameters))
        {
            _logger.LogInformation("[{Manager}] Aplicando variável de ambiente: {Key}", nameof(ConfigurationManager), param.Key);
            Environment.SetEnvironmentVariable(param.Key, param.Value);
        }
    }
}
