using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples
{
    [RequireComponent(typeof(PathCreator))]
    public class PathHandler : MonoBehaviour
    {
        [SerializeField]
        private PathDataSO pathData;

        private float speed;

        private enum scrollable
        {
            Floor,
            Bookcase,
            Shelf,
            Book
        };

        [SerializeField]
        private scrollable scrollableType;

        public float Speed { get => speed; set => speed = value; }

        private void Start()
        {
            switch (scrollableType)
            {
                case scrollable.Floor:
                    {
                        Speed = pathData.FloorScrollSpeed;
                    }
                    break;
                case scrollable.Bookcase:
                    {
                        Speed = pathData.BookcaseScrollSpeed;
                    }
                    break;
                case scrollable.Shelf:
                    {
                        Speed = pathData.ShelfScrollSpeed;
                    }
                    break;
                case scrollable.Book:
                    {
                        Speed = pathData.FloorScrollSpeed;
                    }
                    break;
                default:
                    {
                        Debug.LogError("Wrong scrollable type");
                    }
                    break;
            }
        }
    }
}
