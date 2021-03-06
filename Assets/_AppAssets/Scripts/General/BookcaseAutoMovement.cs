﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookcaseAutoMovement : MonoBehaviour
{
    [SerializeField] private float waitSeconds = 0;
    [SerializeField] private float delayAfterMove = 0;
    [SerializeField] private BookcasePathHandller_Bendary bookcasePathHandller;

    private Vector3 lastMousePos;
    private float waitingSeconds = 0;
    private AutoMoveMode isAutoMoveOn = AutoMoveMode.ready;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (Input.mousePosition == lastMousePos &&
           GameManager.Instance.gameplayFSMManager.getCurrentState() == GameplayState.Floor &&
           !LevelUI.Instance.isUIOpen &&
           !CameraPath.instance.cameraMoving &&
           !Input.GetKeyDown(KeyCode.Escape) &&
           !Input.GetKeyDown(KeyCode.Return) &&
           !Input.GetKeyDown(KeyCode.KeypadEnter) &&
           gameManager.pathData.FloorScrollSpeed == 0
           )
        {
            waitingSeconds += Time.deltaTime;
            if (waitingSeconds >= waitSeconds && isAutoMoveOn == AutoMoveMode.ready)
            {
                isAutoMoveOn = AutoMoveMode.running;

                // Call Move Bookcases on random diraction
                bookcasePathHandller.MoveBookcaseAutomatic(delayAfterMove);
            }
        }
        else
        {
            bookcasePathHandller.StopAutoMoveCoroutine();
            isAutoMoveOn = AutoMoveMode.ready;
            lastMousePos = Input.mousePosition;
            waitingSeconds = 0;
        }
    }

    private enum AutoMoveMode
    {
        running,
        ready
    }
}
