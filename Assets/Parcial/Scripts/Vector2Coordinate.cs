namespace FlyEngine
{
    public struct Vector2Coordinate : ICoordinate
    {
        public float x;
        public float y;
        public int gridPosX;
        public int gridPosY;

        public Vector2Coordinate(int gridPosX, int gridPosY, float x, float y)
        {
            this.x = x;
            this.y = y;
            this.gridPosX = gridPosX;
            this.gridPosY = gridPosY;
        }

        public float DistanceTo(Vector2Coordinate other)
        {
            return (float)System.Math.Sqrt(System.Math.Pow(other.x - x, 2) + System.Math.Pow(other.y - y, 2));
        }

        public float DistanceTo(ICoordinate other)
        {
            if (other is Vector2Coordinate otherCoord)
            {
                return DistanceTo(otherCoord);
            }
            else
            {
                throw new System.ArgumentException("Expected a Vector2Coordinate");
            }
        }

        public int GetGridPosX()
        {
            return gridPosX;
        }

        public int GetGridPosY()
        {
            return gridPosY;
        }
    }

}