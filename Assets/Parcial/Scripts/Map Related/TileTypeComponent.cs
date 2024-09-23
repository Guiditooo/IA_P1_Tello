namespace FlyEngine
{
    public enum TileType
    {
        Grass,      // 3 para Citizen - 3 para Caravana
        Road,      // 2 para Citizen - 1 para Caravana
        Mountain,  // 10 para Citizen - Imposible para Caravana
        Stream     // Imposible para Citizen - 5 para Caravana
    }

    public class TileTypeComponent : ECSComponent
    {
        public TileType tileType;

        public TileTypeComponent(TileType tileType)
        {
            this.tileType = tileType;
        }
        
    }
}