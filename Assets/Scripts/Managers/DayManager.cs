
using UnityEngine;
using System;

public class DayManager : MonoBehaviour, IDataPersistence
{
    public static DayManager Instance;

    [Header("Time Settings")]
    [SerializeField, Min(0.01f)] private float dayDuration = 360f;

    [Header("In-Game Clock")]
    [SerializeField, Range(0, 23)] private int startingHour = 8;
    [SerializeField, Range(0, 59)] private int startingMinute = 0;

    [SerializeField, Range(0, 23)] private int endingHour = 18;
    [SerializeField, Range(0, 59)] private int endingMinute = 0;

    [SerializeField, Min(1)] private int timeIncrementMinutes = 10;

    [Header("Day Tracking")]
    [SerializeField] private float timeLeft;
    private bool isDayActive = true;

    public int currentDay = 1;

    private bool hasLoadedData = false;

    public enum Level
    {
        Slumps,
        Soldato,
        NRY,
        CasinoBoss
    }

    public Level currentGameLevel;

    [Header("Day Count")]
    [SerializeField] private int maxDays;
    [SerializeField] private int maxDaysAtSlumps = 3;
    [SerializeField] private int maxDaysAtNRY = 7;

    [Header("End Game - Soldato Boss")]
    public bool allSlumpsQuestsAreCompleted;
    public GameObject gameOverUI;

    [Header("End Game - Casino Boss")]
    public bool allCasinoQuestsAreCompleted;

    // UI and gameplay events
    public event Action<int> OnTimeChanged;
    public event Action<float> OnTimeProgressChanged;
    public event Action<int> OnDayChanged;

    //for spawning ingredients.
    public event Action OnNewDay;
    public event Action OnNightStart;

    private int lastReportedTime = -1;

    private int StartingTimeInMinutes
    {
        get
        {  
            return startingHour * 60 + startingMinute;
        }
    }

    private int EndingTimeInMinutes
    {
        get
        {
            return endingHour * 60 + endingMinute;
        }
    }

    private int TotalGameMinutes 
    {
        get
        {
            return EndingTimeInMinutes - StartingTimeInMinutes;
        }
    }
    public float TimeRemainingNormalized
    {
        get
        {
            return dayDuration > 0f ? timeLeft / dayDuration : 0f;
        }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if(!hasLoadedData)
        {
            StartDay();
        }
        else
        {
            ResumeLoadedDay();
        }

    }

    private void Update()
    {
        if (!isDayActive)
            return;

        timeLeft -= Time.deltaTime;
        timeLeft = Mathf.Max(0f, timeLeft);

        // Notify radial timer UI.
        OnTimeProgressChanged?.Invoke(timeLeft / dayDuration);

        // Notify digital clock only when its displayed time changes.
        int currentTime = GetCurrentTimeInMinutes();

        if (currentTime != lastReportedTime)
        {
            lastReportedTime = currentTime;
            OnTimeChanged?.Invoke(currentTime);
        }

        if (timeLeft <= 0f)
        {
            StartNextDay();
        }
    }

    public int GetCurrentTimeInMinutes()
    {
        if (TotalGameMinutes <= 0 || dayDuration <= 0f)
            return StartingTimeInMinutes;

        float elapsedTime = dayDuration - timeLeft;

        float elapsedGameMinutes = (elapsedTime / dayDuration) * TotalGameMinutes;

        // Snap the clock to the chosen increment, e.g. every 10 minutes.
        int roundedGameMinutes = Mathf.FloorToInt(elapsedGameMinutes / timeIncrementMinutes)* timeIncrementMinutes;

        // Make sure the clock reaches the exact ending time.
        roundedGameMinutes = Mathf.Min(roundedGameMinutes, TotalGameMinutes);

        return StartingTimeInMinutes + roundedGameMinutes;
    }

    private void StartDay()
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

        // Validate the clock configuration.
        if (TotalGameMinutes <= 0)
        {
            Debug.LogError("Ending time must be later than starting time.");
            isDayActive = false;
            return;
        }

        timeLeft = dayDuration;
        isDayActive = true;
        lastReportedTime = -1;
        //notify Ingredient Spawner
        OnDayChanged?.Invoke(currentDay);
        OnNewDay?.Invoke();

        // Initial UI values.
        OnTimeChanged?.Invoke(StartingTimeInMinutes);
        OnTimeProgressChanged?.Invoke(1f);
    }
    private void ResumeLoadedDay()
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

        // Validate the clock configuration.
        if (TotalGameMinutes <= 0)
        {
            Debug.LogError("Ending time must be later than starting time.");
            isDayActive = false;
            return;
        }

        isDayActive = true;
        lastReportedTime = -1;

        // int currentTime = GetCurrentTimeInMinutes();

        // Initial UI values.
        OnTimeChanged?.Invoke(StartingTimeInMinutes);
        OnTimeProgressChanged?.Invoke(1f);
    }

    public void StartNextDay()
    {
        if (!isDayActive)
            return;

        isDayActive = false;
        OnNightStart?.Invoke();

        // Retrieve singleton references when needed, not during field initialization.
        WithdrawalMechanic withdrawal = WithdrawalMechanic.Instance;

        if (withdrawal != null)
            withdrawal.NewDay();

        CheeseAbilitySystem cheeseSystem = CheeseAbilitySystem.Instance;

        if (cheeseSystem != null)
            cheeseSystem.ResetDailyCheeseLimits();

        currentDay++;

        if (currentDay > maxDays)
        {
            EndWeek();
            return;
        }

        StartDay();
    }

    public void AddTime(float additionalTime)
    {
        timeLeft += additionalTime;
        timeLeft = Mathf.Min(timeLeft, dayDuration);

        // Refresh the clock and radial display after adding time.
        int currentTime = GetCurrentTimeInMinutes();

        if (currentTime != lastReportedTime)
        {
            lastReportedTime = currentTime;
            OnTimeChanged?.Invoke(currentTime);
        }

        OnTimeProgressChanged?.Invoke(timeLeft / dayDuration);
    }

    public void ResumeDay()
    {
        isDayActive = true;
    }

    public void PauseDay()
    {
        isDayActive = false;
    }

    private void EndWeek()
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

    private void FightSlumpsBoss()
    {
        Debug.Log("Prepare for the final showdown with a Slumps Boss!");
        ResetWeek();
    }

    private void FightCasinoBoss()
    {
        Debug.Log("Prepare for the final showdown with a Casino Boss!");
        ResetWeek();
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        ResetWeek();
    }

    private void ResetWeek()
    {
        currentDay = 1;
        StartDay();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void LoadData(GameData data)
    {
        this.currentDay = data.currentDay;
        this.timeLeft = data.timeLeft;
        //this.currentGameLevel = data.currentGameLevel;
    }
    public void SaveData(ref GameData data)
    {
        data.currentDay = this.currentDay;
        data.timeLeft = this.timeLeft;
        //data.currentGameLevel = this.currentGameLevel;
    }
}
