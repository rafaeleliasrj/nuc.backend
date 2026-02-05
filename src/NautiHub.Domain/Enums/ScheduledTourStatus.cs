namespace NautiHub.Domain.Enums;

/// <summary>
/// Enumeração que representa os status de um passeio agendado.
/// </summary>
public enum ScheduledTourStatus
{
    /// <summary>
    /// Agendado - Passeio agendado e disponível para reservas
    /// </summary>
    Scheduled = 1,
    
    /// <summary>
    /// Em Andamento - Passeio em andamento
    /// </summary>
    InProgress = 2,
    
    /// <summary>
    /// Concluído - Passeio finalizado com sucesso
    /// </summary>
    Completed = 3,
    
    /// <summary>
    /// Cancelado - Passeio cancelado
    /// </summary>
    Cancelled = 4,
    
    /// <summary>
    /// Suspenso - Passeio temporariamente suspenso
    /// </summary>
    Suspended = 5
}