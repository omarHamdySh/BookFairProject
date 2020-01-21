using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectAlingerOverPathv2 : MonoBehaviour
{
    [SerializeField]
    private int objectIndex;

    public int ObjectIndex { get => objectIndex; set => objectIndex = value; }


    private void Awake()
    {
        objectIndex = transform.GetSiblingIndex();
    }

    public void DOMOve(Vector3 destination)
    {
        transform.DOMove(destination,3);
        //objectIndex++;
    }
}
