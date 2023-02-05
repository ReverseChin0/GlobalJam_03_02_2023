using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Attack : MonoBehaviour
{
    private Enemy_StateMachine m_StateMachine;
    [SerializeField] private byte m_Damage;
    [SerializeField] private float m_Radius;
    [SerializeField] private LayerMask m_PlayerLayer;
    [SerializeField] private Vector3 m_Offset;
    [SerializeField] private float m_AnticipationTime;
    private WaitForSeconds m_Wait;
    private Vector3 m_OffsetP;
    private Vector3 m_OffsetN;
    private Basic_Movement m_Movement;

    private void Awake()
    {
        m_StateMachine = GetComponent<Enemy_StateMachine>();
        m_Movement = GetComponent<Basic_Movement>();
        m_OffsetP = m_Offset;
        m_OffsetN = m_Offset * -1f;
        m_Wait = new WaitForSeconds(m_AnticipationTime);
    }

    private void OnEnable()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        if (m_Movement.m_IsFacingRight) m_Offset = m_OffsetP;
        else m_Offset = m_OffsetN;

        yield return m_Wait;

        //Hacer animacion de golpe
        Collider2D _col = Physics2D.OverlapCircle(transform.position + m_Offset, m_Radius, m_PlayerLayer);
        if (_col != null)
        {
            _col.gameObject.GetComponent<PlayerHealth>().TakeDamage(m_Damage);
        }
        m_StateMachine.ActivateState(m_StateMachine.m_States[0]);
    }
}
