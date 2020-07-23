using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private FixTextMeshPro tutorialTxt;
    [SerializeField] private TutorialLine[] tutorialLines;

    private List<int> randomIndexList = new List<int>();
    private float timer;
    private bool goToNextTxt;

    private void Start()
    {
        GenerateRandom();
        timer = tutorialLines[randomIndexList[0]].delay;
        StartCoroutine(ShowTutorial());
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    IEnumerator ShowTutorial()
    {
        int i = 0;

        while (true)
        {
            tutorialTxt.SetText((PlayerPrefs.GetString(ImportantStrings.langPPKey).Equals(ImportantStrings.arabicPPValue)) ?
                tutorialLines[randomIndexList[i]].arline :
                tutorialLines[randomIndexList[i]].enline);
            yield return new WaitUntil(() => ((timer <= 0) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || goToNextTxt));
            i = (i + 1) % randomIndexList.Count;
            timer = tutorialLines[randomIndexList[i]].delay;
            goToNextTxt = false;
        }
    }

    public void GoToNextText()
    {
        goToNextTxt = true;
    }

    private void GenerateRandom()
    {
        List<int> listIndexPool = new List<int>();
        for (int i = 0; i < tutorialLines.Length; i++)
        {
            listIndexPool.Add(i);
        }

        int index = 0;
        randomIndexList.Clear();
        for (int i = 0; i < tutorialLines.Length; i++)
        {
            index = Random.Range(0, listIndexPool.Count);
            randomIndexList.Add(listIndexPool[index]);
            listIndexPool.RemoveAt(index);
        }
    }
}
[System.Serializable]
public class TutorialLine
{
    public float delay;
    [Multiline] public string arline;
    [Multiline] public string enline;
}