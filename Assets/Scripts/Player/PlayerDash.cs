using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D m_Rigibody;
    private Collider2D m_Collider;
    [SerializeField] private float m_Force;
    private PlayerMovement m_Movement;

    private void Awake()
    {
        m_Rigibody = GetComponent<Rigidbody2D>();
        m_Movement = GetComponent<PlayerMovement>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (isActiveAndEnabled)
        {
            if (context.performed)
            {
                //m_Collider.enabled = false;
                if(m_Movement.IsFacingRight)
                    m_Rigibody.AddForce(Vector2.right * m_Force);
                else
                    m_Rigibody.AddForce(Vector2.left * m_Force);
            }
        }
    }
}
