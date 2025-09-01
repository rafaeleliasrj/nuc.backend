namespace Avvo.Core.Messaging
{
    public class Namespace
    {
        private static string prefix = string.Empty;

        /// <summary>
        /// This is the namespace to prefix to Queue/Topic names.
        /// </summary>
        public static string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }
    }
}
