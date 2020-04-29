using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndlessSliderLoadingBar : MonoBehaviour
{
    [SerializeField] private Vector2[] waypoints;
    [SerializeField] private float delay;

    [SerializeField] private RectTransform progress;

    private int index = 0;
    private bool isArraived;

    private void Start()
    {
        progress.anchoredPosition = waypoints[index];
        StartCoroutine(MoveBetweenWayPoints());
    }

    IEnumerator MoveBetweenWayPoints()
    {
        while (true)
        {
            isArraived = false;
            index = (index + 1) % waypoints.Length;
            progress.DOAnchorPos(waypoints[index], delay).OnComplete(Arrived);
            yield return new WaitUntil(() => isArraived == true);
        }
    }

    private void Arrived()
    {
        isArraived = true;
    }
}
