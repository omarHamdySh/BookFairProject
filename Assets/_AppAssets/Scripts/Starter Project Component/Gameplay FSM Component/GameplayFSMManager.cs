﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Lean.Touch;

/// <summary>
/// define the gamepaly states
/// Transition state controls the the transition between two states
/// washing state which the player clean the teeth after bacterias attack it (teeth)
/// fighting state which the start state of the player where he/she defends opposite bacteria
/// pause state which controling the pause status for opening/closing the menu
/// </summary>
public enum GameplayState
{
    Floor,
    Search,
    BookCase,
    Shelf,
    Book,
    BookPage,
    Pause
}

public class GameplayFSMManager : MonoBehaviour
{
    //Debug Variables
    public TextMeshProUGUI currentStateTxt;
    public TextMeshProUGUI hintTxt;

    public Transform pageStateColliderPos;

    /// <summary>
    /// Declaration of dynamic variables for surving the logic goes here.
    /// Eg.
    ///     public int chasingRange;
    ///     public int shootingRange;
    ///     public int alertRange;
    /// </summary>
    //define the stack which controlling the current state
    Stack<IGameplayState> stateStack = new Stack<IGameplayState>();

    /// <summary>
    /// Declaration of states Instances goes here.
    /// </summary>

    [HideInInspector]
    public FloorState floorState;

    [HideInInspector]
    public ShelfState shelfState;

    [HideInInspector]
    public BookCaseState bookCaseState;

    [HideInInspector]
    public BookState bookState;

    [HideInInspector]
    public BookPageState bookPageState;

    [HideInInspector]
    public PauseState pauseState;

    [HideInInspector]
    public SearchState searchState;

    //define a temp to know which the state the player come from it to pause state
    [HideInInspector]
    public IGameplayState tempFromPause;

    //
    [HideInInspector]
    public IGameplayState tempTransitionTo;

    [HideInInspector]
    public IGameplayState tempTransitionFrom;


    /// <summary>
    /// Declaration of references will be used for the states logic goes here
    /// Eg. 
    ///     public ISteer steeringScript;
    ///     public GameObject pathRoute;
    ///     public Queue<GameObject> enemyQueue = new Queue<GameObject>();
    /// 
    /// </summary>
    private void Start()
    {
        /// <summary>
        /// Instantiation of states Instances goes here.
        /// Eg.
        /// chaseEnemy = new ChaseState()
        ///        {
        ///     chasingRange = this.chasingRange,
        ///     shootingRange = this.shootingRange,
        ///     alertRange = this.alertRange,
        ///     movementController = this
        ///         };
        /// </summary>

        ////Instantiate the first state
        floorState = new FloorState()
        {
            gameplayFSMManager = this
        };

        bookCaseState = new BookCaseState()
        {
            gameplayFSMManager = this
        };

        searchState = new SearchState()
        {
            gameplayFSMManager = this
        };

        shelfState = new ShelfState()
        {
            gameplayFSMManager = this
        };

        bookState = new BookState()
        {
            gameplayFSMManager = this
        };


        pauseState = new PauseState()
        {
            gameplayFSMManager = this
        };


        bookPageState = new BookPageState()
        {
            gameplayFSMManager = this
        };

        //push the first state for the player
        PushState(floorState);
        //PushState(shelfState);
        //PushState(bookState);
        if (hintTxt)
        {
            hintTxt.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        stateStack.Peek().OnStateUpdate();
        if (Input.GetKeyDown(KeyCode.Escape) && !LevelUI.Instance.isUIOpen)
        {
            //if (SelectionManager.instance.canSelect)
            //{
            //    SelectionManager.instance.deselectCurrent();
            //}
        }
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !LevelUI.Instance.isUIOpen)
        {
            //if (SelectionManager.instance.canSelect)
            //{

            //    LeanFinger f = new LeanFinger();
            //    f.ScreenPosition.x = CameraPath.instance.cameraTransform.position.x;
            //    f.ScreenPosition.y = CameraPath.instance.cameraTransform.position.y;
            //    f.Set = true;
            //    f.TapCount = 1;
            //    Lean.Touch.LeanTouch.Fingers.Add(f);
            //}
        }
        Debug.Log(getCurrentState());
    }
    /// <summary>
    /// functions to define the stak functionality
    /// </summary>
    public void PopState()
    {
        if (stateStack.Count > 0)
            stateStack.Pop().OnStateExit();
    }
    public void PushState(IGameplayState newState)
    {
        newState.OnStateEnter();
        stateStack.Push(newState);

        if (currentStateTxt)
            currentStateTxt.text = stateStack.Peek().ToString();
    }


