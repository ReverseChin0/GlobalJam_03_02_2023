using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouch : MonoBehaviour
{
    public bool crouchInput = false;
    public bool isCrouching = false;
    public bool otherInput = false;
    public PlayerGroundCheck ground;
    private PlayerJump jump;
    private Collider2D col;

    public PhysicsMaterial2D noFriction;
    public PhysicsMaterial2D fullFriction;

    public float cooldown = 0f;

    private void Start()
    {
        ground = GetComponent<PlayerGroundCheck>();
        jump = GetComponent<PlayerJump>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (cooldown < 0f)
        {
            if (ground.IsGrounded() && crouchInput && !isCrouching)
            {
                isCrouching = true;
                ground.m_GroundCheck.transform.GetComponent<Collider2D>().sharedMaterial = fullFriction;
            }
            if (isCrouching && !crouchInput)
            {
                isCrouching = false;
                ground.m_GroundCheck.transform.GetComponent<Collider2D>().sharedMaterial = noFriction;
            }
            if (jump.m_IsJumping || jump.cancelCrouch)
            {
                cooldown = 0.1f;
                isCrouching = false;
                ground.m_GroundCheck.transform.GetComponent<Collider2D>().sharedMaterial = noFriction;
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
            if (context.started)
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
