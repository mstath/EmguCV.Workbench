using System.ComponentModel;
using System.Windows.Media;

namespace EmguCV.Workbench.Algorithms
{
    public class None : ImageAlgorithm
    {
        [Browsable(false)]
        public override Color AnnoColor { get; set; }

        [Browsable(false)]
        public override int LineThick { get; set; }
    }
}
