using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SettingsScript : MonoBehaviour
{
    [Header("Input")]
    public ThirdPersonController thirdPersonController;

    void Start()
    {
        thirdPersonController = ThirdPersonController.Instance;
        thirdPersonController.OnPauseGame += OnPauseGamePerformed;
    }
    void Update()
    {
        
    }

}
