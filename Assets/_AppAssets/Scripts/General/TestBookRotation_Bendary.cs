using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class TestBookRotation_Bendary : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] bookCoverRenderers;
    [SerializeField] private MeshRenderer[] bookPapersRenderers;

    private bool rotationEnabled = false;
    private Animator myAnim;
    private Vector3 OrignalRot;

    #region Mouse Info
    private Vector3 prevPos = Vector3.zero;
    private Vector3 posDelta = Vector3.zero;
    #endregion

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        OrignalRot = transform.localEulerAngles;
    }

    private void Update()
    {
        // If right mouse Click pressed
        if (Input.GetMouseButton(1))
        {
            // Get the mouse pos Delta
            posDelta = Input.mousePosition - prevPos;

            // Pass Data to Rotate method in the shape
            RotateBook(posDelta);
        }

        // Get the current mouse pos
        prevPos = Input.mousePosition;
    }

    #region Helper
    public void ToggleRenderers(bool enabled)
    {
        foreach (MeshRenderer mesh in bookCoverRenderers)
        {
            mesh.enabled = enabled;
        }

        foreach (MeshRenderer mesh in bookPapersRenderers)
        {
            mesh.enabled = enabled;
        }
    }

    public void RotateBook(Vector3 posDelta)
    {
        if (rotationEnabled)
        {
            if (Vector3.Dot(transform.up, Vector3.up) >= 0)
            {
                // Rotate Around Y Axis
                transform.Rotate(transform.up, -Vector3.Dot(-posDelta, Camera.main.transform.right), Space.World);
            }
            else
            {
                // Rotate Around Y Axis but inverted
                transform.Rotate(transform.up, Vector3.Dot(-posDelta, Camera.main.transform.right), Space.World);
            }

            // Rotate Around X Axis
            transform.Rotate(Camera.main.transform.right, Vector3.Dot(posDelta, Camera.main.transform.up), Space.World);
        }
    }

    public void OpenBook()
    {
        rotationEnabled = true;
        myAnim.SetBool("IsBookOpen", rotationEnabled);
    }

    public void CloseBook()
    {
        rotationEnabled = false;
        myAnim.SetBool("IsBookOpen", rotationEnabled);
    }

    public void AssignCoverMaterials(Material coverMat)
    {
        foreach (MeshRenderer mesh in bookCoverRenderers)
        {
            mesh.material = coverMat;
        }
    }



    public void RotateToOrign(float delay, TweenCallback tw)
    {
        transform.DORotate(OrignalRot, delay, RotateMode.Fast).OnComplete(tw);
    }
    #endregion
}
