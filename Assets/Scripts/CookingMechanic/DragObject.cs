using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class DragObject : MonoBehaviour
{
    private ThirdPersonController controller;
    private Camera mainCamera;

    public float distanceFromCamera = 5f;
    public GameObject objectToDrag;

    private bool isDragging;
    private Vector2 mousePos;

    private void Start()
    {
        controller = ThirdPersonController.Instance;
        mainCamera = Camera.main;

        controller.OnMouseDrag += HandleDrag;
        controller.OnMousePosition += HandleMousePosition;
        Debug.Log("object distance from camera: " + (objectToDrag.transform.position.z - mainCamera.transform.position.z));
    }

    private void OnDestroy()
    {
        if (controller == null) return;

        controller.OnMouseDrag -= HandleDrag;
        controller.OnMousePosition -= HandleMousePosition;
    }

    private void HandleDrag(bool dragging)
    {
        isDragging = dragging;
    }

    private void HandleMousePosition(Vector2 pos)
    {
        mousePos = pos;
    }

    private void Update()
    {
        if (!isDragging) return;

        Vector3 screenPoint = new Vector3(mousePos.x, mousePos.y, distanceFromCamera);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPoint);

        transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CookingPot"))
        {
            CookingManager.Instance.StartHeat();
            Debug.Log("Entered area");

        }
    }

}