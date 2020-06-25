using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    float currentTime = 0;
    float timeMilestone = 0;
    const float minute = 60.0f;

    int playerScore = 0;

    public int blueprintCountMax = 6;
    public int blueprintCountMin = 3;

    [SerializeField]
    Slider detectionSlider;

    [SerializeField]
    float maxDetection = 100.0f;
    float detectionMetreValue = 0.0f;
    float detectionMetreMilestone = 0.0f;

    [SerializeField]
    Transform[] blueprintSpawns;

    [SerializeField]
    GameObject blueprintPrefab;

    [SerializeField]
    GameObject[] guardSpawns;

    [SerializeField]
    Text timer;

    [SerializeField]
    Text score;

    bool playerDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        int blueprintCount = Random.Range(blueprintCountMin, blueprintCountMax + 1);

        for(int i = 0; i < blueprintCount; ++i)
        {
            GameObject spawn = Instantiate(blueprintPrefab, blueprintSpawns[i]);
            spawn.transform.position += spawn.transform.up;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Return to menu
        }

        UpdateTimer();
        score.text = "Score: " + playerScore.ToString();

        if(!playerDetected)
        {
            detectionMetreValue = Mathf.Clamp(detectionMetreValue - Time.deltaTime * 5.0f, detectionMetreMilestone, maxDetection);
        }

        playerDetected = false;
    }

    public void PickupScore()
    {
        playerScore += 100;
    }

    public void PlayerDetected()
    {
        detectionMetreValue = Mathf.Clamp(detectionMetreValue + Time.deltaTime * 5.0F, detectionMetreMilestone, maxDetection);

        detectionSlider.value = detectionMetreValue;

        if (detectionMetreValue >= detectionMetreMilestone + 10.0f)
        {
            detectionMetreMilestone += 10.0f;
        }

        if(detectionMetreValue >= maxDetection)
        {
            //Go to game over
        }

        playerDetected = true;
    }

    void UpdateTimer()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= timeMilestone + minute)
        {
            timeMilestone = currentTime;
            playerScore -= 10;
        }

        float time = currentTime;
        string minutes = Mathf.Floor(time / 60.0f).ToString("00");
        string seconds = (time % 60.0f).ToString("00");
        timer.text = string.Format("{0}:{1}", minutes, seconds);
    }
}
