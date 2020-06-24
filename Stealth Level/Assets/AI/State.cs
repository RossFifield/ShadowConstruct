
using System.Collections.Generic;

public struct StateTransition
{
    public string targetState;
    public bool transitionCondition;
}

public abstract class State
{
    BaseAI agent;
    public List<StateTransition> transitions;

    State(BaseAI agent)
    {
        this.agent = agent;
    }

    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnExitState();
}
