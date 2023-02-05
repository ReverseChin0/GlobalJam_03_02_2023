using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_StateMachine : Enemy_Base
{
    [SerializeField] public List<MonoBehaviour> m_States;
    [SerializeField] private int m_InitialStateIndex;

    private MonoBehaviour m_ActualState;
    private int m_ActualStateIndex;

    private void Awake()
    {
        m_CurrentHealth = m_MaxHealth;
        m_ActualStateIndex = m_InitialStateIndex;
    }

    private void Start()
    {
        ActivateState(m_States[m_InitialStateIndex]);
    }

    public void ActivateState(MonoBehaviour a_NewState)
    {
        if (m_ActualState != null)
            m_ActualState.enabled = false;

        m_ActualState = a_NewState;
        m_ActualState.enabled = true;
    }

    public void NextState()
    {
        m_ActualStateIndex++;
        if (m_ActualStateIndex >= m_States.Count) m_ActualStateIndex = 0;
        ActivateState(m_States[m_ActualStateIndex]);
    }
}
