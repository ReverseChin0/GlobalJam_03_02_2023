using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public JicamaAnimManager m_Animations;
    [HideInInspector] public Vector2 m_Horizontal;
    private Rigidbody2D m_Rigibody;
    [SerializeField] private float m_Speed;
    public bool IsFacingRight => m_IsFacingRight;
    private bool m_IsFacingRight;

    private void Awake()
    {
        m_Rigibody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        m_Rigibody.velocity = new Vector2(m_Horizontal.x * m_Speed, m_Rigibody.velocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (isActiveAndEnabled)
        {
            m_Horizontal = context.ReadValue<Vector2>();
            m_Horizontal.Normalize();

            if (m_Horizontal.x > 0)
            {
                m_IsFacingRight = true;
                m_Animations.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            else if (m_Horizontal.x < 0)
            {
                m_IsFacingRight = false;
                m_Animations.gameObject.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
            }

            if(context.started)
            {
                m_Animations.Move();
            }
            if(context.canceled)
            {
                m_Animations.Move();
            }
        }
    }
}
