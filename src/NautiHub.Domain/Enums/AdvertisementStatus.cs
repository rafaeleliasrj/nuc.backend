namespace NautiHub.Domain.Enums
{
    /// <summary>
    /// Representa o status de um anúncio no sistema.
    /// </summary>
    public enum AdvertisementStatus
    {
        /// <summary>
        /// Anúncio criado e aguardando aprovação.
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Anúncio aprovado e ativo para visualização.
        /// </summary>
        Approved = 2,

        /// <summary>
        /// Anúncio rejeitado pelo administrador.
        /// </summary>
        Rejected = 3,

        /// <summary>
        /// Anúncio suspenso temporariamente.
        /// </summary>
        Suspended = 4,

        /// <summary>
        /// Anúncio cancelado pelo proprietário.
        /// </summary>
        Cancelled = 5,

        /// <summary>
        /// Anúncio finalizado/expirado.
        /// </summary>
        Completed = 6,

        /// <summary>
        /// Anúncio pausado pelo proprietário.
        /// </summary>
        Paused = 7
    }
}