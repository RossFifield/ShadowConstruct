using UnityEngine;
using UnityEngine.AI;

public class BaseAI : MonoBehaviour
{
    public GameObject player;
    FSM fsm;

    public float FOV = 90.0f;
    public float visionRange;

    public NavMeshAgent navMesh;

    public Transform[] patrolPoints;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fsm = new FSM(this);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
    }

    public bool SensePlayer()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, visionRange);

        foreach (Collider col in objectsInRange)
        {
            if (col.gameObject == player)
            {
                float angle = Vector3.Angle(transform.forward, transform.position - player.transform.position);

                if (angle <= FOV / 2)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
