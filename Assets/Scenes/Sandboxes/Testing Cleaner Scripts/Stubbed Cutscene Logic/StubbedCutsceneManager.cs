using NUnit.Framework;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class StubbedCutsceneManager : MonoBehaviour
{
    public static StubbedCutsceneManager Instance;
    public ThirdPersonController thirdPersonController;

    [SerializeField] private GameObject stubbedCutsceneScreen;

    private int currPanelIndex = 0;
    private StubbedCutsceneData currStubbedCutscene;
    [SerializeField] private Image currPanel;

    [SerializeField] private List<StubbedCutsceneData> allStubbedCutscenes = new List<StubbedCutsceneData>();
    [SerializeField] private InputActionReference click;

    [SerializeField] private CanvasGroup fade;
    [SerializeField] private float fadeDuration = 0.25f;

    public bool inCutscene;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        //OpenCutscene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (inCutscene && click.action.WasPressedThisFrame()) // && action is pressed, cycle through cutscene.
        {
            CycleThroughCutscene();
        }
        
    }
    // cycle through each custcene panel on click, and display it
    public void OpenCutscene(int cutsceneNum)
    {
        stubbedCutsceneScreen.SetActive(true);
        currStubbedCutscene = allStubbedCutscenes[cutsceneNum];
        currPanelIndex = 0;
        // trigger fade in
        FadeIn();
        inCutscene = true;
        currPanel.sprite = currStubbedCutscene.panels[0];
        currPanelIndex++;
    }

    public void CloseCutscene()
    {
        StartCoroutine(CloseCutsceneCoroutine());
    }

    private IEnumerator CloseCutsceneCoroutine()
    {
        inCutscene = false;

        yield return StartCoroutine(FadePanel(0f));

        stubbedCutsceneScreen.SetActive(false);
    }

    public void CycleThroughCutscene()
    {
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (currPanelIndex >= currStubbedCutscene.panels.Count)
        {
            currPanelIndex = 0;
            CloseCutscene();
            // teleport player if needed
            // or open scene if needed :)
            // if nothing, just closes cutscene
        }
        else
        {
            currPanel.sprite = currStubbedCutscene.panels[currPanelIndex];
            currPanelIndex++;
        }
    }

    // HANDLE TELEPORTATION IF NEEDS IT
    public void TeleportPlayer()
    {
        // call player teleport
    }

    public void OpenScene()
    {
        // start new scene
    }

    // handle fade in and out
    public void FadeIn()
    {
        StartCoroutine(FadePanel(1f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadePanel(0f));
    }

    

    private IEnumerator FadePanel(float targetAlpha)
    {
        if (fade != null)
        {
            Debug.Log("Not null at this point...");

        }
        float startingAlpha = fade.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            fade.alpha = Mathf.Lerp(
                startingAlpha,
                targetAlpha,
                elapsedTime / fadeDuration
            );

            yield return null;
        }

        if (fade != null)
        {
            fade.alpha = targetAlpha;
        }
    }
}
