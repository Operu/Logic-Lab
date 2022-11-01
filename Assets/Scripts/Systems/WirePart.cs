using UnityEngine;

namespace Systems
{
    public class WirePart : MonoBehaviour
    {
        private Wire parent;
        private EdgeCollider2D edgeCollider2D;

        private Vector3 origin;
        private Vector3 destination;

        public void Initialize(Wire newParent)
        {
            parent = newParent;
            edgeCollider2D = GetComponent<EdgeCollider2D>();
            edgeCollider2D.points = new Vector2[] {origin, destination};
        }

        public void PositionLine(Vector3 inOrigin, Vector3 inDestination)
        {
            origin = inOrigin;
            destination = inDestination;

            if (inOrigin.x > inDestination.x)
            {
                origin = inDestination;
                destination = inOrigin;
            }

            if (inOrigin.y > inDestination.y)
            {
                origin = inDestination;
                destination = inOrigin;
            }

            transform.position = (origin + destination) / 2;

            Vector3 direction = destination - origin;
            if (direction.x > direction.y)
            {
                direction.y = 0.2f;
                direction.x += 0.2f;
                transform.localScale = direction;
            }
            else
            {
                direction.x = 0.2f;
                direction.y += 0.2f;
                transform.localScale = direction;
            }
        }

    }
}