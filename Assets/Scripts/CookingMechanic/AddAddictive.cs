using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using StarterAssets;

public class AddAddictive : MonoBehaviour
{
    public ThirdPersonController thirdPersonController;
    
    public RectTransform arrow;
    public RectTransform greenZone;
    public RectTransform orangeZone;
    [SerializeField] private Image stabilityProgressBar;

    private float defaultSpeed;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float incrementalSpeed = 20f;
    private float direction = 1f;

    public CookingSessionData session = new CookingSessionData();

    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;

    void Start()
    {
        thirdPersonController = ThirdPersonController.Instance;
        defaultSpeed = speed;


        RandomizeOrangeZone();
    }
    void OnEnable()
    {
        //speed = defaultSpeed;
        session.stability = 100f;
        session.successfulHits = 0;
        stabilityProgressBar.fillAmount = session.stability / 100f;
    }

    void Update()
    {
        MoveArrow();

        if (thirdPersonController.sprinkle.WasPressedThisFrame())
        {
            HandleHit();
        }
    }

    void MoveArrow()
    {
        arrow.anchoredPosition += Vector2.right * speed * direction * Time.deltaTime;

        if (arrow.anchoredPosition.x >= rightBound)
        {
            direction = -1f;
        }
        else if (arrow.anchoredPosition.x <= leftBound)
        {
            direction = 1f;
        }
    }

    void HandleHit()
    {
        float arrowX = arrow.anchoredPosition.x;
        session.successfulHits++;

        speed = speed * (100+incrementalSpeed) / 100; // Increase speed after each hit
        
            
        if (IsInside(arrow, greenZone))
        {
            //RandomizeOrangeZone();

            
        }
        else if (IsInside(arrow, orangeZone))
        {
            session.stability -= 5f;
        }
        else
        {
            session.stability -= 25f;
        }

        // Clamp stability
        session.stability = Mathf.Clamp(session.stability, 0f, 100f);
        stabilityProgressBar.fillAmount = session.stability / 100f;
        ShowSprinkleFeedback();
        RandomizeOrangeZone();

        if (session.successfulHits >= 4)
        {
            FinishMinigame();
        }
    }

    bool IsInside(RectTransform a, RectTransform b)
    {
        float aX = a.anchoredPosition.x;
        float bMin = b.anchoredPosition.x - b.rect.width / 2;
        float bMax = b.anchoredPosition.x + b.rect.width / 2;

        return aX >= bMin && aX <= bMax;
    }

    void RandomizeOrangeZone()
    {
        float randomX = Random.Range(leftBound + 50f, rightBound - 50f);
        orangeZone.anchoredPosition = new Vector2(randomX, orangeZone.anchoredPosition.y);
    }

    void FinishMinigame()
    {


        CheeseData cheese = new CheeseData(session.stability);
        //reset values
        session.successfulHits = 0;
        speed = defaultSpeed;

        // Send to inventory or manager
        Debug.Log("Final Stability: " + cheese.finalStability);
        
        // TODO: InventorySystem.Add(cheese);
        CookingManager.Instance.FinishCooking();
    }

    void ShowSprinkleFeedback()
    {
        // Hook animation / VFX here
        Debug.Log("Sprinkling sugar...");
    }
}