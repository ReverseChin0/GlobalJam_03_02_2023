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
    public Collider2D crouch_col;

    public PhysicsMaterial2D noFriction;
    public PhysicsMaterial2D fullFriction;

    public float cooldown = 0f;

    private void Start()
    {
        ground = GetComponent<PlayerGroundCheck>();
        jump = GetComponent<PlayerJump>();
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (cooldown < 0f)
        {
            if (ground.IsGrounded() && crouchInput && !isCrouching)
            {
                isCrouching = true;
                crouch_col.enabled = true;
                col.enabled = false;
                ground.m_GroundCheck.transform.GetComponent<BoxCollider2D>().sharedMaterial = fullFriction;
            }
            if (isCrouching && !crouchInput)
            {
                isCrouching = false;
                col.enabled = true;
                crouch_col.enabled = false;
                ground.m_GroundCheck.transform.GetComponent<BoxCollider2D>().sharedMaterial = noFriction;
            }
            if (jump.m_IsJumping)
            {
                cooldown = 0.1f;
                isCrouching = false;
                col.enabled = true;
                crouch_col.enabled = false;
                ground.m_GroundCheck.transform.GetComponent<BoxCollider2D>().sharedMaterial = noFriction;
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
