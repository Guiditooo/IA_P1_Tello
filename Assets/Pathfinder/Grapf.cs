using System.Collections.Generic;

namespace FlyEngine
{
    public class Grapf<NodeType, CoordinateType> where NodeType : INode<CoordinateType>, INode, new() where CoordinateType : ICoordinate
    {
        private Dictionary<(int x, int y), NodeType> nodes = new Dictionary<(int x, int y), NodeType>();

        public Grapf(int xCount, int yCount, float nodeSeparation, float initialX = 0.0f, float initialY = 0.0f)
        {
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    NodeType node = new NodeType();

                    // Dependiendo del tipo de coordenada, asigna los valores correspondientes
                    CoordinateType coordinate = (CoordinateType)System.Activator.CreateInstance(
                        typeof(CoordinateType), i, j, initialX + i * nodeSeparation, initialY + j *+nodeSeparation);

                    node.SetCoordinate(coordinate);

                    nodes.Add((i, j), node);
                }
            }
        }

        public NodeType GetNode(int x, int y)
        {
            nodes.TryGetValue((x, y), out NodeType node);
            return node;
        }

        public void SetNode(int x, int y, NodeType node)
        {
            nodes[(x, y)] = node;
        }

        public Dictionary<(int x, int y), NodeType> GetNodes() => nodes;
    }
}