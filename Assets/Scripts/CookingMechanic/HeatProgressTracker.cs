using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HeatProgressTracker : MonoBehaviour
{
    [SerializeField] private Image heatProgressBar;//how hot the pot->keep in zone->start counting doness
    [SerializeField] private Image donessProgressBar;//how close the pot is to being done->next step
    [SerializeField] private float heatAmount = 5f;//how much the progress bar fills per push
    [SerializeField] private float donessSpeed = 1f;//how much the progress bar fills per second
    [SerializeField] private float coolDownSpeed = 0.35f;//how much the progress bar empties per second
    [SerializeField] private float pumpWaitTime = 0.25f;//how much time must pass without heat up before the bar starts to cool down
    [SerializeField] private float targetMinRotation = 0f;
    [SerializeField] private float targetMaxRotation = 10f;
    [SerializeField] private float heatUpTargetMax = 90f;//
    [SerializeField] private float heatUpTargetMin = 70f;//
    [SerializeField] private bool canPump = true;


    [SerializeField] private float currentFill = 0f;
    private float currentDoness = 0f;
    //private bool isProcessingDoneness = false;
    private Coroutine  waitCoroutine= null;
    [SerializeField] private Transform belloObject;

    [SerializeField] private GameObject cookingPotVFX;

    void Start()
    {
        currentFill = 0f;
        canPump = true;
    }


    private void Update()
    {
        
        float zRotation = GetZRotation();

        bool isInPumpZone = zRotation >= targetMinRotation && zRotation <= targetMaxRotation;

        // ENTERING the zone → count as a pump
        if(isInPumpZone && canPump)
        {
            Debug.Log("pumped");
            HeatUp();
            canPump = false;
        } 
        // EXITING the zone → start cooldown timer
         if(!isInPumpZone && !canPump)
         {
            if(waitCoroutine == null)
            {
                waitCoroutine = StartCoroutine(WaitForNextPump());
            }
         }
        
        //Debug.Log($"Z: {zRotation} | InZone: {isInPumpZone} | CanPump: {canPump}");

        EvaluateHeat();
        CoolDown();
        if ( currentDoness >= 100f)
        {
            EvaluateDoness();
        }
    }
    float GetZRotation()
    {
        //this function converts the local Z rotation of the object 
        // to a value between -180 and 180, where 0 is the default position, 
        // positive values are clockwise rotations, and negative values are counterclockwise rotations. 
        // This makes it easier to compare the rotation to the target min and max values for the pump zone.
        float z = belloObject.localEulerAngles.z;
        return (z > 180f) ? z - 360f : z;
    }
    private void HeatUp()
    {
        Debug.Log("Heat Up!");
        currentFill += heatAmount;
        heatProgressBar.fillAmount = currentFill / 100.0f;
        canPump = false;
    }
    private void EvaluateHeat()
    {
        if(currentFill >= heatUpTargetMin && currentFill <= heatUpTargetMax)
        {//currentFill is in good heatzone, start filling doness bar
            Debug.Log("In heat zone, filling doness bar");
            currentDoness += donessSpeed * Time.deltaTime;
            donessProgressBar.fillAmount = currentDoness / 100.0f;
        }
        
    }
    
    private void CoolDown()
    {
        currentFill -= coolDownSpeed * Time.deltaTime;
        if (currentFill < 0f)
        {
            currentFill = 0f;
        }
        heatProgressBar.fillAmount = currentFill / 100.0f;
        
    }
    private void EvaluateDoness()
    {
        //isProcessingDoneness = true;

        currentDoness = 0f;//reset doness for next time
        donessProgressBar.fillAmount = 0f;
        //Play VFX
        cookingPotVFX.SetActive(true);
        Animator animator = cookingPotVFX.GetComponent<Animator>();
        animator.SetTrigger("StartVFX");//put animation event at the end of animation

        //CookingManager.Instance.StartAdditive();

        //isProcessingDoneness = false;
        
    }    
    private IEnumerator WaitForNextPump()
    {
        yield return new WaitForSeconds(pumpWaitTime);
        canPump = true;
        waitCoroutine = null;
    }
    
    

}