using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    [Header("Stats")]
    private int m_CurrentHealth;
    [SerializeField] private int m_MaxHealth;
    private Rigidbody2D m_Rigibody;

    private void Awake()
    {
        m_CurrentHealth = m_MaxHealth;
        m_Rigibody = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int a_Amount)
    {
        m_CurrentHealth -= a_Amount;
        if (m_CurrentHealth <= 0)
            Death();
    }

    private void Death()
    {
        GameManager.Instance.EnemyKilled();
        FindObjectOfType<PlayerShootComponent>().RemoveMeFromList(transform);
        Destroy();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
