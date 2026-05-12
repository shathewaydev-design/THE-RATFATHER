using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour,
    IPointerEnterHandler,
    IPointerClickHandler
{
    private static float lastHoverTime;
    public float hoverCooldown = 0.05f;//avoid hovering over buttons fast
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Time.unscaledTime - lastHoverTime > hoverCooldown)
        {//Time.unscaledTime so when Time.timeScale = 0, audio still plays
            SoundManager.Instance.PlayUISound(SoundManager.Instance.uiHover);
            lastHoverTime = Time.unscaledTime;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlayUISound(SoundManager.Instance.uiClick);
    }
}