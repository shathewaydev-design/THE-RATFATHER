using NUnit.Framework;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    bool justClicked = false;

    // Update is called once per frame
    void Update()
    {
        if (inCutscene && click.action.WasPressedThisFrame()) // && action is pressed, cycle through cutscene.
        {
            
           // if (!justClicked)
            //{
                //Debug.Log("please for the love of god CLIIIIIIICK!!!!");
                Debug.Log("index: " + currPanelIndex);

                CycleThroughCutscene();

                justClicked = true;
            //}
            //else
            //{
             //   justClicked = false;
             //   return;
            //}
            
        }
        
    }

    public void OpenCutscene(int cutsceneNum)
    {
        inCutscene = true;

        currStubbedCutscene = allStubbedCutscenes[cutsceneNum];
        currPanelIndex = 0;

        // Show the cutscene immediately; no fade.
        stubbedCutsceneScreen.SetActive(true);

        thirdPersonController.GetComponent<PlayerInput>()
            .SwitchCurrentActionMap("Mouse");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        currPanel.sprite = currStubbedCutscene.panels[0];
        //currPanelIndex++;
    }

    public void CloseCutscene()
    {
        inCutscene = false;

        // Hide the cutscene immediately; no fade.
        stubbedCutsceneScreen.SetActive(false);

        if (currStubbedCutscene.OpensNewScene())
        {
            OpenScene();
        }
        else if (currStubbedCutscene.TeleportsPlayer())
        {
            TeleportPlayer();
        }


        thirdPersonController.GetComponent<PlayerInput>()
            .SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;



    }














    // cycle through each custcene panel on click, and display it
    //public void OpenCutscene(int cutsceneNum)
    //{
    //    //stubbedCutsceneScreen.SetActive(true);

    //    inCutscene = true;


    //    currStubbedCutscene = allStubbedCutscenes[cutsceneNum];
    //    currPanelIndex = 0;

    //    StartCoroutine(OpenCutsceneCoroutine());

    //    //thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");
    //    //Cursor.visible = true;
    //    //Cursor.lockState = CursorLockMode.None;

    //    thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    //    Cursor.visible = false;
    //    Cursor.lockState = CursorLockMode.Locked;


    //    // trigger fade in
    //    //FadeIn();
    //    //inCutscene = true;
    //    currPanel.sprite = currStubbedCutscene.panels[0];
    //    currPanelIndex++;
    //}

    //private IEnumerator OpenCutsceneCoroutine()
    //{
    //    //inCutscene = true;

    //    yield return StartCoroutine(FadePanel(1f));

    //    stubbedCutsceneScreen.SetActive(true);
    //}

    //public void CloseCutscene()
    //{
    //    inCutscene = false;


    //    StartCoroutine(CloseCutsceneCoroutine());

    //    //thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    //    //Cursor.visible = false;
    //    //Cursor.lockState = CursorLockMode.Locked;

    //}

    //private IEnumerator CloseCutsceneCoroutine()
    //{
    //    //inCutscene = false;

    //    yield return StartCoroutine(FadePanel(0f));

    //    stubbedCutsceneScreen.SetActive(false);
    //}

    public void CycleThroughCutscene()
    {
        //Debug.Log("CycleThroughCutscene called!");
        Debug.Log("CycleThroughCutscene called!");

        if (currPanelIndex > currStubbedCutscene.panels.Count - 1)
        {

            //Debug.Log("CycleThroughCutscene not working!");

            //currPanelIndex = 0;
            CloseCutscene();
            // teleport player if needed
            // or open scene if needed :)
            // if nothing, just closes cutscene
        }
        else
        {
            //Debug.Log("index: " + currPanelIndex);

            currPanelIndex++;

            currPanel.sprite = currStubbedCutscene.panels[currPanelIndex];
            //currPanelIndex++;
        }
    }

    // HANDLE TELEPORTATION IF NEEDS IT
    public void TeleportPlayer()
    {
        // call player teleport
        // will invoke 
    }

    public void OpenScene()
    {
        // start new scene
        SceneManager.LoadScene(currStubbedCutscene.GetSceneName());
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
