using Emgu.CV.CvEnum;

namespace EmguCV.Workbench.Util
{
    /// <summary>
    /// Selector for binary threshold method.
    /// </summary>
    public enum ThreshType
    {
        Binary = ThresholdType.Binary,
        BinaryInv = ThresholdType.BinaryInv
    }

    /// <summary>
    /// Selector for border type for warp processors.
    /// </summary>
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

    /// <summary>
    /// Selector for target type for camera calibration.
    /// </summary>
    public enum CalibTargetType
    {
        ChessBoard,
        CirclesGrid
    }

    /// <summary>
    /// Selector for order type for Sobel processor.
    /// </summary>
    public enum SobelOrder
    {
        X,
        Y
    }

    /// <summary>
    /// Selector for bounding rectangle type.
    /// </summary>
    public enum BoundingRectType
    {
        Upright,
        Rotated
    }

    /// <summary>
    /// Selector for bounding circle or ellipse.
    /// </summary>
    public enum BoundingCircleType
    {
        Circle,
        Ellipse
    }

    /// <summary>
    /// Selector for resultant image of background subtraction algorithm.
    /// </summary>
    public enum BgSubImageType
    {
        Motion,
        FgMask,
        Background
    }

    /// <summary>
    /// Selector for feature detection type.
    /// </summary>
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

    /// <summary>
    /// Selector for feature detection type for feature match algorithm.
    /// </summary>
    public enum MatcherDetectorType
    {
        SURF,
        SIFT,
        KAZE,
    }

    /// <summary>
    /// Selector for feature matcher type.
    /// </summary>
    public enum MatcherType
    {
        Flann,
        BF,
    }

    /// <summary>
    /// Selector for index parameter type to be used by Flann based matcher.
    /// </summary>
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