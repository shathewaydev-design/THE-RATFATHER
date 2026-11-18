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

    private float timeLeft;
    private bool isDay = true;
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


    [Header("End Game")]
    public bool allQuestsAreCompleted;
    public GameObject gameOverUI;

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
        timeLeft -= Time.deltaTime;
        float fillAmount = timeLeft / dayDuration;
        timeLeftUI.fillAmount = fillAmount;

        seconds = Mathf.FloorToInt(timeLeft % 60);
        minutes = Mathf.FloorToInt(timeLeft / 60) % 60;
        if (isDay && timeLeft <= 0)
        {
            StartNextDay();
        }
    
    }

    void StartDay()
    {
        isDay = true;
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

    void EndWeek()
    {
        if (allQuestsAreCompleted)
        {
            FightMafiaBoss();
        }
        else
        {
            GameOver();
        }
    }

    void FightMafiaBoss()
    {
        Debug.Log("Prepare for the final showdown with a Mafia Boss!");
        // teleport player?
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