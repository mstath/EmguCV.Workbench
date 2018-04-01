using Emgu.CV.CvEnum;

namespace EmguCV.Workbench.Util
{
    public enum ThreshType
    {
        Binary = ThresholdType.Binary,
        BinaryInv = ThresholdType.BinaryInv
    }

    public enum BordType
    {
        Constant = BorderType.Constant,
        Replicate = BorderType.Replicate,
        Reflect = BorderType.Reflect,
        Wrap = BorderType.Wrap,
        Reflect101 = BorderType.Reflect101,
        Default = BorderType.Default,
        Transparent = BorderType.Constant,
    }

    public enum CalibTargetType
    {
        ChessBoard,
        CirclesGrid
    }

    public enum SobelOrder
    {
        X,
        Y
    }

    public enum BoundingRectType
    {
        Upright,
        Rotated
    }

    public enum BoundingCircleType
    {
        Circle,
        Ellipse
    }

    public enum BgSubImageType
    {
        Motion,
        FgMask,
        Background
    }

    public enum DetectorType
    {
        AKAZE,
        Brisk,
        FastDetector,
        GFTTDetector,
        KAZE,
        MSERDetector,
        ORBDetector,
        SIFT,
        StarDetector,
        SURF,
    }

    public enum MatcherDetectorType
    {
        SURF,
        SIFT,
        KAZE,
    }

    public enum MatcherType
    {
        Flann,
        BF,
    }

    public enum MatcherIndexParamsType
    {
        Autotuned,
        Composite,
        HierarchicalClustering,
        KdTree,
        KMeans,
        Linear,
    }
}