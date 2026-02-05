namespace NautiHub.Domain.Enums;

/// <summary>
/// Enumeração que representa os tipos de embarcação disponíveis no sistema.
/// </summary>
public enum BoatType
{
    /// <summary>
    /// Veleiro - Embarcação a vela
    /// </summary>
    Sailboat = 1,
    
    /// <summary>
    /// Lancha - Embarcação a motor de pequeno/médio porte
    /// </summary>
    Motorboat = 2,
    
    /// <summary>
    /// Iate - Embarcação de luxo a motor
    /// </summary>
    Yacht = 3,
    
    /// <summary>
    /// Catamarã - Embarcação com dois cascos
    /// </summary>
    Catamaran = 4,
    
    /// <summary>
    /// Barco de pesca - Embarcação para atividades de pesca
    /// </summary>
    FishingBoat = 5,
    
    /// <summary>
    /// Barco rápido - Embarcação de alta velocidade
    /// </summary>
    Speedboat = 6,
    
    /// <summary>
    /// Casa flutuante - Embarcação residencial
    /// </summary>
    Houseboat = 7
}