using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{


    private void OnEnable()
    {
        Enforcer.OnConsequenceComplete += TeleportPlayer;
        CasinoEntrance.OnEnteredCasino += TeleportPlayer;
        CasinoEntrance.OnExitedCasino += TeleportPlayer;
    }

    private void OnDisable()
    {
        Enforcer.OnConsequenceComplete -= TeleportPlayer;
        CasinoEntrance.OnEnteredCasino -= TeleportPlayer;
        CasinoEntrance.OnExitedCasino -= TeleportPlayer;

    }



    public void TeleportPlayer(Transform teleportLocation)
    {
        Debug.Log("teleporting player...");

        CharacterController controller = GetComponent<CharacterController>();

        controller.enabled = false;
        transform.position = teleportLocation.position;
        controller.enabled = true;
    }

}
