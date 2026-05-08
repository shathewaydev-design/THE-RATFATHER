using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationUIController : MonoBehaviour
{
    public static NotificationUIController Instance;

    [SerializeField]
    private RectTransform panel;

    [SerializeField]
    private TextMeshProUGUI messageText;

    private Vector2 hiddenPos;
    private Vector2 shownPos;

    private void Awake()
    {
        Instance = this;

        hiddenPos = new Vector2(600, 0);
        shownPos = new Vector2(0, 0);

        panel.anchoredPosition = hiddenPos;
    }

    public void ShowNotification(string message)
    {
        StopAllCoroutines();

        StartCoroutine(
            AnimateNotification(message)
        );
    }

    private IEnumerator AnimateNotification(
        string message)
    {
        messageText.text = message;

        panel.anchoredPosition = shownPos;

        yield return new WaitForSeconds(2f);

        panel.anchoredPosition = hiddenPos;
    }
}