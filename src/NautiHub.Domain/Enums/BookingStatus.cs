namespace NautiHub.Domain.Enums
{
    public enum BookingStatus
    {
        Pending,       // Aguardando confirmação
        Confirmed,     // Confirmada
        Paid,          // Paga
        InProgress,    // Em andamento
        Completed,     // Concluída
        Cancelled,     // Cancelada
        Refunded       // Reembolsada
    }
}
