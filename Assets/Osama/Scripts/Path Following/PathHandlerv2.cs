using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHandlerv2 : MonoBehaviour
{
    [Range(-1, 1)] [SerializeField] private int direaction;
    [SerializeField] private ObjectAlingerOverPathv2[] objectsOverPath;
    [SerializeField] private ShelfPathTransforms[] shelfPathTransforms;


    private void Start()
    {
        alignSHelvesOverPath();
    }

    [ContextMenu("Bendaaar")]
    private void alignSHelvesOverPath()
    {
        objectsOverPath = GetComponentsInChildren<ObjectAlingerOverPathv2>();

        shelfPathTransforms = GetComponentsInChildren<ShelfPathTransforms>();

        foreach (var obj in objectsOverPath)
        {
            if (direaction > 0)
            {
                
                Vector3 newDistenation = shelfPathTransforms[(obj.ObjectIndex + 1) % shelfPathTransforms.Length].transform.position;
                obj.ObjectIndex = (obj.ObjectIndex + 1) % shelfPathTransforms.Length;
                obj.DOMOve(newDistenation);
            }
            else if (direaction < 0)
            {

            }
        }
    }
}