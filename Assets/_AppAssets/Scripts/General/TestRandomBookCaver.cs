using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRandomBookCaver : MonoBehaviour
{
    [SerializeField] private List<Sprite> bookCovers;

    [SerializeField] private Transform[] covers;

    private List<Material> coverMaterials;

    private void Start()
    {
        foreach (Transform i in covers)
        {
            coverMaterials.Add(i.GetChild(0).GetComponent<MeshRenderer>().material);
        }

        RandomCovers();
    }

    public void RandomCovers()
    {
        // Create random index list 
        List<int> randomIndexesList = new List<int>();
        for (int i = 0; i < bookCovers.Count; i++)
        {
            randomIndexesList.Add(i);
        }

        // Assaign random sprite and remove it's index from the random index list
        foreach (Material i in coverMaterials)
        {
            int index = Random.Range(0, randomIndexesList.Count);
            i.mainTexture = bookCovers[randomIndexesList[index]].texture;
            randomIndexesList.RemoveAt(index);
        }
    }
}
