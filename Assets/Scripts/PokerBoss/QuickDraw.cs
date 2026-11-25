using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using StarterAssets;

public class QuickDraw : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference skillCheck1;

    [Header("UI")]
    [SerializeField] private RectTransform whiteCircle;
    [SerializeField] private GameObject quickDrawPanel;

    [Header("Timing")]
    [SerializeField] private float quickDrawDuration = 1.5f;
    [SerializeField] private float UIStartScale = 0.1f;
    [SerializeField] private float UIEndScale = 1.3f;
    [SerializeField] private float scaleSpeed = 1.3f;

    [SerializeField, Range(0f, 2f)]
    private float successWindowStart = 1.2f;

    [SerializeField, Range(0f, 2f)]
    private float successWindowEnd = 1.35f;

    [SerializeField] private float elapsedTime;
    private bool isRunning;

    private Action<bool> onComplete;

    private void OnEnable()
    {
        skillCheck1.action.performed += OnSkillCheckPressed;
    }

    private void OnDisable()
    {
        skillCheck1.action.performed -= OnSkillCheckPressed;
    }

    public void StartQuickDraw(float duration, Action<bool> success)
    {
        if(quickDrawPanel != null)
            quickDrawPanel.SetActive(true);
        quickDrawDuration = duration;
        elapsedTime = 0f;
        isRunning = true;

        onComplete = success;//pass bool to QTEManager

        whiteCircle.localScale = Vector3.one * scaleSpeed * UIStartScale;
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / quickDrawDuration);

            float currentScale = Mathf.Lerp(UIStartScale, UIEndScale, progress);

            whiteCircle.localScale = Vector3.one * currentScale;

            // Time ran out
            if (elapsedTime >= quickDrawDuration)
            {
                if(quickDrawPanel != null)
                    quickDrawPanel.SetActive(false);
                FinishQuickDraw(false);
            }
        }
    }

    private void OnSkillCheckPressed(InputAction.CallbackContext context)
    {
        if (!isRunning)
            return;

        float progress = elapsedTime / quickDrawDuration;

        bool success = progress >= successWindowStart && progress <= successWindowEnd;

        FinishQuickDraw(success);
    }

    private void FinishQuickDraw(bool success)
    {
        if (!isRunning)
            return;
        
        if(quickDrawPanel != null)
            quickDrawPanel.SetActive(false);
        isRunning = false;

        onComplete?.Invoke(success);
        //pass bool to StartQuickDraw
        onComplete = null;

        Debug.Log(success ? "Quick Draw Success!" : "Quick Draw Failed!");
    }
}
