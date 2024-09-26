using System.Collections.Generic;

namespace FlyEngine
{

    public abstract class Pathfinder<NodeType, CoordinateType, T> where NodeType : INode<CoordinateType, T>, INode, new() where CoordinateType : ICoordinate<T>, IGrid, new()
    {
        protected Grapf<NodeType, CoordinateType, T> actualGrapf;

        public List<NodeType> FindPath(NodeType startNode, NodeType destinationNode)
        {
            // Crear un diccionario para almacenar los nodos, su padre, el costo acumulado y la heurística
            Dictionary<NodeType, (NodeType Parent, float AcumulativeCost, float Heuristic)> nodes =
                new Dictionary<NodeType, (NodeType Parent, float AcumulativeCost, float Heuristic)>();

            // Inicializar todos los nodos en el grafo con el costo acumulado máximo
            foreach (var nodePair in actualGrapf.GetNodes())
            {
                NodeType node = nodePair.Value; // Obtenemos el nodo del diccionario
                nodes.Add(node, (default, float.MaxValue, 0));
            }

            // Crear listas para los nodos abiertos y cerrados
            List<NodeType> openList = new List<NodeType> { startNode };
            List<NodeType> closedList = new List<NodeType>();

            // Inicializar el nodo de inicio con costo acumulado 0 y calcular la heurística inicial
            nodes[startNode] = (default, 0, Distance(startNode, destinationNode));

            // Bucle principal del algoritmo A*
            while (openList.Count > 0)
            {
                NodeType currentNode = openList[0];
                int currentIndex = 0;

                // Encontrar el nodo con la menor suma de costo acumulado + heurística en la lista abierta
                for (int i = 1; i < openList.Count; i++)
                {
                    var currentData = nodes[openList[i]];
                    var currentNodeData = nodes[currentNode];

                    if (currentData.AcumulativeCost + currentData.Heuristic < currentNodeData.AcumulativeCost + currentNodeData.Heuristic)
                    {
                        currentNode = openList[i];
                        currentIndex = i;
                    }
                }

                // Sacar el nodo actual de la lista abierta y añadirlo a la lista cerrada
                openList.RemoveAt(currentIndex);
                closedList.Add(currentNode);

                // Si llegamos al nodo de destino, generamos y devolvemos el camino
                if (NodesEquals(currentNode, destinationNode))
                {
                    return GeneratePath(startNode, destinationNode);
                }

                // Obtener los vecinos del nodo actual y calcular el costo para cada uno
                foreach (NodeType neighbor in GetNeighbors(currentNode))
                {
                    if (closedList.Contains(neighbor) || IsBloqued(neighbor))
                    {
                        continue;
                    }

                    float tentativeAcumulativeCost = nodes[currentNode].AcumulativeCost + MoveToNeighborCost(currentNode, neighbor);

                    // Si el vecino no está en la lista abierta o encontramos un camino más corto hacia él
                    if (!openList.Contains(neighbor) || tentativeAcumulativeCost < nodes[neighbor].AcumulativeCost)
                    {
                        // Actualizar el nodo vecino con el nuevo padre, costo acumulado y heurística
                        nodes[neighbor] = (currentNode, tentativeAcumulativeCost, Distance(neighbor, destinationNode));

                        // Si el vecino no está en la lista abierta, lo añadimos
                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }
            }

            // Si no se encuentra un camino, devolvemos null
            return null;

            // Método auxiliar para generar el camino desde el nodo de inicio hasta el destino
            List<NodeType> GeneratePath(NodeType startNode, NodeType goalNode)
            {
                List<NodeType> path = new List<NodeType>();
                NodeType currentNode = goalNode;

                // Retroceder desde el nodo de destino hasta el nodo de inicio, siguiendo los padres
                while (!NodesEquals(currentNode, startNode))
                {
                    path.Add(currentNode);
                    currentNode = nodes[currentNode].Parent;
                }

                path.Add(startNode);
                path.Reverse(); // Invertimos el camino para que empiece desde el nodo de inicio
                return path;
            }
        }

        public void UpdateGrapf(Grapf<NodeType, CoordinateType, T> newGrapf)
        {
            actualGrapf = newGrapf;
        }
        protected abstract ICollection<NodeType> GetNeighbors(NodeType node);

        protected abstract float Distance(NodeType A, NodeType B);

        protected abstract bool NodesEquals(NodeType A, NodeType B);

        protected abstract float MoveToNeighborCost(NodeType A, NodeType b);

        protected abstract bool IsBloqued(NodeType node);
    }
}