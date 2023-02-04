using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    private PlayerGroundCheck m_GroundCheck;
    private Rigidbody2D m_Rigibody;
    public float jumpTimeCounter;
    private bool jumpButtonHeld;
    [SerializeField] private int maxJumpTimes;
    private int jumpTimes;

    private bool m_IsJumping;
    public float jumpingPower = 0.1f;
    public float jumpTime;
    public bool landed = false;
    public bool doubleJumpFall;

    private float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public LayerMask whatIsGround;

    void Awake()
    {
        m_GroundCheck = GetComponent<PlayerGroundCheck>();
        m_Rigibody = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        //jumpButtonHeld = false;
        coyoteTimeCounter = 0f;
        jumpTimeCounter = 0f;
        m_Rigibody.velocity = new Vector2(m_Rigibody.velocity.x, 0);
    }

    public void FixedUpdate()
    {
        if (m_Rigibody.velocity.y < 0)
        {
            m_IsJumping = false;
        }
        if (m_IsJumping)
        {
            if (jumpButtonHeld)
            {
                if (jumpTimeCounter > 0)
                {
                    m_Rigibody.velocity = new Vector2(m_Rigibody.velocity.x, jumpingPower);
                    jumpTimeCounter -= Time.unscaledDeltaTime * (1 / Time.timeScale);
                }
                else
                {
                    m_IsJumping = false;
                }
            }
        }
        if (m_GroundCheck.IsGrounded())
        {
            if (!m_IsJumping)
            {
                if (!landed)
                {
                    Land();
                    jumpTimes = maxJumpTimes;
                }
            }
        }
        else
        {
            if (coyoteTimeCounter <= 0f && !doubleJumpFall)
            {
                if (maxJumpTimes > 1)
                {
                    if (jumpTimes > 0)
                    {
                        jumpTimes = 1;
                    }
                    doubleJumpFall = true;
                }
                else
                {
                    jumpTimes = 0;
                }

            }
            coyoteTimeCounter -= Time.deltaTime;
            landed = false;
        }
    }

    public void Land()
    {
        landed = true;
        doubleJumpFall = false;
        coyoteTimeCounter = coyoteTime;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isActiveAndEnabled)
        {
            if (context.performed && (coyoteTimeCounter > 0f || jumpTimes > 0))
            {
                jumpTimes -= 1;
                m_IsJumping = true;
                jumpButtonHeld = true;
                jumpTimeCounter = jumpTime;
                m_Rigibody.velocity = new Vector2(m_Rigibody.velocity.x, jumpingPower / Time.deltaTime * Time.unscaledDeltaTime);
                
            }
            if (context.canceled)
            {
                jumpButtonHeld = false;
                coyoteTimeCounter = 0f;
            }
        }
    }
}
