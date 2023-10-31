namespace Model
{
    public class Image : DomainObject
    {
        #region props

        public string Name { get; set; }

        public Event? Event { get; set; }

        public long? Event_Id { get; set; }

        #endregion

        #region ctors

        public Image() 
        {
            Name = string.Empty;
            Event = null;
            Event_Id = null;
        }

        public Image(string name, Event? ev)
        {
            Name = name;
            Event = ev;
            Event_Id = ev?.Id;
        }

        #endregion
    }
}
