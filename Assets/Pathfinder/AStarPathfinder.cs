using System.Collections.Generic;

namespace FlyEngine
{
    public class AStarPathfinder<NodeType, CoordinateType> : Pathfinder<NodeType, CoordinateType>
        where NodeType : INode<CoordinateType>, INode, new()
        where CoordinateType : ICoordinate
    {

        public AStarPathfinder(Grapf<NodeType, CoordinateType> grapf) 
        {
            actualGrapf = grapf;
        }
        protected override float Distance(NodeType A, NodeType B)
        {
            CoordinateType coordA = A.GetCoordinate();
            CoordinateType coordB = B.GetCoordinate();
            return coordA.DistanceTo(coordB); // Usa el método de la interfaz ICoordinate
        }

        // Método para obtener los vecinos de un nodo
        protected override ICollection<NodeType> GetNeighbors(NodeType node)
        {
            List<NodeType> neighbors = new List<NodeType>();

            var coordinate = node.GetCoordinate();
            int x = coordinate.GetGridPosX();
            int y = coordinate.GetGridPosY();

            (int, int)[] directions = new (int, int)[]
            {
                (-1, 0),  (1, 0),  (0, -1), (0, 1),
                (-1, -1), (1, -1), (-1, 1), (1, 1)
            };

            foreach (var (dx, dy) in directions)
            {
                // Nueva coordenada en la dirección actual
                int newX = x + dx;
                int newY = y + dy;

                // Iterar sobre los nodos del grafo y buscar aquellos que tengan las coordenadas (newX, newY)
                foreach (var nodePair in actualGrapf.GetNodes())
                {
                    NodeType potentialNeighbor = nodePair.Value; // Obtén el nodo del diccionario
                    var neighborCoord = potentialNeighbor.GetCoordinate();

                    // Verifica si las coordenadas coinciden
                    if (neighborCoord.GetGridPosX() == newX && neighborCoord.GetGridPosY() == newY)
                    {
                        // Verifica si el nodo no está bloqueado
                        if (!potentialNeighbor.IsBloqued())
                        {
                            neighbors.Add(potentialNeighbor);
                        }
                    }
                }
            }

            return neighbors;
        }

        protected override bool IsBloqued(NodeType node)
        {
            return node.IsBloqued();
        }

        protected override float MoveToNeighborCost(NodeType A, NodeType B)
        {
            return Distance(A, B);
        }

        protected override bool NodesEquals(NodeType A, NodeType B)
        {
            return A.GetCoordinate().Equals(B.GetCoordinate());
        }
    }
}