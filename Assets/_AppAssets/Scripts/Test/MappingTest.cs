using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MappingTest : MonoBehaviour
{
    public List<GameObject> objs;
    private void Start()
    {
        StartCoroutine(test());
    }
    IEnumerator test()
    {
        int oddIndexFatcor = (int)((objs.Count / 2f) - .5f); //Less than the center index with 1
        int evenIndexFactor = (int)((objs.Count / 2f) + .5f); //Center Index; [Zero]

        for (int i = 0; i < objs.Count; i++)
        {
            var obj = objs[mapBooksIndicies(i, oddIndexFatcor, evenIndexFactor, out oddIndexFatcor, out evenIndexFactor)];
            obj.transform.DOMove(new Vector3(obj.transform.position.x, -1, obj.transform.position.z), 0.5f, false);
            yield return new WaitForSeconds(0.6f);
        }
    }
    public int mapBooksIndicies(int index, int oddIndexFatcor, int evenIndexFactor, out int oddIndexFatcorOut, out int evenIndexFactorOut)
    {
        if (objs.Count % 2 == 0)
        {//Avoid excuting the algorithm because the list isn't odd number
            oddIndexFatcorOut = oddIndexFatcor;
            evenIndexFactorOut = evenIndexFactor;
            return index;
        }


        if (index % 2 == 0)
        {//even
            if (index == 0)
            {
                oddIndexFatcorOut = oddIndexFatcor;
                evenIndexFactorOut = evenIndexFactor;
                return evenIndexFactor - 1;
            }

        oddIndexFatcorOut = oddIndexFatcor;
        evenIndexFactorOut = ++evenIndexFactor;
        return evenIndexFactor - 1;

        }
        else
        {//odd

            oddIndexFatcorOut = oddIndexFatcor-1;
            evenIndexFactorOut = evenIndexFactor;
            return oddIndexFatcor-1;
        }

        #region Deprecated

        //switch (index)
        //{
        //    case 0:
        //        return 4;
        //    case 1:
        //        return 3;
        //    case 2:
        //        return 5;
        //    case 3:
        //        return 2;
        //    case 4:
        //        return 6;
        //    case 5:
        //        return 1;
        //    case 6:
        //        return 7;
        //    case 7:
        //        return 0;
        //    case 8:
        //        return 8;
        //    default:
        //        return 4;
        //}
        
        #endregion
    }

}
