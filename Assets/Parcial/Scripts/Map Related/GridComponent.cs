namespace FlyEngine
{
    public class GridComponet : ECSComponent
    {
        public int x;
        public int y;

        public GridComponet(int gridX, int gridY)
        {
            x = gridX;
            y = gridY;
        }

    }
}