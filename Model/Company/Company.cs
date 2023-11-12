namespace Model
{
    public class Company : DomainObject
    {
        #region props

        public string Name { get; set; }

        #endregion

        #region ctors

        public Company()
        {
            Name = string.Empty;
        }

        public Company(string name)
        {
            Name = name;
        }

        #endregion

    }
}
