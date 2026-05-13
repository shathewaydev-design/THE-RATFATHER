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

    [SerializeField] private bool movePosition = false;
    [SerializeField] private bool moveRotation = false;
    [SerializeField] private float maxRotation = 28f;
    [SerializeField] private float minRotation = 0f;

    private void Start()
    {
        controller = ThirdPersonController.Instance;
        mainCamera = Camera.main;

        controller.OnMouseDrag += HandleDrag;
        controller.OnMousePosition += HandleMousePosition;
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
        if (!isDragging)
        { 
            // objectToDrag.layer = LayerMask.NameToLayer("Default");
            return;
        }
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        // Draws a red ray in Scene view
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
        
        objectToDrag.layer = LayerMask.NameToLayer("Dragging");

        Vector3 screenPoint = new Vector3(mousePos.x, mousePos.y, distanceFromCamera);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPoint);
        
        if (movePosition)
        {
            transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        }
        else if(moveRotation)
        {
            Vector3 direction = worldPos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if(angle > maxRotation)
            {
                angle = maxRotation;
            }
            else if(angle < minRotation)
            {
                angle = minRotation;
            }
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CookingPot"))
        {
            // objectToDrag.layer = LayerMask.NameToLayer("Default");
            SoundManager.Instance.PlayUISound(SoundManager.Instance.addFlavor);

            CookingManager.Instance.StartHeat();
        }
    }

}