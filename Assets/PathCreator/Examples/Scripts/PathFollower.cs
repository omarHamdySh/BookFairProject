using UnityEngine;
using DG.Tweening;
namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        private int points = 0;
        [SerializeField] private int pathpointIndex;
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;

        public float duration = 1;
        bool clicked;

        void Start()
        {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                points = transform.GetSiblingIndex();
                pathpointIndex = points * 2;
                transform.position = pathCreator.path.GetPoint(pathpointIndex);
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !clicked)
                {
                    clicked = true;
                    pathpointIndex = (++points) * 2;

                    if (pathpointIndex >= pathCreator.path.NumPoints - 2)
                    {
                        points = 0;
                    }
                    transform.DOMove(pathCreator.path.GetPoint(pathpointIndex), duration).OnComplete(fireUnClick);
                }
            }
        }

        public void fireUnClick() {
          
            clicked = false;
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            //distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}