using System.Collections.Generic;
using UnityEngine;

namespace FlyEngine
{
    public class GrapfView : MonoBehaviour
    {

        private Grapf<Node<Vector2Coordinate>, Vector2Coordinate> grapf;

        public GrapfView(Grapf<Node<Vector2Coordinate>, Vector2Coordinate> grapf)
        {
            this.grapf = grapf;
        }

        private void OnDrawGizmos()
        {
            if (grapf == null || grapf.GetNodes() == null || !Application.isPlaying)
                return;

            foreach (KeyValuePair<(int x, int y), Node<Vector2Coordinate>> entry in grapf.GetNodes())
            {
                Node<Vector2Coordinate> node = entry.Value;
                Vector2Coordinate coord = node.GetCoordinate();

                if (node.IsBloqued())
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.green;

                Gizmos.DrawWireSphere(new Vector3(coord.x, coord.y, 0), 0.1f);
            }
        }

    }
}