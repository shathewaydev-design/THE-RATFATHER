using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public enum TutorialStage
    {
        None,
        TalkToNPC,
        CompleteQuest,
        ReturnToNPC,
        Finished
    }

    public TutorialStage currentStage;

    void Awake()
    {
        // Singleton pattern (simple version); avoid duplicates
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentStage = TutorialStage.TalkToNPC;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
