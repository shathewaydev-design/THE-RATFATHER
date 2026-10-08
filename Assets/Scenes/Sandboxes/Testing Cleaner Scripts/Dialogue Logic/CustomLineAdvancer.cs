using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class CustomLineAdvancer : MonoBehaviour, ILineAdvancerInput
{
    public LineAdvancer LineAdvancer { get; set; }
    [SerializeField] private LineAdvancer lineAdvancer;

    [SerializeField] private InputActionReference advanceAction;

    public bool inputEnabled;

    public void OnDialogueStarted()
    {
        inputEnabled = true;
        advanceAction.action.Enable();
    }

    public void OnDialogueComplete()
    {
        inputEnabled = false;
        advanceAction.action.Disable();
    }

    private void Update()
    {
        if (!inputEnabled)
        {
            return;
        }

        if (advanceAction.action.WasPressedThisFrame())
        {
            lineAdvancer.OnInputHurryUpLines();
        }
    }

}
