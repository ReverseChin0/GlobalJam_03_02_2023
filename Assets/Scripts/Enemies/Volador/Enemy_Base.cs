using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    [Header("Stats")]
    private byte m_CurrentHealth;
    [SerializeField] private byte m_MaxHealth;
    private Rigidbody2D m_Rigibody;

    private void Awake()
    {
        m_CurrentHealth = m_MaxHealth;
        m_Rigibody = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(byte a_Amount)
    {
        m_CurrentHealth -= a_Amount;
        if (m_CurrentHealth <= 0)
            Death();
    }

    private void Death()
    {
        //Reproducir animacion de muerte
        //Sumar puntos
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
