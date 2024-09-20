using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyEngine
{
    public class Voronoi
    {
        private List<Vector2Coordinate> nodeList; //Reemplazar por node para hacerlo por pesos

        public Voronoi()
        {
            nodeList = new List<Vector2Coordinate>();
        }

        public void AddNode(Vector2Coordinate v)
        {
            nodeList.Add(v);
        }

        public void AddNodes(List<Vector2Coordinate> nodes)
        { 
            nodeList.AddRange(nodes);
        }

        public Vector2Coordinate GetMostNearbyNode(Vector2Coordinate node) //Reemplazar por voronoi
        {
            if (nodeList == null || nodeList.Count == 0)
                throw new ArgumentException("La lista de nodeList no puede estar vacía.");

            Vector2Coordinate nearby = nodeList.OrderBy(n => n.DistanceTo(node)).First();

            return nearby;
        }
    }

}