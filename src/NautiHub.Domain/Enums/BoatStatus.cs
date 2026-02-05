namespace NautiHub.Domain.Enums;

/// <summary>
/// Enumeração que representa os status de aprovação do cadastro da embarcação.
/// </summary>
public enum BoatStatus
{
    /// <summary>
    /// Pendente - Aguardando aprovação do administrador
    /// </summary>
    Pending = 1,
    
    /// <summary>
    /// Aprovado - Cadastro aprovado e ativo no sistema
    /// </summary>
    Approved = 2,
    
    /// <summary>
    /// Rejeitado - Cadastro rejeitado, necessita correções
    /// </summary>
    Rejected = 3,
    
    /// <summary>
    /// Suspenso - Cadastro temporariamente suspenso
    /// </summary>
    Suspended = 4,
    
    /// <summary>
    /// Cancelado - Cadastro cancelado pelo proprietário
    /// </summary>
    Cancelled = 5
}