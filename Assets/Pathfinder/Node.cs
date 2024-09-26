namespace FlyEngine
{
    public class Node<CoordType, T> : INode, INode<CoordType, T> where CoordType : ICoordinate<T>
    {
        private CoordType coordinate;
        private bool isBlocked;

        public Node()
        {
            isBlocked = false;
        }

        public Node(bool isBlocked = false)
        {
            this.isBlocked = isBlocked;
        }

        public bool IsBloqued()
        {
            return isBlocked;
        }

        public void SetCoordinate(CoordType newCoordinate)
        {
            coordinate = newCoordinate;
        }

        public CoordType GetCoordinate()
        {
            return coordinate;
        }
    }
}