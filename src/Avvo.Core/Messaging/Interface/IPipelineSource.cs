namespace Avvo.Core.Messaging.Interface
{
    public interface IPipelineSource
    {
        /// <summary>
        /// This is the topic of the source
        /// </summary>
        ITopic Topic { get; }

        /// <summary>
        /// This is the name of the source
        /// </summary>
        string Name { get; }
    }
}
