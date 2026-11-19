
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayTimeUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DayManager dayManager;

    [Header("Optional UI Elements")]
    [SerializeField] private TextMeshProUGUI clockText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private Image radialTimer;

    private void Start()
    {
        if (dayManager == null)
            dayManager = DayManager.Instance;

        SubscribeToEvents();
        RefreshUI();
    }

    private void OnEnable()
    {
        // Handles re-enabling this UI after Start has run.
        if (dayManager != null)
            SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        if (dayManager == null)
            return;

        // Remove first to prevent duplicate subscriptions.
        dayManager.OnTimeChanged -= UpdateClock;
        dayManager.OnTimeProgressChanged -= UpdateRadialTimer;
        dayManager.OnDayChanged -= UpdateDayText;

        dayManager.OnTimeChanged += UpdateClock;
        dayManager.OnTimeProgressChanged += UpdateRadialTimer;
        dayManager.OnDayChanged += UpdateDayText;
    }

    private void RefreshUI()
    {
        if (dayManager == null)
            return;

        UpdateClock(dayManager.GetCurrentTimeInMinutes());
        UpdateDayText(dayManager.currentDay);
        UpdateRadialTimer(dayManager.TimeRemainingNormalized);
    }

    private void UpdateClock(int totalMinutes)
    {
        if (clockText == null)
            return;

        int hour = totalMinutes / 60;
        int minute = totalMinutes % 60;

        string period = hour >= 12 ? "PM" : "AM";

        int displayHour = hour % 12;

        if (displayHour == 0)
            displayHour = 12;

        clockText.text = $"{displayHour}:{minute:00} {period}";
    }

    private void UpdateDayText(int day)
    {
        if (dayText != null)
            dayText.text = $"Day {day}";
    }

    private void UpdateRadialTimer(float fillAmount)
    {
        if (radialTimer != null)
            radialTimer.fillAmount = fillAmount;
    }

    private void OnDisable()
    {
        if (dayManager == null)
            return;

        dayManager.OnTimeChanged -= UpdateClock;
        dayManager.OnTimeProgressChanged -= UpdateRadialTimer;
        dayManager.OnDayChanged -= UpdateDayText;
    }
}
