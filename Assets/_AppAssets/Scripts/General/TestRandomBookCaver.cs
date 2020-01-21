using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRandomBookCaver : MonoBehaviour
{
    [SerializeField] private List<Sprite> bookCovers;

    [SerializeField] private SpriteRenderer[] covers;

    private void Start()
    {
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
        foreach (SpriteRenderer i in covers)
        {
            int index = Random.Range(0, randomIndexesList.Count);
            i.sprite = bookCovers[randomIndexesList[index]];
            randomIndexesList.RemoveAt(index);
        }
    }
}
