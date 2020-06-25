
using System.Collections.Generic;
using UnityEngine;

public struct StateTransition
{
    public string targetState;
    public delegate bool condition();
    public condition transitionCondition;
}

public abstract class State
{
    protected BaseAI agent;
    public List<StateTransition> transitions = new List<StateTransition>();
    protected GameManager gameManager;

    public State(BaseAI agent, GameManager manager)
    {
        this.agent = agent;
        gameManager = manager;
    }

    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnExitState();
}

public class IdleState : State
{
    bool playerDetected = false;
    float timer = 15.0f;

    bool PlayerDetected()
    {
        return playerDetected;
    }
    public IdleState(BaseAI agent, GameManager manager) : base(agent, manager)
    {
        StateTransition tran = new StateTransition();
        tran.targetState = "Search";
        tran.transitionCondition += PlayerDetected;
        transitions.Add(tran);
    }

    public override void OnEnterState()
    {
        agent.navMesh.destination = agent.startingPos;
        timer = 15.0f;
    }

    public override void OnExitState()
    {
        playerDetected = false;
    }

    public override void OnUpdateState()
    {
        playerDetected = agent.SensePlayer();

        if(agent.transform.position == agent.navMesh.destination)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                timer = 15.0f;
                agent.navMesh.destination = agent.GetRandomMove();
            }

        }
    }
}

public class SearchState : State
{
    bool playerDetected = false;
    bool playerLost = false;

    float timer = 1.0f;

    bool PlayerDetected()
    {
        return playerDetected;
    }

    bool PlayerLost()
    {
        return playerLost;
    }

    public SearchState(BaseAI agent, GameManager manager) : base(agent, manager)
    {
        StateTransition tran = new StateTransition();
        tran.targetState = "Alert";
        tran.transitionCondition += PlayerDetected;
        transitions.Add(tran);

        tran = new StateTransition();
        tran.targetState = "Idle";
        tran.transitionCondition += PlayerLost;
        transitions.Add(tran);
    }

    public override void OnEnterState()
    {
        agent.navMesh.destination = agent.player.transform.position;
        timer = 1.0f;
    }

    public override void OnExitState()
    {
        playerDetected = false;
        playerLost = false;
        timer = 1.0f;
    }

    public override void OnUpdateState()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            if (agent.SensePlayer())
            {
                playerDetected = true;
            }
            else
            {
                playerLost = true;
            }
        }
    }
}

public class AlertState : State
{
    bool playerEscaped = false;

    bool PlayerEscaped()
    {
        return playerEscaped;
    }
    public AlertState(BaseAI agent, GameManager manager) : base(agent, manager)
    {
        StateTransition tran = new StateTransition();
        tran.targetState = "Search";
        tran.transitionCondition += PlayerEscaped;
        transitions.Add(tran);
    }

    public override void OnEnterState()
    {
    }

    public override void OnExitState()
    {
        playerEscaped = false;
    }

    public override void OnUpdateState()
    {
        agent.navMesh.destination = agent.player.transform.position;
        gameManager.PlayerDetected();
        
        if(Vector3.Distance(agent.transform.position, agent.player.transform.position) > agent.visionRange)
        {
            playerEscaped = true;
        }
    }
}