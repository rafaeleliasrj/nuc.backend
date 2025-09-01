using System.Threading.Tasks;

namespace Avvo.Core.Messaging.Interface
{
    public interface ITopicService
    {
        /// <summary>
        /// This method creates a topic for use.
        /// </summary>
        /// <param name="topic">The topic to create.</param>
        Task CreateTopicAsync(ITopic topic);

        /// <summary>
        /// This method creates a topic for use.
        /// </summary>
        /// <typeparam name="T">The type of the ITopic</typeparam>
        Task CreateTopicAsync<T>()
        where T : ITopic;

        /// <summary>
        /// This method is called to determine if a specific ITopic has been registered.
        /// </summary>
        /// <typeparam name="T">The type of the ITopic</typeparam>
        bool IsRegistered<T>()
        where T : ITopic;

        /// <summary>
        /// This method is called to determine if a specific ITopic has been registered.
        /// </summary>
        /// <param name="topic">The ITopic</param>
        bool IsRegistered(ITopic topic);
    }
}