    public void holdTempTransitionTo(IGameplayState nextState)
    {

        tempTransitionTo = nextState;
        tempTransitionFrom = stateStack.Peek();
        #region -- Deprecated Crap 
        /** 
         switch (stateStack.Peek().GetState())
         {
             case GameplayState.Tutorial:
                 switch (nextState.GetState())
                 {
                     case GameplayState.Tutorial:
                         //Error You are mapping to the same sate
                         break;
                     case GameplayState.AssemblyDisassembly:
                         break;
                     case GameplayState.AssemblyDisassemblyTutorial:
                         break;
                     case GameplayState.Shooting:
                         break;
                     case GameplayState.Testing:
                         break;
                     case GameplayState.Transition:
                         break;
                     case GameplayState.Pause:
                         break;
                 }
                 break;
             case GameplayState.AssemblyDisassembly:
                 switch (nextState.GetState())
                 {
                     case GameplayState.Tutorial:
                         break;
                     case GameplayState.AssemblyDisassembly:
                         //Error You are mapping to the same sate
                         break;
                     case GameplayState.AssemblyDisassemblyTutorial:
                         break;
                     case GameplayState.Shooting:
                         break;
                     case GameplayState.Testing:
                         break;
                     case GameplayState.Pause:
                         break;
                 }
                 break;
             case GameplayState.AssemblyDisassemblyTutorial:
                 switch (nextState.GetState())
                 {
                     case GameplayState.Tutorial:
                         break;
                     case GameplayState.AssemblyDisassembly:
                         break;
                     case GameplayState.AssemblyDisassemblyTutorial:
                         //Error You are mapping to the same sate
                         break;
                     case GameplayState.Shooting:
                         break;
                     case GameplayState.Testing:
                         break;
                     case GameplayState.Pause:
                         break;
                 }
                 break;
             case GameplayState.Shooting:
                 switch (nextState.GetState())
                 {
                     case GameplayState.Tutorial:
                         break;
                     case GameplayState.AssemblyDisassembly:
                         break;
                     case GameplayState.AssemblyDisassemblyTutorial:
                         break;
                     case GameplayState.Shooting:
                         //Error You are mapping to the same sate
                         break;
                     case GameplayState.Testing:
                         break;
                     case GameplayState.Pause:
                         break;
                 }
                 break;
             case GameplayState.Testing:
                 switch (nextState.GetState())
                 {
                     case GameplayState.Tutorial:
                         break;
                     case GameplayState.AssemblyDisassembly:
                         break;
                     case GameplayState.AssemblyDisassemblyTutorial:
                         break;
                     case GameplayState.Shooting:
                         break;
                     case GameplayState.Testing:
                         //Error You are mapping to the same sate
                         break;
                     case GameplayState.Pause:
                         break;
                 }
                 break;
             case GameplayState.Pause:
                 switch (nextState.GetState())
                 {
                     case GameplayState.Tutorial:
                         break;
                     case GameplayState.AssemblyDisassembly:
                         break;
                     case GameplayState.AssemblyDisassemblyTutorial:
                         break;
                     case GameplayState.Shooting:
                         break;
                     case GameplayState.Testing:
                         break;
                     case GameplayState.Pause:
                         //Error You are mapping to the same sate
                         break;
                 }
                 break;
             default:
                 break;
         }**/
        #endregion
    }


    public void toFloorState()
    {
        Debug.Log("toFloorState()");
        PopState();
        PushState(floorState);
    }
    public void toBookCaseState()
    {
        Debug.Log("toBookCaseState()");
        PopState();
        PushState(bookCaseState);
    }
    public void toSearchState()
    {
        Debug.Log("toSearchState()");
        PopState();
        PushState(searchState);
    }
    public void toShelfState()
    {
        Debug.Log("toShelfState()");
        PopState();
        PushState(shelfState);
    }
    public void toBookState()
    {
        Debug.Log("toBookState()");
        PopState();
        PushState(bookState);
    }
    public void toBookPageState()
    {
        Debug.Log("toBookPageState()");
        PopState();
        PushState(bookPageState);
    }


    public void pauseGame()
    {
        if (tempFromPause == null)
        {
            tempFromPause = stateStack.Peek();
            PopState();
            PushState(pauseState);
        }

    }
    public void resumeGame()
    {
        if (tempFromPause != null)
        {
            PopState();
            PushState(tempFromPause);
            tempFromPause = null;
        }
    }

    //return the current state at the stack
    public GameplayState getCurrentState()
    {
        return stateStack.Peek().GetState();
    }

    

}
