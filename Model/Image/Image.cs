namespace Model
{
    public class Image : DomainObject
    {
        #region props

        public string URL { get; set; }

        public Event? Event { get; set; }

        public long? Event_Id { get; set; }

        #endregion

        #region ctors

        public Image() 
        {
            URL = string.Empty;
            Event = null;
            Event_Id = null;
        }

        public Image(string url, Event? ev)
        {
            URL = url;
            Event = ev;
            Event_Id = ev?.Id;
        }

        #endregion
    }
}
