using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class PokerBossUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PokerBossState pokerBossState;
    [SerializeField] private PokerBossManager pokerBossManager;
    [Header("Panels")]
    [SerializeField] private GameObject confidencePanel;
    [SerializeField] private GameObject dealPanel;
    [SerializeField] private GameObject tellPanel;
    [SerializeField] private GameObject reactionPanel;
    [SerializeField] private GameObject showdownPanel;
    [Header("UI")]
    [SerializeField] private Image bossConfidenceBar;
    [SerializeField] private Image playerConfidenceBar;

    [SerializeField] private TextMeshProUGUI doseCountText;
    [SerializeField] private TextMeshProUGUI phaseText;
    [SerializeField] private TextMeshProUGUI bossNameText;
    private void OnEnable()
    {
        if (pokerBossState != null)
        {
            pokerBossState.OnConfidenceChanged += UpdateConfidenceUI;
            pokerBossState.OnDoseCountChanged += UpdateDoseUI;
            pokerBossState.OnBossChanged += UpdateBossNameUI;
        }

        if (pokerBossManager != null)
        {
            pokerBossManager.OnPhaseChanged += UpdatePhaseUI;
        }
    }

    private void OnDisable()
    {
        if (pokerBossState != null)
        {
            pokerBossState.OnConfidenceChanged -= UpdateConfidenceUI;
            pokerBossState.OnDoseCountChanged -= UpdateDoseUI;
            pokerBossState.OnBossChanged -= UpdateBossNameUI;
        }

        if (pokerBossManager != null)
        {
            pokerBossManager.OnPhaseChanged -= UpdatePhaseUI;
        }
    }

    private void UpdateConfidenceUI(float bossConfidence, float playerConfidence)
    {
        if (bossConfidenceBar != null)
            bossConfidenceBar.fillAmount = bossConfidence / 100f;

        if (playerConfidenceBar != null)
            playerConfidenceBar.fillAmount = playerConfidence / 100f;

    }

    private void UpdateDoseUI(int doses)
    {
        if (doseCountText != null)
            doseCountText.text = "Doses: " + doses;
    }

    private void UpdatePhaseUI(BossPhase phase)
    {
        dealPanel.SetActive(phase == BossPhase.Deal);
        // tellPanel.SetActive(phase == BossPhase.Tell);
        // reactionPanel.SetActive(phase == BossPhase.Reaction);
        // showdownPanel.SetActive(phase == BossPhase.Showdown);

        phaseText.text = phase.ToString();
    }
    private void UpdateBossNameUI(string bossName)
    {
        if (bossNameText != null)
            bossNameText.text = bossName;
    }
}