using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public Transform m_GroundCheck;
    public LayerMask m_GroundLayer;
    public bool isGrounded = false;

    public bool IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(m_GroundCheck.position, 0.3f, m_GroundLayer);
        return Physics2D.OverlapCircle(m_GroundCheck.position, 0.3f, m_GroundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_GroundCheck.position, 0.3f);
    }
}
