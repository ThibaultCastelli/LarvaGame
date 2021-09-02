using UnityEngine;

public class StateMachine : MonoBehaviour
{
    #region Variables
    public IState CurrentState { get; private set; }
    public IState PreviousState { get; private set; }
    #endregion

    #region Starts & Updates
    protected virtual void Update()
    {
        if (CurrentState != null)
            CurrentState.Tick();
    }

    protected virtual void FixedUpdate()
    {
        if (CurrentState != null)
            CurrentState.FixedTick();
    }
    #endregion

    #region Functions
    public void SetState(IState state)
    {
        if (state == CurrentState)
            return;

        if (CurrentState != null)
            CurrentState.OnExit();

        PreviousState = CurrentState;
        CurrentState = state;

        CurrentState.OnEnter();
    }
    #endregion
}
