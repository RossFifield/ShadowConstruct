
using System.Collections.Generic;

public class FSM
{
    BaseAI agent;
    Dictionary<string, State> states;
    State currentState;

    public FSM(BaseAI agent)
    {
        this.agent = agent;
        currentState = states["Idle"];
        currentState.OnEnterState();
    }

    public void Update()
    {
        currentState.OnUpdateState();
        PollTransition();
    }

    void PollTransition()
    {
        foreach(StateTransition tran in currentState.transitions)
        {
            if(tran.transitionCondition)
            {
                currentState.OnExitState();
                currentState = states[tran.targetState];
                currentState.OnEnterState();
            }
        }
    }
}
