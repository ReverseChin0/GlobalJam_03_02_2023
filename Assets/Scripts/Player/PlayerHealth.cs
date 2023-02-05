using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public JicamaAnimManager m_Animations;
    private int m_CurrentHealth;
    [SerializeField] private int m_MaxHealth;
    [SerializeField] private List<GameObject> m_Icons;

    private void Awake()
    {
        m_Animations = GetComponentInChildren<JicamaAnimManager>();
        m_CurrentHealth = m_MaxHealth;
    }

    public void TakeDamage(int a_Amount)
    {
        m_Animations.Hurt();
        m_CurrentHealth -= a_Amount;
        UpdateView();
        if (m_CurrentHealth <= 0)
            Death();
    }

    public void ResetHealth()
    {
        m_Animations.Burrow();
        m_CurrentHealth = m_MaxHealth;
        UpdateView();
    }

    private void Death()
    {
        GameManager.Instance.GameOverRoutine();
    }

    private void UpdateView()
    {
        for (int i = 0; i < m_Icons.Count; i++)
        {
            if (i < m_CurrentHealth)
            {
                m_Icons[i].transform.GetChild(0).gameObject.SetActive(true);
                m_Icons[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                m_Icons[i].transform.GetChild(0).gameObject.SetActive(false);
                m_Icons[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}
