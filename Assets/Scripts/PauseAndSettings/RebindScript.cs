using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
// using StarterAssets;
using UnityEngine.InputSystem;

public class RebindScript : MonoBehaviour
{
    [SerializeField] private InputActionAsset InputActions;
    [SerializeField] private InputActionReference m_action;
    [SerializeField] private int m_bindingIndex = 0;//this is when i.e. Interact E and Gamepad: A,...

    [SerializeField] private KeybindText keybindText;//used to update text after rebind is completed

    private InputActionRebindingExtensions.RebindingOperation m_rebindingOperation;
    //private InputAction m_interact;//default is E
    [SerializeField] private Button m_rebindButton;
    [SerializeField] private TextMeshProUGUI m_rebindLabel;

    private InputActionMap m_playerMap;
    private InputActionMap m_cookingMap;
    private InputActionMap m_globalMap;

    
    
    private void Awake()
    {
        //m_interact = InputActions.FindAction("Interact");
        m_playerMap = InputActions.FindActionMap("Player");
        m_cookingMap = InputActions.FindActionMap("Cooking");
        m_globalMap = InputActions.FindActionMap("Global");
    }
    private void OnEnable()
    {
        m_rebindButton.Select();
        m_rebindButton.onClick.AddListener(Rebind);
        m_rebindLabel.text = "";
    }
    private void OnDisable()
    {
        m_rebindButton.onClick.RemoveListener(Rebind);
    }

    public void Rebind()
    {
       m_playerMap.Disable();
       m_cookingMap.Disable();
       m_globalMap.Disable();

        m_rebindLabel.text = "Choose a new button...";
        m_rebindButton.interactable = false;
        // m_interact.Disable();
        //m_rebindingOperation = m_interact.PerformInteractiveRebinding().WithCancelingThrough("<Keyboard>/escape").OnComplete(operation => RebindCompleted()).OnCancel(operation => RebindCanceled());
        m_rebindingOperation = m_action.action.PerformInteractiveRebinding(m_bindingIndex).WithCancelingThrough("<Keyboard>/escape").OnComplete(operation => RebindCompleted()).OnCancel(operation => RebindCanceled());
        //if player needs to cancel, 
        //there's an option to cancel the rebind operation

        m_rebindingOperation.Start();
    }
    void RebindCompleted()
    {
        //m_rebindingOperation.Dispose();
        
        //string newBinding = m_interact.bindings[0].effectivePath;
        //string newBinding = m_action.action.bindings[m_bindingIndex].effectivePath;
        string newBinding = m_action.action.GetBindingDisplayString(m_bindingIndex);
        //display readable text
        m_rebindLabel.text = "Rebind completed!";

        SaveRebinds();

        keybindText.RefreshKeyText();

        m_playerMap.Enable();
        m_cookingMap.Enable();
        m_globalMap.Enable();
        // m_interact.Enable();
        
        
        m_rebindButton.interactable = true;
        
        StartCoroutine(CleanupRebindingOperation());
        //to make sure Unity has time to process the rebind operation 
        //before disposing of it
    }

    private void RebindCanceled()
    {
        m_rebindLabel.text = "Rebind canceled.";

        m_playerMap.Enable();
        m_cookingMap.Enable();
        m_globalMap.Enable();
        m_rebindButton.interactable = true;

        StartCoroutine(CleanupRebindingOperation());
        //to make sure Unity has time to process the rebind operation 
        //before disposing of it
    }
    private void SaveRebinds()
    {
        string rebinds = InputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.Save();
    }
    private IEnumerator CleanupRebindingOperation()
    {
        yield return null;

        if (m_rebindingOperation != null)
        {
            m_rebindingOperation.Dispose();
            m_rebindingOperation = null;
        }
    }
}
