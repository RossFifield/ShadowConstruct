
using System.Collections.Generic;

public class FSM
{
    BaseAI agent;
    Dictionary<string, State> states;
    State currentState;

    public FSM(BaseAI agent)
    {
        this.agent = agent;

        states.Add("Idle", new IdleState(agent));
        states.Add("Search", new SearchState(agent));
        states.Add("Alert", new AlertState(agent));

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
            if(tran.transitionCondition.Invoke())
            {
                currentState.OnExitState();
                currentState = states[tran.targetState];
                currentState.OnEnterState();
            }
        }
    }
}
