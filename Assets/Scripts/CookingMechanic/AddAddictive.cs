using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using StarterAssets;

public class AddAddictive : MonoBehaviour
{
    public GameObject cookingPot;
    public ThirdPersonController thirdPersonController;
    
    public RectTransform arrow;
    public RectTransform greenZone;
    public RectTransform orangeZone;

    public float speed = 300f;
    private float direction = 1f;

    public CookingSessionData session = new CookingSessionData();

    private float leftBound;
    private float rightBound;

    void Start()
    {
        thirdPersonController = ThirdPersonController.Instance;
        cookingPot.SetActive(false);

        leftBound = -400f;
        rightBound = 400f;

        RandomizeGreenZone();
    }

    void Update()
    {
        MoveArrow();

        if (thirdPersonController.sprinkle.IsPressed())
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

        if (IsInside(arrow, greenZone))
        {
            session.successfulHits++;
            Debug.Log("Perfect hit!");

            RandomizeGreenZone();

            if (session.successfulHits >= 5)
            {
                FinishMinigame();
            }
        }
        else if (IsInside(arrow, orangeZone))
        {
            session.stability -= 5f;
            Debug.Log("Minor mistake (-5%)");
        }
        else
        {
            session.stability -= 25f;
            Debug.Log("Major mistake (-25%)");
        }

        // Clamp stability
        session.stability = Mathf.Clamp(session.stability, 0f, 100f);

        ShowSprinkleFeedback();
    }

    bool IsInside(RectTransform a, RectTransform b)
    {
        float aX = a.anchoredPosition.x;
        float bMin = b.anchoredPosition.x - b.rect.width / 2;
        float bMax = b.anchoredPosition.x + b.rect.width / 2;

        return aX >= bMin && aX <= bMax;
    }

    void RandomizeGreenZone()
    {
        float randomX = Random.Range(leftBound + 50f, rightBound - 50f);
        greenZone.anchoredPosition = new Vector2(randomX, greenZone.anchoredPosition.y);
    }

    void FinishMinigame()
    {
        Debug.Log("Minigame complete!");

        CheeseData cheese = new CheeseData(session.stability);

        // Send to inventory or manager
        Debug.Log("Final Stability: " + cheese.finalStability);

        // TODO: InventorySystem.Add(cheese);
    }

    void ShowSprinkleFeedback()
    {
        // Hook animation / VFX here
        Debug.Log("Sprinkling sugar...");
    }
}