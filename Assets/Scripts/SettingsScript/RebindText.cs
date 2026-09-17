using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class KeybindText : MonoBehaviour
{
    [SerializeField] private InputActionReference actionReference;
    [SerializeField] private TextMeshProUGUI keyText;

    private void OnEnable()
    {
        RefreshKeyText();
    }

    public void RefreshKeyText()
    {
        if (actionReference == null || keyText == null)
            return;

        keyText.text = actionReference.action.GetBindingDisplayString();
    }
}