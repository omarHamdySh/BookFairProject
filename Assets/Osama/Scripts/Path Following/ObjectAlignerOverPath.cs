using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PathCreation.Examples
{
    public class ObjectAlignerOverPath : MonoBehaviour
    {
        private int points = 0;

        [SerializeField]
        private int pathpointIndex;

        private PathCreator pathCreator;// Must be the parent of scrollable objects

        public EndOfPathInstruction endOfPathInstruction;

        [SerializeField] private int verticesMultiplier;

        public float duration = 1;
        bool clicked;

        public float speed = 5;

        float distanceTravelled;

        void Start()
        {
            pathCreator = transform.parent.GetComponent<PathCreator>();

            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                points = transform.GetSiblingIndex();
                pathpointIndex = points * verticesMultiplier;
                transform.position = pathCreator.path.GetPoint(pathpointIndex);
            }
            distanceTravelled += pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        void Update()
        {
            if(pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        public void fireUnClick()
        {
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
