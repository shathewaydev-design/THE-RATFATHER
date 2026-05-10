using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;
    private bool cursorUnlocked;
    public ThirdPersonController thirdPersonController;

    private void Awake()
    {
        Instance = this;
        thirdPersonController = ThirdPersonController.Instance;
    }
    private void Start()
    {
        thirdPersonController.OnToggleCursor += ToggleCursor;
    }

    public void ToggleCursor()//for debugging purposes
    {
        cursorUnlocked = !cursorUnlocked;

        Cursor.visible = cursorUnlocked;

        Cursor.lockState = cursorUnlocked
            ? CursorLockMode.None
            : CursorLockMode.Locked;
    }
    public void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}