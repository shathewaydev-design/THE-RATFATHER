using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{


    private void OnEnable()
    {
        Enforcer.OnConsequenceComplete += TeleportPlayer;
    }

    private void OnDisable()
    {
        Enforcer.OnConsequenceComplete -= TeleportPlayer;
    }



    public void TeleportPlayer(Transform teleportLocation)
    {
        transform.position = teleportLocation.position;
    }

}
