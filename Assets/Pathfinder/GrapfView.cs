using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace FlyEngine
{
    public class GrapfView : MonoBehaviour
    {
        private bool active;

        private Grapf<Node<Vector2Coordinate, float>, Vector2Coordinate, float> grapf;

        public void Initialize(int xCount, int yCount, float nodeDistance, float initialX = 0.0f, float initialY = 0.0f)
        {
            grapf = new Grapf<Node<Vector2Coordinate, float>, Vector2Coordinate, float> 
            ( 
                xCount, 
                yCount, 
                nodeDistance, 
                initialX, 
                initialY 
            );
        }
        public void Initialize(Grapf<Node<Vector2Coordinate, float>, Vector2Coordinate, float> grapf)
        {
            this.grapf = grapf;
        }

        private void OnDrawGizmos()
        {

            if (grapf == null || grapf.GetNodes() == null || !Application.isPlaying || !active)
                return;

            foreach (KeyValuePair<(int x, int y), Node<Vector2Coordinate, float>> entry in grapf.GetNodes())
            {
                Node<Vector2Coordinate, float> node = entry.Value;
                Vector2Coordinate coord = node.GetCoordinate();

                if (node.IsBloqued())
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.green;

                Gizmos.DrawWireSphere(new Vector3(0.5f + coord.x, 0.5f + coord.y, 0), 0.1f);
            }

        }

        public void SetActive(bool isActive)
        {
            active = isActive;
            Debug.Log(active ? "Grapf is on" : "Grapf is off");
        }

    }
}