namespace Model
{
    public class City : BaseObject
    {
        public City() 
        {
            Name = string.Empty;
        }

        public City(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
