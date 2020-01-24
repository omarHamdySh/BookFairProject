using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        PushState(bookCaseState);
        if (hintTxt)
        {
            hintTxt.enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        stateStack.Peek().OnStateUpdate();
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
        PopState();
        PushState(floorState);
    }
    public void toBookCaseState() {

        PopState();
        PushState(bookCaseState);
    }
    public void toShelfState()
    {
        PopState();
        PushState(shelfState);
    }
    public void toBookStae()
    {
        PopState();
        PushState(bookState);
    }
    public void toBookPageState()
    {
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
