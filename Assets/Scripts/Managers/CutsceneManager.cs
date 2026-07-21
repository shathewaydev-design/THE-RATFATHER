using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;


public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;


    // game manager checks and calls this script << GOAL (for simplicity)

    [SerializeField] private GameObject cutscenePanel;
    [SerializeField] private RawImage currComic;
    [SerializeField] private TMP_Text currDialogue;
    [SerializeField] private TMP_Text currSpeaker;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    //[SerializeField] private AudioClip currBKGMusic; // attatched to entire cutscene object
    //[SerializeField] private AudioClip currBKGNoise; // tied to each individual panel

    [SerializeField] private CanvasGroup cutsceneCanvasGroup;
    //[SerializeField] private RawImage fadeOverlay;


    [SerializeField] private float fadeDuration = 1.5f;

    public ComicCutscene currCutscene;
    private int cutsceneIndex = 0;


    // NOTES FOR THIS OBJECT TYPE

    //public GameObject CutsceneData currCutscene;

    // cutscenes as types? Start with list of images, properly labeled << dialogue must also be
    // properly linked to each panel, and may have more than one dialogue line per image

    // AS OF NOW: currCutscene is set outside of this manager, and StartCutscene is called
    // outside of this manager

    public bool isCutsceneActive; // game manager sets this true? maybe purely for this class

    public ThirdPersonController thirdPersonController; // movement reference


    private void Awake()
    {

        // Singleton pattern (simple version); avoid duplicates
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (isCutsceneActive)
        //{
        //    if () // if mouse button clicked
        //    {
        //        AdvanceCutscene();


        //    }
        //}


        if (isCutsceneActive && thirdPersonController.mouseClick.WasPressedThisFrame())
        {
            AdvanceCutscene();
        }

    }


    // 1. pull up entire cutscene panel
    // 2. grab first comic panel of proper cutscene and display it
    // 3. grab first dialogue line of custene and display it
    // 4. allow for a click to continue the cutscene
    public void StartCutscene()
    {
        //StartCoroutine(FadeIn()); // testing fade in (works, unsure of how to implement black screen)


        cutscenePanel.SetActive(true); // pull up cutscene
        isCutsceneActive = true; // cutscene is now active

        // CURSOR NEEDS TO BE LOCKED
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Mouse");

        // BKG MUSIC IF THERE
        if (currCutscene.bkgMusic != null)
            musicSource.clip = currCutscene.bkgMusic;
            musicSource.Play();

        // FIND FIRST PANEL
        currComic.texture = currCutscene.panels[0].image;
        currDialogue.text = currCutscene.panels[0].dialogue;
        currSpeaker.text = currCutscene.panels[0].speaker;

        if (currCutscene.panels[0].soundEffect != null)
            sfxSource.PlayOneShot(currCutscene.panels[0].soundEffect); // sound effect if there

        cutsceneIndex++;

    }

    // if there is an existing comic panel after, when clicking, 
    // it displays this next panel
    // if not, calls for the cutscene to end
    private void AdvanceCutscene()
    {
        if (cutsceneIndex < currCutscene.panels.Count)
        {
            currComic.texture = currCutscene.panels[cutsceneIndex].image;
            currDialogue.text = currCutscene.panels[cutsceneIndex].dialogue;
            currSpeaker.text = currCutscene.panels[cutsceneIndex].speaker;

            if (currCutscene.panels[cutsceneIndex].soundEffect != null)
                sfxSource.PlayOneShot(currCutscene.panels[cutsceneIndex].soundEffect); // sound effect if there

            cutsceneIndex++;

        }
        else
        {
            StartCoroutine(EndCutscene());
        }

    }

    // when cutscene has reached its end, removes cutscene panel 
    // and gives player back character controls -- PERCHANCE TELEPORTS 
    private IEnumerator EndCutscene()
    {
        // fade out of cutscene
        yield return FadeOut();
        // cutscenePanel is no longer active
        isCutsceneActive = false;

        // end bkgmusic
        musicSource.Stop();

        // remove cutscene panel
        cutscenePanel.SetActive(false);

        // remove current cutscene?
        currCutscene = null;

        // reset index
        cutsceneIndex = 0;

        // give player controls back
        thirdPersonController.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");

        

    }

    private IEnumerator FadeIn()
    {
        cutsceneCanvasGroup.alpha = 0f;
        cutsceneCanvasGroup.blocksRaycasts = true;
        cutsceneCanvasGroup.interactable = false;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            cutsceneCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);

            yield return null;
        }

        cutsceneCanvasGroup.alpha = 1f;
        cutsceneCanvasGroup.interactable = true;

    }

    private IEnumerator FadeOut()
    {

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            cutsceneCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

            yield return null;
        }

        cutsceneCanvasGroup.alpha = 0f;
        cutsceneCanvasGroup.blocksRaycasts = false;
    }




}
