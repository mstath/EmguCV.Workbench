using System.ComponentModel;
using System.Windows.Media;

namespace EmguCV.Workbench.Algorithms
{
    /// <summary>
    /// No algorithm (display raw image).
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Algorithms.ImageAlgorithm" />
    public class None : ImageAlgorithm
    {
        [Browsable(false)]
        public override Color AnnoColor { get; set; }

        [Browsable(false)]
        public override int LineThick { get; set; }
    }
}
