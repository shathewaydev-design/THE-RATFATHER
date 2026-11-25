using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Diagnostics;
public class AllIn : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference skillCheck1;
    [Header("UI")]
    [SerializeField] private RectTransform arrow;
    [SerializeField] private RectTransform greenZone;
    [SerializeField] private RectTransform orangeZone;
    [SerializeField] private GameObject allInPanel;
    [SerializeField] private Image allInDurationTimer;
    [SerializeField] private Image allInProgressBar;
    [Header("All In Settings")]
    [SerializeField] private float allInDuration = 6.0f;
    private float elapsedTime;
    private bool isRunning;
    [SerializeField] private float defaultSpeed = 10;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float incrementalSpeed = 13f;
    private float direction = 1f;
    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;
    [SerializeField] private float allInProgress = 0f;
    private float allInGoal = 100f;
    [SerializeField] private float greenZoneSuccessAmount = 25f;
    [SerializeField] private float orangeZoneSuccessAmount = 20f;
    private Action<bool> onComplete;

    void Start()
    {
        defaultSpeed = speed;

        RandomizeOrangeZone();
    }
    void OnEnable()
    {
        skillCheck1.action.performed += OnSkillCheckPressed;
        speed = defaultSpeed;
        allInProgress = 0f;
        //allInProgress = 0f;
        allInProgressBar.fillAmount = 0f;
    }

    void OnDisable()
    {
        skillCheck1.action.performed -= OnSkillCheckPressed;
    }
    public void StartAllIn(Action<bool> success)
    {
        if(allInPanel != null)
            allInPanel.SetActive(true);
        elapsedTime = allInDuration;
        isRunning = true;

        onComplete = success;//pass bool to QTEManager
    }
    void Update()
    {
        if (!isRunning)
            return;
        MoveArrow();

        elapsedTime -= Time.deltaTime;
        allInDurationTimer.fillAmount = elapsedTime;
        //TODO: [VFX] play timer blinking when time is low
        // if (thirdPersonController.sprinkle.WasPressedThisFrame())
        // {
        //     HandleHit();
        // }
        if (elapsedTime <= 0 && allInProgress < allInGoal)
        {
            if(allInPanel != null)
                allInPanel.SetActive(false);
            FinishAllIn(false);//player failed all in
        }
    }

    void MoveArrow()
    {
        arrow.anchoredPosition += Vector2.right * speed * direction * Time.deltaTime;

        if (arrow.anchoredPosition.x >= rightBound)
        {
            direction = -1f;
        }
        else if (arrow.anchoredPosition.x <= leftBound)
        {
            direction = 1f;
        }
    }
    
    private void OnSkillCheckPressed(InputAction.CallbackContext context)
    //"InputAction.CallbackContext context" is the way to tell Unity an input action is called?
    {
        HandleHit();
    }

    void HandleHit()
    {
        //SoundManager.Instance.PlayUISound(SoundManager.Instance.sprinkleAddictive);
        float arrowX = arrow.anchoredPosition.x;        
            
        if (IsInside(arrow, greenZone))
        {
            speed = speed * (100+incrementalSpeed) / 100; // Increase speed after each hit
            allInProgress += greenZoneSuccessAmount;
            RandomizeOrangeZone();
            ShowFeedback();
        }
        else if (IsInside(arrow, orangeZone))
        {
            speed = speed * (100+incrementalSpeed) / 100; // Increase speed after each hit
            allInProgress += orangeZoneSuccessAmount;
            RandomizeOrangeZone();
            ShowFeedback();
        }

        // Clamp stability
        allInProgress = Mathf.Clamp(allInProgress, 0f, 100f);
        allInProgressBar.fillAmount = allInProgress / 100f;
        
        

        if (allInProgress >= allInGoal)
        {
            FinishAllIn(true);
            //SoundManager.Instance.PlayUISound(SoundManager.Instance.cookingComplete);
        }
    }

    bool IsInside(RectTransform a, RectTransform b)
    {
        float aX = a.anchoredPosition.x;
        float bMin = b.anchoredPosition.x - b.rect.width / 2;
        float bMax = b.anchoredPosition.x + b.rect.width / 2;

        return aX >= bMin && aX <= bMax;
    }

    void RandomizeOrangeZone()
    {
        //greeen zone is child of orange zone
        float randomX = UnityEngine.Random.Range(leftBound + 50f, rightBound - 50f);
        orangeZone.anchoredPosition = new Vector2(randomX, orangeZone.anchoredPosition.y);
    }

    void FinishAllIn(bool success)
    {
        //reset values
        speed = defaultSpeed;
        allInProgress = 0f;
        allInProgressBar.fillAmount = 0f;

        if (!isRunning)
            return;
        
        if(allInPanel != null)
            allInPanel.SetActive(false);
        isRunning = false;

        onComplete?.Invoke(success);
        //pass bool to All In
        onComplete = null;

        UnityEngine.Debug.Log(success ? "All In Success!" : "All InFailed!");
    }

    void ShowFeedback()
    {
        // Hook animation / VFX here
        UnityEngine.Debug.Log("Slam chips on table");
    }
}