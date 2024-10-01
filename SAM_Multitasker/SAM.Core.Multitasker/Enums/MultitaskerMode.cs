using System.ComponentModel;

namespace SAM.Core.Multitasker
{
    [Description("Multitasker Mode")]
    public enum MultitaskerMode
    {
        [Description("Parallel")] Parallel,
        [Description("Series")] Series
    }
}
