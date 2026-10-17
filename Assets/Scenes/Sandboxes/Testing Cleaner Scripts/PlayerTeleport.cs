using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTeleport : MonoBehaviour
{


    private void OnEnable()
    {
        Enforcer.OnConsequenceComplete += TeleportPlayer;
        CasinoEntrance.OnEnteredCasino += TeleportPlayer;
        CasinoEntrance.OnExitedCasino += TeleportPlayer;
        StubbedCutsceneManager.OnPlayerTeleport += TeleportPlayer;
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


        GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }

}
