using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public int pourThreshold = 45; // Minimum tilt angle to consider as pouring
    public Transform origin = null;
    public GameObject streamPrefab = null;

    private bool isPouring = false;
    private Stream currentStream = null;

    private bool pourCheck = false;
    private float calculatedPourAngle = 0f;

    private void Update()
    {
        CalculatePourAngle();
        if(calculatedPourAngle > pourThreshold)
        {
            pourCheck = true;
        }
        else
        {
            pourCheck = false;
        }

        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;

            if (isPouring)
                StartPour();//create new object streamPrefab and set currentStream to it
            else
                EndPour();//remove previous streamPrefab
        }
        
    }

    private void StartPour()
    {
        print("Start Pouring");
        currentStream = CreateStream();
        currentStream.Begin();
    }

    private void EndPour()
    {
        print("End Pouring");
        currentStream.End();
        currentStream = null;
    }

    private void CalculatePourAngle()
    {
        calculatedPourAngle = transform.localEulerAngles.z;//convert from radiant to angle
    }

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);

        return streamObject.GetComponent<Stream>();
    }
}