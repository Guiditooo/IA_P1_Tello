namespace FlyEngine
{
    public interface INode
    {
        public bool IsBloqued();
    }

    public interface ICoordinate
    {
        float DistanceTo(ICoordinate other);
        int GetGridPosX();
        int GetGridPosY();
    }

    public interface INode<CoordinateType> where CoordinateType : ICoordinate
    {
        public void SetCoordinate(CoordinateType coordinateType);
        public CoordinateType GetCoordinate();
    }

}