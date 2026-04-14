using UnityEngine;
using UnityEngine.UI;

public class PourMilkStep : MonoBehaviour
{
    public Transform milkCarton;
    public float rotateSpeed = 100f;

    public Image progressBar;
    public float fillSpeed = 20f;

    public float targetMin = 40f;
    public float targetMax = 60f;

    private float currentFill = 0f;
    
    void Update()
    {
        HandleRotation();
        HandlePour();
    }

    void HandleRotation()
    {
        float input = 0;

        if (CookingManager.Instance.tiltLeft.IsPressed()) 
        {
            input += 1;
        }
        if (CookingManager.Instance.tiltRight.IsPressed()) 
        {
            input -= 1;
        }

        milkCarton.Rotate(Vector3.forward * input * rotateSpeed * Time.deltaTime);
    }

    void HandlePour()
    {
        float tilt = Mathf.Abs(milkCarton.localEulerAngles.z);
        if (tilt > 180) tilt = 360 - tilt;

        float pourAmount = tilt / 90f; // normalized

        if (pourAmount > 0.2f)
        {
            currentFill += pourAmount * fillSpeed * Time.deltaTime;
            progressBar.fillAmount = currentFill;
        }

        if (currentFill >= targetMin)
        {
            Evaluate();
        }
    }

    void Evaluate()
    {
        if (currentFill >= targetMin && currentFill <= targetMax)
        {
            Debug.Log("Perfect pour");
        }
        else
        {
            Debug.Log("Overpour penalty");
        }

        CookingManager.Instance.StartFlavor();
        gameObject.SetActive(false);
    }
}