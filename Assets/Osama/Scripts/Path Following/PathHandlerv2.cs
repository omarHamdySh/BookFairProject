using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathHandlerv2 : MonoBehaviour
{
    #region Private Varibales

    [SerializeField] private List <ObjectPathTransform> objectPathTransform;

    private float currentScrollSpeed;

    private bool motionStarted = false;

    #endregion

    #region Public Varibales

    public IScrollable[] scrollables;

    public enum ObjectType
    {
        Shelf,
        Book
    };

    public ObjectType objectTypeToLookFor;

    #endregion

    #region MonoBehaviours

    private void Start()
    {
        scrollables = assignScrollables();
        //objectPathTransform = GetComponentsInChildren<ObjectPathTransform>();
    }

    private void Update()
    {
        currentScrollSpeed = scrollables[1].getScrollSpeed();

        if (motionStarted && currentScrollSpeed == 0)// if the objects is not moving, declare land State and fire land event
        {
            motionStarted = false;

            foreach (var scrollable in scrollables)// Change each scrollable State to --> onLand()
            {
                scrollable.onLand();
            }
            return;
        }
        else if (currentScrollSpeed == 0)// If scrolling speed reaches 0, return to skip frame
        {
            return;
        }

        if (currentScrollSpeed != 0)
        {
            if (!motionStarted)
            {
                motionStarted = true;

                foreach (var scrollable in scrollables)// Change each scrollable State to --> onDeparture()
                {
                    scrollable.onDeparture();
                }
            }

            if (currentScrollSpeed != 0)
            {
                foreach (var scrollable in scrollables)
                {
                    if (currentScrollSpeed > 0)
                    {
                        int nextTransformIndex;
                        Vector3 newDestination;

                        if (scrollable.getLandStatus())
                        {
                            nextTransformIndex = (scrollable.getObjectIndex() + 1) % objectPathTransform.Count;

                            scrollable.setObjectIndex(nextTransformIndex);

                            newDestination = objectPathTransform[nextTransformIndex].transform.position;

                            if (nextTransformIndex == 0)
                            {
                                scrollable.move(newDestination, currentScrollSpeed, false);
                            }
                            else
                            {
                                scrollable.move(newDestination, currentScrollSpeed);
                            }
                        }
                    }

                    else
                    {
                        int nextTransformIndex;
                        Vector3 newDestination;

                        if (scrollable.getLandStatus())
                        {
                            nextTransformIndex = ((objectPathTransform.Count - 1) + scrollable.getObjectIndex()) % objectPathTransform.Count;

                            scrollable.setObjectIndex(nextTransformIndex);

                            newDestination = objectPathTransform[nextTransformIndex].transform.position;
                            scrollable.move(newDestination, -currentScrollSpeed);
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Private Methods

    private IScrollable[] assignScrollables()// Depending on the object type defined in the enum "ObjectType", gets the apropriate children to the array of scrollables
    {
        switch (objectTypeToLookFor)
        {
            case ObjectType.Shelf:
                {
                    Debug.Log("Shelf Shelf");
                    assignTransforms(ObjectPathTransform.TransformType.Shelf);
                    return GetComponentsInChildren<Shelf>();
                }
                break;

            case ObjectType.Book:
                {
                    Debug.Log("Book Book");
                    assignTransforms(ObjectPathTransform.TransformType.Book);
                    return GetComponentsInChildren<Book>();
                }
                break;

            default:
                {
                    Debug.Log("Wrong type detected");
                    return null;
                }
                break;
        }
    }

    private void assignTransforms(ObjectPathTransform.TransformType objectType)
    {
        var transformWithApropriateType = GetComponentsInChildren<ObjectPathTransform>().Where(anObt => anObt.transformType == objectType);

        foreach (var item in transformWithApropriateType)
        {
            objectPathTransform.Add(item);
        }
    }

    #endregion
}