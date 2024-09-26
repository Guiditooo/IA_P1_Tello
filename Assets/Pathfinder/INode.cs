namespace FlyEngine
{
    public interface INode
    {
        public bool IsBloqued();
    }

    public interface ICoordinate<T>
    {
        ICoordinate<T> Create(params T[] values);
        T[] GetValues();
    }

    public interface IGrid
    {
        void SetGridPosX(int x);
        void SetGridPosY(int y);
        void SetGridPos(int x, int y);
        int GetGridPosX();
        int GetGridPosY();
        (int,int) GetGridPos();
    }


    public interface INode<CoordinateType, T> where CoordinateType : ICoordinate<T>
    {
        public void SetCoordinate(CoordinateType coordinateType);
        public CoordinateType GetCoordinate();
    }

}