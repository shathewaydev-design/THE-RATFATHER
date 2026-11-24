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
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private float UIStartScale = 0.1f;
    [SerializeField] private float UIEndScale = 1.3f;

    [SerializeField, Range(0f, 1f)]
    private float successWindowStart = 0.7f;

    [SerializeField, Range(0f, 1f)]
    private float successWindowEnd = 0.85f;

    private float elapsedTime;
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

    public void StartQuickDraw(Action<bool> success)
    {
        if(quickDrawPanel != null)
            quickDrawPanel.SetActive(true);
        elapsedTime = 0f;
        isRunning = true;

        onComplete = success;//pass bool to QTEManager

        whiteCircle.localScale = Vector3.one * UIStartScale;
    }

    private void Update()
    {
        if (!isRunning)
            return;

        elapsedTime += Time.deltaTime;

        float progress = Mathf.Clamp01(elapsedTime / duration);

        float currentScale = Mathf.Lerp(UIStartScale, UIEndScale, progress);

        whiteCircle.localScale = Vector3.one * currentScale;

        // Time ran out
        if (elapsedTime >= duration)
        {
            if(quickDrawPanel != null)
                quickDrawPanel.SetActive(false);
            FinishQuickDraw(false);
        }
    }

    private void OnSkillCheckPressed(InputAction.CallbackContext context)
    {
        if (!isRunning)
            return;

        float progress = elapsedTime / duration;

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
