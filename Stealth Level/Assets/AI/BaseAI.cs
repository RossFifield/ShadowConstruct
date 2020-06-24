using UnityEngine;

public class BaseAI : MonoBehaviour
{
    FSM fsm;
    // Start is called before the first frame update
    void Start()
    {
        fsm = new FSM(this);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
    }
}
