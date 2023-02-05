using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public JicamaAnimManager m_Animations;
    private int m_CurrentHealth;
    [SerializeField] private int m_MaxHealth;

    private void Awake()
    {
        m_Animations = GetComponentInChildren<JicamaAnimManager>();
    }

    public void TakeDamage(int a_Amount)
    {
        m_Animations.Hurt();
        m_CurrentHealth -= a_Amount;
        if (m_CurrentHealth <= 0)
            Death();
    }

    public void ResetHealth()
    {
        m_Animations.Burrow();
        Debug.Log("VIDA REGENERADA");
        m_CurrentHealth = m_MaxHealth;
    }

    private void Death()
    {
        //Animacion de muerte
        //Pantalla de muerte
    }
}
