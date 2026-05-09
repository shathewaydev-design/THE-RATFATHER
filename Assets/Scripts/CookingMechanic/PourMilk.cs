using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class PourMilkStep : MonoBehaviour
{
    public Transform milkCarton;
    public float rotateSpeed = 100f;

    public Image progressBar;
    public float fillSpeed = 0.005f;

    [SerializeField] private float startingRotation = 90f;
    public float targetMin = 40f;
    public float targetMax = 60f;

    private float currentFill = 0f;
    public ThirdPersonController thirdPersonController;
    
    void Start ()
    {
        thirdPersonController = ThirdPersonController.Instance;
    }
    void OnEnable()
    {        //reset values
        currentFill = 0f;
        progressBar.fillAmount = currentFill / 100.0f;
        milkCarton.rotation = Quaternion.Euler(milkCarton.rotation.x, milkCarton.rotation.y, startingRotation);
    }
    void Update()
    {
        HandleRotation();
        HandlePour();
    }

    void HandleRotation()
    {
        float input = 0;

        if (thirdPersonController.tiltLeft.IsPressed()) 
        {
            input += 1;
        }
        if (thirdPersonController.tiltRight.IsPressed()) 
        {
            input -= 1;
        }

        milkCarton.Rotate(Vector3.forward * input * rotateSpeed * Time.deltaTime);

        // Clamp rotation
        Vector3 angles = milkCarton.localEulerAngles;

        // Convert 0-360 into -180 to 180
        if (angles.z > 180f)
            angles.z -= 360f;

        angles.z = Mathf.Clamp(angles.z, 0f, 100f);

        milkCarton.localEulerAngles = angles;
    }

    void HandlePour()
    {
        float tilt = Mathf.Abs(milkCarton.localEulerAngles.z);
        if (tilt > 360f) 
        {
            tilt = 360f - tilt;
        }

        float pourAmount = tilt / 90f; // normalized

        if (pourAmount > 0.5f)//start pouring when tilt is greater than 45 degrees
        {
            currentFill += pourAmount * fillSpeed * Time.deltaTime;
            progressBar.fillAmount = currentFill / 100.0f;
        }

        if (pourAmount < 0.5f && currentFill > 0f)//stop pouring when tilt is less than 45 degrees
        {
            Evaluate();
        }
    }

    void Evaluate()
    {
        bool finishPour = false;
        if (currentFill >= targetMin && currentFill <= targetMax)
        {
            //Debug.Log("Perfect pour");
            finishPour = true;
        }
        if(currentFill > targetMax)
        {
            //Debug.Log("Overpour penalty");
            finishPour = true;
        }
        if(finishPour)
        {
            currentFill = 0f;
            CookingManager.Instance.StartFlavor();
            gameObject.SetActive(false);
        }
    }
}