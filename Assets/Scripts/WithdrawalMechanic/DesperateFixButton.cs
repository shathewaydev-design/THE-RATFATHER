using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class DesperateFixButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [Header("References")]

    [SerializeField] private GameObject[] buttonTexts;

    [SerializeField] private Image progressFill;
    [SerializeField] private Image desperateFixDurationFill;
    [SerializeField] private GameObject desperateFixDurationFillUI;

    [Header("Settings")]
    [SerializeField] private float holdDuration = 2.5f;
    [SerializeField] private float desperateFixDuration = 20.0f;//20s

    [Header("Input")]
    [SerializeField] private InputActionReference desperateFixAction;//interact button

    private bool isMouseOver;
    private bool isMouseHeld;
    private bool isActivating;

    private float holdTimer;
    private float desperateFixTimer;

    private void OnEnable()
    {
        if (desperateFixAction != null)
        {
            desperateFixAction.action.Enable();
        }
        desperateFixDurationFillUI.SetActive(false);
    }

    private void OnDisable()
    {
        if (desperateFixAction != null)
        {
            desperateFixAction.action.Disable();
        }
    }

    private void Start()
    {
        ResetProgress();
        ShowDesperateFixText();
        desperateFixTimer = desperateFixDuration;
    }

    private void Update()
    {
        bool keyboardHolding = false;

        if (desperateFixAction != null)
        {
            keyboardHolding = desperateFixAction.action.IsPressed();
        }

        bool mouseHolding = isMouseOver && isMouseHeld;

        bool isHolding = keyboardHolding || mouseHolding;

        if (isHolding)
        {
            StartHolding();
        }
        else
        {
            StopHolding();
        }
        DrainingDuration();
    }

    // =========================================================
    // HOLD LOGIC
    // =========================================================

    private void StartHolding()
    {
        if (isActivating)
            return;

        holdTimer += Time.deltaTime;

        float progress = holdTimer / holdDuration;

        progress = Mathf.Clamp01(progress);

        UpdateProgress(progress);

        if (holdTimer >= holdDuration)
        {
            ActivateDesperateFix();
        }
    }

    private void StopHolding()
    {
        if (holdTimer <= 0f)
            return;

        holdTimer = 0f;

        UpdateProgress(0f);
    }
    private void DrainingDuration()
    {
        if (WithdrawalMechanic.Instance.isInDesperateFix)
        {
            
            desperateFixTimer -= Time.deltaTime;
            float duration = desperateFixTimer / desperateFixDuration;

            UpdateDesperateFixProgress(duration);
            if (desperateFixTimer <= 0f)
            {
                WithdrawalMechanic.Instance.isInDesperateFix = false;
                desperateFixDurationFillUI.SetActive(false);
                desperateFixTimer = desperateFixDuration;
                Debug.Log("Desperate Fix mode has ended.");
                // Additional logic for ending Desperate Fix mode can be added here
            }
            if (WithdrawalMechanic.Instance.currentStage == WithdrawalMechanic.WithdrawalStage.Critical || WithdrawalMechanic.Instance.currentStage == WithdrawalMechanic.WithdrawalStage.Emergency)
            {
                ShowDesperateFixText();
            }
        }
    }
    private void ActivateDesperateFix()
    {
        if (isActivating)
            return;

        isActivating = true;
        HideDesperateFixText();
        desperateFixDurationFillUI.SetActive(true);
        Debug.Log("Player has entered Desperate Fix mode.");

        // Tell the withdrawal system to enter
        // emergency satiation mode.

        WithdrawalMechanic.Instance.isInDesperateFix = true;
        StartCoroutine(WithdrawalMechanic.Instance.DesperateFixTimer());
        
        //run a dureation for desperate fix mode
        ResetProgress();

        isActivating = false;
        //this.gameObject.SetActive(false);
    }

    // =========================================================
    // MOUSE
    // =========================================================

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        isMouseHeld = false;

        StopHolding();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isMouseHeld = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isMouseHeld = false;
        }
    }

    // =========================================================
    // UI
    // =========================================================

    private void UpdateProgress(float progress)
    {
        if (progressFill != null)
        {
            progressFill.fillAmount = progress;
        }
    }
    private void UpdateDesperateFixProgress(float duration)
    {
        if (desperateFixDurationFill != null)
        {
            desperateFixDurationFill.fillAmount = duration;
        }
    }
    private void HideDesperateFixText()
    {
        buttonTexts[0].SetActive(false);
        buttonTexts[1].SetActive(false);
    }
    public void ShowDesperateFixText()
    {
        buttonTexts[0].SetActive(true);
        buttonTexts[1].SetActive(true);
    }

    private void ResetProgress()
    {
        holdTimer = 0f;
        UpdateProgress(0f);
    }

}