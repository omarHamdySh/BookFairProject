using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PathCreation.Examples
{
    public class ObjectAlignerOverPath_Mostafa : MonoBehaviour
    {
        private int points = 0;

        [SerializeField]
        private int pathpointIndex;

        private PathCreator pathCreator;// Must be the parent of scrollable objects

        public EndOfPathInstruction endOfPathInstruction;

        [SerializeField] private int verticesMultiplier;

        private IScrollable scrollable;

        float distanceTravelled;

        bool motionStarted = false;

        float currentScrollSpeed;

        public Vector3 activeLibraryPosition;
        public Quaternion activeLibraryRotation;
        public Quaternion sideLibraryRotation;

        private float maxThreshold; //for surruonding rotations
        private float minThreshold; // for preview library rotation

        void Start()
        {
            scrollable = GetComponent<IScrollable>();

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

            activeLibraryPosition = new Vector3(40, 30, 20);
            activeLibraryRotation = new Quaternion(1, 1, 1, 1);
            sideLibraryRotation = new Quaternion(0, 20, 0, 1);
            maxThreshold = 8;
            minThreshold = 4;

            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

        }

        void Update()
        {
            currentScrollSpeed = scrollable.getScrollSpeed();

            if (motionStarted && currentScrollSpeed == 0) // If the object is not moving, declare land State and fire land event
            {
                motionStarted = false;
                scrollable.onLand();
                return;
            }

            else if (currentScrollSpeed == 0)
            {
                return;
            }

            if (currentScrollSpeed > 0)
            {
                if (!motionStarted)
                {
                    motionStarted = true;
                    scrollable.onDeparture();
                }

                scrollable.onMoving();
            }

            
            distanceTravelled += currentScrollSpeed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            /*
            float tmpDistance = Vector3.Distance(transform.position, activeLibraryPosition);

          if (Vector3.Distance(transform.position, activeLibraryPosition) < minThreshold){

                transform.DORotate(activeLibraryRotation.eulerAngles, Mathf.Abs(currentScrollSpeed)*0.03f);
                //transform.rotation = activeLibraryRotation;
            }
            else
            {
                transform.DORewind();
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }*/
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            //distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}
