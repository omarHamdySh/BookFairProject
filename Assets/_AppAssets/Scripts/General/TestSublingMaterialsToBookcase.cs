using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSublingMaterialsToBookcase : MonoBehaviour
{
    [SerializeField] private Material[] bookMaterials;
    [SerializeField] private Sprite[] bookCovers;

    private void Start()
    {
        Book_Bendary[] books = GetComponentsInChildren<Book_Bendary>();
        for (int i = 0; i < books.Length; i++)
        {
            books[i].bookBodyMeshRenderer.material = bookMaterials[i];
            books[i].bookBodyMeshRenderer.material.mainTexture = bookCovers[Random.Range(0, bookCovers.Length)].texture;
        }
    }
}
