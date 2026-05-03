using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using StarterAssets;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryBigPanel;
    public ThirdPersonController thirdPersonController;
    private bool isOpen = false;

    void Start()
    {
        thirdPersonController = ThirdPersonController.Instance;
        inventoryBigPanel.SetActive(isOpen);
        thirdPersonController.OnOpenInventory += ToggleInventory;
    }
    void Update()
    {
        
  
    }

    private void ToggleInventory()
    {
        Debug.Log("Toggle Inventory");
        isOpen = !isOpen;

        inventoryBigPanel.SetActive(isOpen);

        Cursor.visible = isOpen;

        Cursor.lockState = isOpen
            ? CursorLockMode.None//use this if isOpen is trur
            : CursorLockMode.Locked;//use this if isOpen is false
    }
    private void OnDestroy()
    {
        if (thirdPersonController == null) return;

        thirdPersonController.OnOpenInventory -= ToggleInventory;
    }
}