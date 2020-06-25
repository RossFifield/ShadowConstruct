
using System.Collections.Generic;
using UnityEngine;

public struct StateTransition
{
    public string targetState;
    public delegate bool condition();
    public condition transitionCondition;
}

public abstract class State : MonoBehaviour
{
    protected BaseAI agent;
    public List<StateTransition> transitions;
    protected GameManager gameManager;

    public State(BaseAI agent)
    {
        this.agent = agent;
        gameManager = GameObject.FindObjectOfType<GameManager>();
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
    public IdleState(BaseAI agent) : base(agent)
    {
        StateTransition tran = new StateTransition();
        tran.targetState = "Search";
        tran.transitionCondition += PlayerDetected;
        transitions.Add(tran);
    }

    public override void OnEnterState()
    {
        agent.navMesh.destination = agent.patrolPoints[Random.Range(0, agent.patrolPoints.Length)].position;
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
                agent.navMesh.destination = agent.patrolPoints[Random.Range(0, agent.patrolPoints.Length)].position;
            }
        }
    }
}

public class SearchState : State
{
    bool playerDetected = false;
    bool playerLost = false;

    bool PlayerDetected()
    {
        return playerDetected;
    }

    bool PlayerLost()
    {
        return playerLost;
    }

    public SearchState(BaseAI agent) : base(agent)
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
    }

    public override void OnExitState()
    {
        playerDetected = false;
        playerLost = false;
    }

    public override void OnUpdateState()
    {
        if(agent.transform.position == agent.navMesh.destination)
        {
            if (agent.SensePlayer())
            {
                RaycastHit hit;
                Physics.Linecast(agent.transform.position, agent.player.transform.position, out hit);
                if (hit.collider.gameObject == agent.player)
                {
                    playerDetected = true;
                }
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
    public AlertState(BaseAI agent) : base(agent)
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
        if(agent.SensePlayer())
        {
            agent.navMesh.destination = agent.player.transform.position;
            gameManager.PlayerDetected();
        }
        else
        {
            playerEscaped = true;
        }
    }
}