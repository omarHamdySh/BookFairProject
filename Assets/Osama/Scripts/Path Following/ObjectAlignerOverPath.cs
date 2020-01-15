using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PathCreation.Examples
{
    public class ObjectAlignerOverPath : MonoBehaviour
    {
        private int points = 0;
        [SerializeField] private int pathpointIndex;
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;

        public float duration = 1;
        bool clicked;

        public float speed = 5;

        float distanceTravelled;
        float initialTravelledDistance;

        void Start()
        {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                points = transform.GetSiblingIndex();
                pathpointIndex = points * 15;
                transform.position = pathCreator.path.GetPoint(pathpointIndex);
            }

            distanceTravelled += pathCreator.path.GetClosestDistanceAlongPath(transform.position) +speed * Time.deltaTime;
        }

        void Update()
        {
            //if (pathCreator != null)
            //{
            //    if (Input.GetKeyDown(KeyCode.Space) && !clicked)
            //    {
            //        clicked = true;
            //        pathpointIndex = (++points) * 15;

            //        if (pathpointIndex >= pathCreator.path.NumPoints - 15)
            //        {
            //            points = 0;
            //        }
            //        transform.DOMove(pathCreator.path.GetPoint(pathpointIndex), duration).OnComplete(fireUnClick);

            //    }
            //}

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
