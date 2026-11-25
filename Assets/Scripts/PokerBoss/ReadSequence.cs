using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using Random = UnityEngine.Random;
public class ReadSequence : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference[] skillChecks;

    [Header("UI")]
    [SerializeField] private Image[] buttonVisuals;
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private GameObject readSequencePanel;
    [SerializeField] private Image readSequenceDurationTimer;

    
    [Header("Read Sequence Settings")]
    [SerializeField] private int minSequenceLength = 4;
    [SerializeField] private int maxSequenceLength = 4;
    [SerializeField] private float readSequenceDuration = 9.0f;
    private float elapsedTime;
    private bool isRunning;
    private bool startTimer = false;
    [Header("Display Settings")]
    [SerializeField] private float displayDuration = 0.5f;
    [SerializeField] private float displayInterval = 0.25f;

    private List<int> sequence = new List<int>();

    private Color[] originalColors;

    private int currentInputIndex;
    private bool isWaitingForInput;
    private Action<bool> onComplete;

    private void Awake()
    {
        originalColors = new Color[buttonVisuals.Length];

        for (int i = 0; i < buttonVisuals.Length; i++)
        {
            originalColors[i] = buttonVisuals[i].color;
        }
    }
    void OnEnable()
    {
        foreach (InputActionReference skillCheck in skillChecks)
        {
            skillCheck.action.performed += OnSkillCheckPressed;
        }
    }

    void OnDisable()
    {
        foreach (InputActionReference skillCheck in skillChecks)
        {
            skillCheck.action.performed -= OnSkillCheckPressed;
        }
        StopAllCoroutines();
    }
    public void StartReadSequence(float duration, Action<bool> success)
    {
        if (isRunning)
            return;
        
        if(readSequencePanel != null)
            readSequencePanel.SetActive(true);

        readSequenceDuration = duration;
        elapsedTime = readSequenceDuration;

        onComplete = success;//pass bool to QTEManager

        isRunning = true;
        isWaitingForInput = false;

        currentInputIndex = 0;

        GenerateSequence();

        StartCoroutine(PlaySequence());
    }
    void Update()
    {
        if (startTimer)
        {
            elapsedTime -= Time.deltaTime;
            readSequenceDurationTimer.fillAmount = elapsedTime / readSequenceDuration;

            if (elapsedTime <= 0)
            {
                if(readSequencePanel != null)
                    readSequencePanel.SetActive(false);
                startTimer = false;
                FinishReadSequence(false);//player failed all in
            }
        }
    }

    private void GenerateSequence()
    {
        sequence.Clear();

        int length = Random.Range(minSequenceLength, maxSequenceLength + 1);

        for (int i = 0; i < length; i++)
        {
            int randomButton = Random.Range(0, skillChecks.Length);

            sequence.Add(randomButton);
        }

        Debug.Log("Generated Sequence: " + string.Join(" -> ", sequence));
    }

    private IEnumerator PlaySequence()
    {
        // Observation phase
        foreach (int buttonIndex in sequence)
        {
            buttonVisuals[buttonIndex].color = highlightColor;

            yield return new WaitForSeconds(displayDuration);

            buttonVisuals[buttonIndex].color = originalColors[buttonIndex];

            yield return new WaitForSeconds(displayInterval);
        }

        // Player input phase
        isWaitingForInput = true;
        startTimer = true;

        Debug.Log("Repeat the sequence!");
    }
    private void OnSkillCheckPressed(InputAction.CallbackContext context)
    //"InputAction.CallbackContext context" is the way to tell Unity an input action is called?
    {
        if (isRunning && isWaitingForInput)
        {

            // Find which skill check was pressed
            for (int i = 0; i < skillChecks.Length; i++)
            {
                if (context.action != skillChecks[i].action)
                    continue;
                //continue: move onto next iteration
                //example player presses 2, current i = 0 -> move to 1, then move to 2

                CheckInput(i);
                break;
            }
        }
    }
    private void CheckInput(int pressedButton)
    {
        int expectedButton = sequence[currentInputIndex];

        if (pressedButton != expectedButton)
        {
            FinishReadSequence(false);
            return;
        }

        currentInputIndex++;

        Debug.Log("Correct! " + currentInputIndex + "/" + sequence.Count);

        // Player completed the sequence
        if (currentInputIndex >= sequence.Count)
        {
            FinishReadSequence(true);
        }
    }

    void FinishReadSequence(bool success)
    {
        if (!isRunning)
            return;
        
        if(readSequencePanel != null)
            readSequencePanel.SetActive(false);
        isRunning = false;
        startTimer = false;
        isWaitingForInput = false;


        onComplete?.Invoke(success);
        //pass bool to All In
        onComplete = null;

        UnityEngine.Debug.Log(success ? "Read Sequence Success!" : "Read Sequence InFailed!");
    }

    void ShowAnimation()
    {
        // Hook animation / VFX here
        UnityEngine.Debug.Log("boss doing animation");
    }
}