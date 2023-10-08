namespace Model
{
    public class Image : BaseObject
    {
        #region props

        public string Path { get; set; }

        #endregion

        #region ctors

        public Image()
        {
            Path = string.Empty;
        }

        public Image(string path)
        {
            Path = path;
        }

        #endregion
    }
}
