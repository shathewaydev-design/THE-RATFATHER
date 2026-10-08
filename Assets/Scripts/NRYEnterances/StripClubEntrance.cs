using UnityEngine;

public class StripClubEntrance : MonoBehaviour
{
    bool hasEntered = false;

    public void StartStubbedCutscene()
    {
        StubbedCutsceneManager.Instance.OpenCutscene(2);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasEntered)
            {
                StartStubbedCutscene();
                hasEntered = true;
            }
            else
            {
                return;
            }

        }
    }


}
