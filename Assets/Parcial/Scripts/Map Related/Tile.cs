using FlyEngine;

public class Tile
{
    private TileType tileType;
    private Vector2Coordinate coord;

    public Tile(TileType tileType, int gridX, int gridY, float screenX, float screenY)
    {
        this.tileType = tileType;
        coord.SetGridPos(gridX, gridY);
        coord.x = screenX;
        coord.y = screenY;
    }

    public TileType GetTileType() => tileType;
}
