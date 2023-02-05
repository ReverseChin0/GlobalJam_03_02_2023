using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public Transform m_GroundCheck;
    public LayerMask m_GroundLayer;
    public bool m_IsGrounded;

    public bool IsGrounded()
    {
        m_IsGrounded = Physics2D.OverlapCircle(m_GroundCheck.position, 0.3f, m_GroundLayer);
        return Physics2D.OverlapCircle(m_GroundCheck.position, 0.3f, m_GroundLayer);
    }

    public bool isOneWay()
    {
        if (Physics2D.OverlapCircle(m_GroundCheck.position, 0.3f, m_GroundLayer) != false)
            return Physics2D.OverlapCircle(m_GroundCheck.position, 0.3f, m_GroundLayer).transform.CompareTag("Platform");
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_GroundCheck.position, 0.3f);
    }
}
