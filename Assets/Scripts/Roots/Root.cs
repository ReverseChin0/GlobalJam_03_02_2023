using UnityEngine;

public class Root : MonoBehaviour
{
    private Collider2D m_Collider;
    private GameObject m_Player;
    private PlayerHealth m_Health;
    private PlayerCrouch m_Crouch;

    private void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_Player = collision.gameObject;
        m_Health = m_Player.GetComponent<PlayerHealth>();
        m_Crouch = m_Player.GetComponent<PlayerCrouch>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_Crouch.crouchInput)
        {
            m_Health.ResetHealth();
            m_Collider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_Player = null;
    }
}
