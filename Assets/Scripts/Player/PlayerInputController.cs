using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance;
    PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();

        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Vector2 GetMovementInput()
    {
        return playerInput.Player.Move.ReadValue<Vector2>();
    }
    public float GetDashInput()
    {
        return playerInput.Player.Dash.ReadValue<float>();
    }
    public float GetShootInput()
    {
        return playerInput.Player.Attack.ReadValue<float>();
    }
    public float GetInteractInput()
    {
        return playerInput.Player.Interact.ReadValue<float>();
    }

    public bool GetPauseInput()
    {
        return playerInput.Player.Pause.WasPressedThisFrame();

    }
}
