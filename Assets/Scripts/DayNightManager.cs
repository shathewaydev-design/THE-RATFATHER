using UnityEngine;
using UnityEngine.UI;
using System;

public class DayNightManager : MonoBehaviour
{
    public static DayNightManager Instance;

    [Header("Time Settings")]
    public float dayDuration = 180f;
    public float nightDuration = 180f;

    private float timer;
    private bool isDay = true;

    [Header("Day Count")]
    public int currentDay = 1;
    public int maxDays = 5;

    [Header("UI")]
    public GameObject sunIcon;
    public GameObject moonIcon;
    public Text dayText;

    [Header("Lighting")]
    public Light dayLight;
    public Light nightLight;

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
            StartNight();
        }
        else if (!isDay && timer >= nightDuration)
        {
            StartNextDay();
        }

        UpdateLightRotation();
    }

    void StartDay()
    {
        isDay = true;
        timer = 0f;

        sunIcon.SetActive(true);
        moonIcon.SetActive(false);

        if (dayLight) dayLight.enabled = true;
        if (nightLight) nightLight.enabled = false;

        UpdateUI();

        OnNewDay?.Invoke(); // 🔥 Ingredient spawn trigger
    }

    void StartNight()
    {
        isDay = false;
        timer = 0f;

        sunIcon.SetActive(false);
        moonIcon.SetActive(true);

        if (dayLight) dayLight.enabled = false;
        if (nightLight) nightLight.enabled = true;
        
        OnNightStart?.Invoke();
    }

    void StartNextDay()
    {
        currentDay++;

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

    void UpdateLightRotation()
    {
        // OPTIONAL: smooth sun movement
        if (dayLight && isDay)
        {
            float t = timer / dayDuration;
            float angle = Mathf.Lerp(0, 180, t);
            dayLight.transform.rotation = Quaternion.Euler(angle, 0, 0);
        }

        if (nightLight && !isDay)
        {
            float t = timer / nightDuration;
            float angle = Mathf.Lerp(180, 360, t);
            nightLight.transform.rotation = Quaternion.Euler(angle, 0, 0);
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