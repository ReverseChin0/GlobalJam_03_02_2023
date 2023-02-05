using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouch : MonoBehaviour
{
    public bool crouchInput = false;
    public bool isCrouching = false;
    public bool otherInput = false;
    public PlayerGroundCheck ground;
    private PlayerJump jump;
    public float cooldown = 0f;

    private void Start()
    {
        ground = GetComponent<PlayerGroundCheck>();
        jump = GetComponent<PlayerJump>();
    }

    private void Update()
    {
        if (cooldown < 0f)
        {
            if (ground.IsGrounded() && crouchInput && !isCrouching)
            {
                isCrouching = true;
            }
            if (isCrouching && !crouchInput)
            {
                isCrouching = false;
            }
            if (jump.m_IsJumping || jump.cancelCrouch)
            {
                cooldown = 0.1f;
                isCrouching = false;
            }
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (isActiveAndEnabled)
        {
            if (context.performed)
            {
                crouchInput = true;
            }
            if (context.canceled)
            {
                crouchInput = false;
            }
        }
    }
}
