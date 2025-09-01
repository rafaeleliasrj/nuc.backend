namespace Avvo.Core.Messaging.Publisher.Dto
{
    public class TrackDynamicDto
    {
        public string EventName { get; set; }
        public string SourceSystem { get; set; }
        public string SourceModule { get; set; }
        public dynamic TrackJSON { get; set; }
    }
}
