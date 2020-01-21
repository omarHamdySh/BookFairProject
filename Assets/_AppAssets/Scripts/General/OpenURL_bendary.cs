using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL_bendary : MonoBehaviour
{
    [SerializeField] private string url;

    public void OpenURL(string Url)
    {
        Application.OpenURL(Url);
    }

    private void OnMouseDown()
    {
        OpenURL(url);
    }
}
