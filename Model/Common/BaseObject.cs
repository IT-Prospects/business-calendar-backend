using System.ComponentModel;

namespace Model
{
    /// <summary>
    /// Базовый класс
    /// </summary>
    [Description("Базовый класс")]
    public abstract class BaseObject
    {
        public virtual long Id { get; set; }
    }
}
