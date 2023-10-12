using System;

namespace Model
{
    public class Image : DomainObject
    {
        #region props

        public string Name { get; set; }

        #endregion

        #region ctors

        public Image() 
        {
            Name = string.Empty;
        }

        public Image(string name)
        {
            Name = name;
        }

        #endregion
    }
}
