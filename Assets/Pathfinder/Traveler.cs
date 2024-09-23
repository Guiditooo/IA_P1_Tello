using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Esta clase no deberia existir en mi proyecto actual.
//Va a ser reemplazada por la clase Agent.
//El pathfinding va a ser igual para todos

namespace FlyEngine
{
    public class Traveler<NodeType, CoordinateType> where NodeType : INode<CoordinateType>, INode, new() where CoordinateType : ICoordinate
    {
        
        //private GrapfView grapfView;
        private Grapf<NodeType, CoordinateType> grapf;

        private AStarPathfinder<NodeType, CoordinateType> pathfinder;
        //private DijstraPathfinder<Node<Vector2Int>> pathfinder;
        //private DepthFirstPathfinder<Node<Vector2Int>> pathfinder;
        //private BreadthPathfinder<Node<Vector2Int>> pathfinder;

        private Node<CoordinateType> startNode;
        private Node<CoordinateType> destinationNode;

        public Traveler(Grapf<NodeType, CoordinateType> grapf)
        {
            this.grapf = grapf;
            pathfinder = new AStarPathfinder<NodeType, CoordinateType> (this.grapf);//Esto no es necesario pasarselo asi
            
        }

        /*
        void Start()
        { 
            //Tengo que hacer que el nodo inicial y el final sean parte del grafo. Porque lo son xd
            startNode = new Node<Vector2Coordinate>();
            //startNode.SetCoordinate(new Vector2Coordinate(Random.Range(0, 10), Random.Range(0, 10)));

            destinationNode = new Node<Vector2Coordinate>();
            //destinationNode.SetCoordinate(new Vector2Coordinate(Random.Range(0, 10), Random.Range(0, 10)));           
            //StartCoroutine(Move(path));
        }
        */
        /*
        public IEnumerator Move(List<Node<Vector2Coordinate>> path)
        {
            foreach (Node<Vector2Coordinate> node in path)
            {
                transform.position = new Vector2Coordinate(node.GetCoordinate().x, node.GetCoordinate().y);
                yield return new WaitForSeconds(1.0f);
            }
        }
        */
    }

}