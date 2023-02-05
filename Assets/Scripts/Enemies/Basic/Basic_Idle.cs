using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Idle : MonoBehaviour
{
    private Enemy_StateMachine m_StateMachine;

    [Header("Variables")]
    private float m_Duration;
    [SerializeField] private Vector2 m_DurationRandomRange;

    private void Awake()
    {
        m_StateMachine = GetComponent<Enemy_StateMachine>();
    }

    private void OnEnable()
    {
        m_Duration = Random.Range(m_DurationRandomRange.x, m_DurationRandomRange.y);
        StartCoroutine(StartNextState());
    }

    private IEnumerator StartNextState()
    {
        yield return new WaitForSeconds(m_Duration);
        m_StateMachine.ActivateState(m_StateMachine.m_States[1]);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
