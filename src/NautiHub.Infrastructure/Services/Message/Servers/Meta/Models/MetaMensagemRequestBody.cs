namespace NautiHub.Infrastructure.Services.Message.Servers.Meta.Models
{
    public class MetaMensagemRequestBody
    {
        public string messaging_product { get; set; }
        public string recipient_type { get; set; }
        public string to { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string document { get; set; }
    }
}
