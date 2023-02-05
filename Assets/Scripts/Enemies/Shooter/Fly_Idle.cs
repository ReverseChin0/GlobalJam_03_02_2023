using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class Fly_Idle : MonoBehaviour
{
    private Enemy_StateMachine m_StateMachine;
    private Transform m_Player;
    [Header("Variables")]
    private float m_Duration;
    [SerializeField] private Vector2 m_DurationRandomRange;

    private void Awake()
    {
        m_StateMachine = GetComponent<Enemy_StateMachine>();
        m_Player = GameObject.Find("P_Player").transform;
    }

    private void Update()
    {
        if (transform.position.x < m_Player.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnEnable()
    {
        m_Duration = Random.Range(m_DurationRandomRange.x, m_DurationRandomRange.y);
        StartCoroutine(StartNextState());
    }

    private IEnumerator StartNextState()
    {
        yield return new WaitForSeconds(m_Duration);
        m_StateMachine.NextState();
    }
}
