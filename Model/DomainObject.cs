using System.ComponentModel;

namespace Model
{
    /// <summary>
    /// Базовый класс
    /// </summary>
    [Description("Базовый класс")]
    public abstract class DomainObject
    {
        public virtual long Id { get; set; }
    }
}
