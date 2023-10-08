namespace Model
{
    public class Image : BaseObject
    {
        public Image() 
        {
            Path = string.Empty;
        }

        public Image(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
    }
}
