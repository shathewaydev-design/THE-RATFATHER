using UnityEngine;
using UnityEngine.UI;
using System;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;
    WithdrawalMechanic withdrawalMechanicScript = WithdrawalMechanic.Instance;
    CheeseAbilitySystem cheeseAbilitySystemScript = CheeseAbilitySystem.Instance;

    [Header("Time Settings")]
    [SerializeField] private float dayDuration = 360f;
    [SerializeField] private float seconds;
    [SerializeField] private float minutes;

    [Header("Day Tracking")]

    private float timeLeft;
    private bool isDayActive = true;
    private bool isGameCompleted = false;
    public Level currentGameLevel;
    public enum Level
    {
        Slumps,
        NRY
    }

    [Header("Day Count")]
    public int currentDay = 1;//current day of the week
    [SerializeField] private int maxDays;
    [SerializeField] private int maxDaysAtSlumps = 3;
    [SerializeField] private int maxDaysAtNRY = 7;

    [Header("UI")]
    public Image timeLeftUI;
    public Text dayText;


    [Header("End Game - Soldato Boss")]
    public bool allSlumpsQuestsAreCompleted;// for Soldato Boss fight
    public GameObject gameOverUI;
    [Header("End Game - Casino Boss")]
    public bool allCasinoQuestsAreCompleted;//for Casino Boss fight

    public event Action OnNewDay;
    public event System.Action OnNightStart;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        StartDay();
    }

    private void Update()
    {
        if(isDayActive)
        {
            timeLeft -= Time.deltaTime;
            float fillAmount = timeLeft / dayDuration;
            timeLeftUI.fillAmount = fillAmount;

            seconds = Mathf.FloorToInt(timeLeft % 60);
            minutes = Mathf.FloorToInt(timeLeft / 60) % 60;
            if (timeLeft <= 0)
            {
                StartNextDay();
            }
        }
    
    }

    void StartDay()
    {
        
        switch (currentGameLevel)
        {
            case Level.Slumps:
                maxDays = maxDaysAtSlumps;
                break;
            case Level.NRY:
                maxDays = maxDaysAtNRY;
                break;
        }
        timeLeft = dayDuration;

        //timeLeftUI.SetActive(true);

        UpdateUI();

        OnNewDay?.Invoke(); //  Ingredient spawn trigger
    }

    public void StartNextDay()//run this function when day is over, and start next day.
    {
        currentDay++;
        if(withdrawalMechanicScript != null)
        {
            withdrawalMechanicScript.NewDay(); // partial cheese drain rate recovery
        }
        if(cheeseAbilitySystemScript != null)
        {
            cheeseAbilitySystemScript.ResetDailyCheeseLimits(); // reset daily cheese consumption limits
        }

        if (currentDay > maxDays)
        {
            EndWeek();
            return;
        }

        StartDay();
    }

    void UpdateUI()
    {
        dayText.text = "Day " + currentDay;
    }

    public void AddTime(float additionalTimeFromDownerChese)
    {
        timeLeft += additionalTimeFromDownerChese;
        if (timeLeft >= dayDuration)
        {
            timeLeft = dayDuration;
        }
    }
    public void ResumeDay()
    {
        //call this function when player finishes talking to informant
        isDayActive = true;
    }
    public void PauseDay()
    {
        //call this function when player first go into NRY
        isDayActive = false;
    }

    void EndWeek()
    {
        if (allSlumpsQuestsAreCompleted)
        {
            FightSlumpsBoss();
        }
        else if (allCasinoQuestsAreCompleted)
        {
            FightCasinoBoss();
        }
        else
        {
            GameOver();
        }
    }

    void FightSlumpsBoss()
    {
        Debug.Log("Prepare for the final showdown with a Slumps Boss!");
        // move player to next level
        //hook with a Game Manager for Slumps Boss fight
        ResetWeek();
    }

    void FightCasinoBoss()
    {
        Debug.Log("Prepare for the final showdown with a Casino Boss!");
        // move player to next level
        //hook with a Game Manager for Casino Boss fight
        ResetWeek();
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);
        ResetWeek();
    }

    void ResetWeek()
    {
        currentDay = 1;
        StartDay();
    }
}