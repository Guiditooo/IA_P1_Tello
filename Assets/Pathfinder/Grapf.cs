using System;
using System.Collections.Generic;
using System.Timers;

namespace FlyEngine
{
    public class Grapf<NodeType, CoordinateType, T> 
        where NodeType : INode<CoordinateType, T>, INode, new() 
        where CoordinateType : ICoordinate<T>, IGrid, new()
    {
        private Dictionary<(int x, int y), NodeType> nodes = new Dictionary<(int x, int y), NodeType>();

        public Grapf(int xCount, int yCount, float nodeSeparation, float initialX = 0.0f, float initialY = 0.0f)
        {
            for (int y = 0; y < xCount; y++)
            {
                for (int x = 0; x < yCount; x++)
                {
                    NodeType node = new NodeType();
                    
                    //Dependiendo del tipo de coordenada, asigna los valores correspondientes
                    //CoordinateType coordinate = (CoordinateType)System.Activator.CreateInstance(typeof(CoordinateType), y, x, initialX + y * nodeSeparation, initialY + x *+nodeSeparation);

                    CoordinateType coordinate = (CoordinateType)Activator.CreateInstance(typeof(CoordinateType));

                    coordinate = (CoordinateType)coordinate.Create
                    (
                        (T)Convert.ChangeType(initialX + x * nodeSeparation, typeof(T)),
                        (T)Convert.ChangeType(initialY + y * nodeSeparation, typeof(T))
                    );

                    string msg = "Nodo " + x + "." + y + " se creo en X=" + initialX + x * nodeSeparation + " Y=" + initialY + y * nodeSeparation;
                    MessageDebugger.ShowMessage(msg);

                    coordinate.SetGridPos(x, y);

                    node.SetCoordinate(coordinate);

                    nodes.TryAdd((x, y), node);
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
            nodes.TryAdd((x, y), node);
        }

        public Dictionary<(int x, int y), NodeType> GetNodes() => nodes;
    }
}