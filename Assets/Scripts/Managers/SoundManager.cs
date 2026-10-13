using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource AmbientSource3D;
    public AudioSource uiSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip uiHover;
    public AudioClip uiClick;
    public AudioClip inventoryTAB;//page turn
    public AudioClip notificationSound;
    public AudioClip toggleInventory;
    [Header("Cooking")]
    public AudioClip fireCrackling;//fire ambience near pot
    public AudioClip dropIngredients;//start cooking, sounds like ingredients being dropped into a pot
    public AudioClip pourMilk;//water slosh
    public AudioClip addFlavor;//water drop
    public AudioClip bellowSound;//air blow or spray
    public AudioClip sprinkleAddictive;//sprinkle salt
    public AudioClip cookingComplete;//
    [Header("Levels")]
    [SerializeField] private bool slumpsLevel = false;
    [SerializeField] private bool NewRatYorkLevel = false;
    public AudioClip slumpsMusic;
    public AudioClip NewRatYorkMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (slumpsLevel)
        {
            PlayMusic(slumpsMusic);
        }
        else if (NewRatYorkLevel)
        {
            PlayMusic(NewRatYorkMusic);
        }
    }

    public void PlayUISound(AudioClip clip)
    {
        uiSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
}