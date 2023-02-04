using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D m_Rigibody;
    [SerializeField] private float m_Speed;
    private PlayerMovement m_Movement;

    private void Awake()
    {
        m_Rigibody = GetComponent<Rigidbody2D>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (isActiveAndEnabled)
        {
            Debug.Log("DASH");
            m_Rigibody.AddForce(Vector2.right * 50, ForceMode2D.Impulse);
            /*if(m_Movement.m_Horizontal.x > 0)
                m_Rigibody.AddForce(Vector2.right * m_Speed, ForceMode2D.Impulse);
            else
                m_Rigibody.AddForce(Vector2.left * m_Speed, ForceMode2D.Impulse);*/
        }
    }
}
