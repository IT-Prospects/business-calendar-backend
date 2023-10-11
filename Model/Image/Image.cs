using System;

namespace Model
{
    public class Image : BaseObject
    {
        #region props

        public string Name { get; set; }

        #endregion

        #region ctors

        public Image() {}

        public Image(string name)
        {
            Name = name;
        }

        #endregion
    }
}
