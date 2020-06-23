using UnityEngine;

public class GameManager : MonoBehaviour
{
    float currentTime = 0;
    float timeMilestone = 0;
    const float minute = 60.0f;

    int playerScore = 0;

    public int blueprintCountMax = 6;
    public int blueprintCountMin = 3;

    [SerializeField]
    Transform[] blueprintSpawns;
    [SerializeField]
    GameObject blueprintPrefab;

    [SerializeField]
    GameObject[] guardSpawns;

    // Start is called before the first frame update
    void Start()
    {
        int blueprintCount = Random.Range(blueprintCountMin, blueprintCountMax + 1);

        for(int i = 0; i < blueprintCount; ++i)
        {
            Instantiate(blueprintPrefab, blueprintSpawns[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= timeMilestone + minute)
        {
            timeMilestone = currentTime;
            playerScore -= 10;
        }
    }

    public void PickupScore()
    {
        playerScore += 100;
    }
}
