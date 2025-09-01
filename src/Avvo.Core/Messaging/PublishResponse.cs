using System;
using System.Collections.Generic;
using System.Text;

namespace Avvo.Core.Messaging
{
    /// <summary>
    /// Contém dados correspondentes a um request de publicação de
    /// uma mensagem em em uma fila.
    /// </summary>
    public class PublishResponse
    {
        /// <summary>
        /// Identificador único do request realizado. É
        /// gerado internamente pelo Avvo.Core
        /// </summary>
        public Guid RequestId { get; }


        public PublishResponse(Guid requestId, string messageId)
        {
            RequestId = requestId;
            MessageId = messageId;
        }

        /// <summary>
        /// Identificador único da mensagem publicada. É
        /// gerado pelo serviço onde a mensagem foi publicada.
        /// </summary>
        public string MessageId { get; }
    }
}
