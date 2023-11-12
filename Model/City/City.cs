namespace Model
{
    public class City : DomainObject
    {
        #region props

        public string Name { get; set; }

        #endregion

        #region ctors

        public City()
        {
            Name = string.Empty;
        }

        public City(string name)
        {
            Name = name;
        }

        #endregion

    }
}
