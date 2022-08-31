using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    protected StateMachine<T> stateMachine;
    protected T context;

    internal void SetStateMachineAndContext(StateMachine<T> stateMachine, T context)
    {
        this.stateMachine = stateMachine;
        this.context = context;

        Init();
    }

    public virtual void Init()
    {

    }

    public virtual void Enter()
    {

    }

    public abstract void Update(float deltaTime);

    public virtual void Exit()
    {

    }


}

public sealed class StateMachine<T>
{
    private T _context;

    public State<T> CurState { private get; set;}
    public State<T> PriState { private get; set; }

    public float ElapsedTimeInState { private get; set; } = 0.0f;

    private Dictionary<System.Type, State<T>> _states = new Dictionary<System.Type, State<T>>();

    public StateMachine(T context, State<T> initialState)
    {
        _context = context;
        AddState(initialState);
        CurState = initialState;
        CurState.Enter();
    }

    public void AddState(State<T> state)
    {
        state.SetStateMachineAndContext(this, _context);
        _states[state.GetType()] = state;
    }


    public void Update(float deltaTime)
    {
        ElapsedTimeInState += deltaTime;
        CurState.Update(deltaTime);
    }

    public R ChangeState<R>() where R : State<T>
    {
        var newType = typeof(R);
        //같은 타입이면 종료
        if (CurState.GetType() == newType)
        {
            return CurState as R;
        }
        //비어있지 않다면 exit
        if (CurState != null)
        {
            CurState.Exit();
        }

        PriState = CurState;
        CurState = _states[newType];
        CurState.Enter();
        ElapsedTimeInState = 0.0f;

        return CurState as R;
    }
}
