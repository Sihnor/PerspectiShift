namespace Player.Scripts
{
    public enum EViewMode
    {
        TwoDimension,
        ThreeDimension
    }
    
    public interface IViewMode
    { 
        public EViewMode ViewMode { get; set; }
    }
}