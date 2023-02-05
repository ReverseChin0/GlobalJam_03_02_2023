using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private byte m_CurrentHealth;
    [SerializeField] private byte m_MaxHealth;

    public void TakeDamage(byte a_Amount)
    {
        m_CurrentHealth -= a_Amount;
        if (m_CurrentHealth <= 0)
            Death();
    }

    public void ResetHealth()
    {
        Debug.Log("VIDA REGENERADA");
        m_CurrentHealth = m_MaxHealth;
    }

    private void Death()
    {
        //Animacion de muerte
        //Pantalla de muerte
    }
}
