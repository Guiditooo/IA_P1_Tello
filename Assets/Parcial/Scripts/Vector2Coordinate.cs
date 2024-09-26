using System;

namespace FlyEngine
{
    public struct Vector2Coordinate : ICoordinate<float>, IGrid
    {
        public float x;
        public float y;
        private int gridPosX;
        private int gridPosY;

        public int GetGridPosX()
        {
            return gridPosX;
        }

        public int GetGridPosY()
        {
            return gridPosY;
        }
        public (int, int) GetGridPos()
        {
            return (gridPosX, gridPosY);
        }

        public ICoordinate<float> Create(params float[] values)
        {
            Vector2Coordinate a = new Vector2Coordinate();
            if (values != null && values.Length >= 2) 
            {
                a.x = values[0];
                a.y = values[1];
            }
            else
            {
                MessageDebugger.ShowMessage("Parametros incorrectos un Vector2Coordinate!");
            }
            return a;
        }

        public void SetGridPos(int x, int y)
        {
            gridPosX = x; 
            gridPosY = y;
        }

        public void SetGridPosX(int x)
        {
            gridPosX = x;
        }

        public void SetGridPosY(int y)
        {
            gridPosY = y;
        }

        public float[] GetValues()
        {
            float[] values = new float[2];
            values[0] = x;
            values[1] = y;
            return values;
        }

        public static float Distance(Vector2Coordinate a, Vector2Coordinate b)
        {
            float x = b.x - a.x;
            float y = b.y - a.y;
            return x*x + y*y;
        }

    }

}