using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerCollision : MonoBehaviour
{
    private Enemy_StateMachine m_StateMachine;

    private void Awake()
    {
        m_StateMachine = transform.parent.GetComponent<Enemy_StateMachine>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            m_StateMachine.ActivateState(m_StateMachine.m_States[2]);
        }
    }
}
