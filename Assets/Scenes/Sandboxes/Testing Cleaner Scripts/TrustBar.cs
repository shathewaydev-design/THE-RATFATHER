using UnityEngine;

public class TrustBar : MonoBehaviour
{
    [SerializeField] private RectTransform indicator;


    private void OnEnable()
    {
        DialogueManager_New.OnTrustChange += UpdateTrustBar;
        DialogueManager_New.OnTrustBarActivated += UpdateTrustBar;
    }

    private void OnDisable()
    {
        DialogueManager_New.OnTrustChange -= UpdateTrustBar;
        DialogueManager_New.OnTrustBarActivated -= UpdateTrustBar;
    }
    public void UpdateTrustBar(float trust)
    {
        trust = Mathf.Clamp(trust, 0f, 100f);

        float xPosition = trust * 5f;

        Vector2 position = indicator.anchoredPosition;
        position.x = xPosition;

        indicator.anchoredPosition = position;
    }

}
