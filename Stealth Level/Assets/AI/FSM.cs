
using System.Collections.Generic;

public class FSM
{
    BaseAI agent;
    Dictionary<string, State> states;
    State currentState;

    public FSM(BaseAI agent, GameManager manager)
    {
        this.agent = agent;

        states = new Dictionary<string, State>();

        State state = new IdleState(agent, manager);
        states.Add("Idle", state);

        state = new SearchState(agent, manager);
        states.Add("Search", state);

        state = new AlertState(agent, manager);
        states.Add("Alert", state);

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
