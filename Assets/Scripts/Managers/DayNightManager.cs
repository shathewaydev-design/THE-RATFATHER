using UnityEngine;
using UnityEngine.UI;
using System;

public class DayNightManager : MonoBehaviour
{
    public static DayNightManager Instance;

    [Header("Time Settings")]
    public float dayDuration = 360f;

    private float timer;
    private bool isDay = true;

    [Header("Day Count")]
    public int currentDay = 1;//current day of the week
    public int maxDays = 7;

    [Header("UI")]
    public GameObject sunIcon;
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
        timer += Time.deltaTime;

        if (isDay && timer >= dayDuration)
        {
            StartNextDay();
        }
    
    }

    void StartDay()
    {
        isDay = true;
        timer = 0f;

        sunIcon.SetActive(true);

        UpdateUI();

        OnNewDay?.Invoke(); // 🔥 Ingredient spawn trigger
    }

    public void StartNextDay()//run this function when day is over, and start next day.
    {
        currentDay++;
        WithdrawalMechanic.Instance.NewDay(); // partial cheese drain rate recovery
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