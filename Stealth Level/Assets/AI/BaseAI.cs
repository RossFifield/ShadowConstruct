using UnityEngine;
using UnityEngine.AI;

public class BaseAI : MonoBehaviour
{
    public GameObject player;
    FSM fsm;

    public float FOV = 45.0f;
    public float visionRange = 20.0f;

    public NavMeshAgent navMesh;

    GameManager manager;

    public Vector3 startingPos;

    public float moveRadius = 50.0f;

    public Animator anim;

    Vector3 currentPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        manager = GameObject.FindObjectOfType<GameManager>();
        fsm = new FSM(this, manager);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();

        if (currentPos != transform.position)
        {
            anim.SetFloat("Movement", navMesh.speed);
        }
        else
        {
            anim.SetFloat("Movement", 0);
        }

        currentPos = transform.position;
    }

    public bool SensePlayer()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, visionRange);

        foreach (Collider col in objectsInRange)
        {
            if (col.gameObject == player)
            {
                float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);

                if (angle <= FOV / 2)
                {
                    RaycastHit hit;
                    Physics.Linecast(transform.position, player.transform.position, out hit);
                    if (hit.collider.gameObject == player)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public Vector3 GetRandomMove()
    {
        float RandomX = Random.Range(startingPos.x - moveRadius, startingPos.x + moveRadius);
        float RandomZ = Random.Range(startingPos.z - moveRadius, startingPos.z + moveRadius);

        return new Vector3(RandomX, transform.position.y, RandomZ);
    }
}
