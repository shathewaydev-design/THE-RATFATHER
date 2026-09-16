using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
// using StarterAssets;
using UnityEngine.InputSystem;

public class RebindScript : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputActionRebindingExtensions.RebindingOperation m_rebindingOperation;
    private InputAction m_interact;//default is E
    [SerializeField] private Button m_rebindButton;
    [SerializeField] private TextMeshProUGUI m_rebindLabel;

    private InputActionMap m_player;

    
    
    private void Awake()
    {
        m_interact = InputActions.FindAction("Interact");
        m_player = InputActions.FindActionMap("Player");
       
    }
    private void OnEnable()
    {
        m_rebindButton.Select();
        m_rebindButton.onClick.AddListener(Rebind);
    }
    private void OnDisable()
    {
        m_rebindButton.onClick.RemoveListener(Rebind);
    }

    public void Rebind()
    {
       m_player.Disable();
        m_rebindLabel.text = "Choose a new button...";
        m_rebindButton.interactable = false;
        // m_interact.Disable();
        m_rebindingOperation = m_interact.PerformInteractiveRebinding().WithCancelingThrough("<Keyboard>/escape").OnComplete(operation => RebindCompleted()).OnCancel(operation => RebindCanceled());
        //if player needs to cancel, 
        //there's an option to cancel the rebind operation

        m_rebindingOperation.Start();
    }
    void RebindCompleted()
    {
        //m_rebindingOperation.Dispose();
        
        string newBinding = m_interact.bindings[0].effectivePath;
        m_rebindLabel.text = $"Rebind completed: {newBinding}";

        SaveRebinds();

        m_player.Enable();
        // m_interact.Enable();
        
        
        m_rebindButton.interactable = true;
        
        //m_rebindingOperation.Dispose();
        //m_rebindingOperation = null;
        StartCoroutine(CleanupRebindingOperation());
        //to make sure Unity has time to process the rebind operation 
        //before disposing of it
    }

    private void RebindCanceled()
    {
        m_rebindLabel.text = "Rebind canceled.";

        m_player.Enable();
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
