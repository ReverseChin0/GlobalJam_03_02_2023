using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Movement : MonoBehaviour
{
    [SerializeField] private LayerMask m_Ground;
    [SerializeField] private Vector3 m_RaycastOffset;
    [SerializeField] private float _moveSpeed=3f;
    private Rigidbody2D m_Rigibody;
    public bool m_IsFacingRight = true;

    private void Awake()
    {
        m_Rigibody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_IsFacingRight = Random.Range(0, 2) == 0 ? false : true;
        if(!m_IsFacingRight)
            m_RaycastOffset = m_RaycastOffset * -1f;
    }

    private void Update()
    {
        if(!Physics2D.Raycast(transform.position + m_RaycastOffset, Vector2.down, 2, m_Ground))
        {
            m_IsFacingRight = !m_IsFacingRight;
            m_RaycastOffset = m_RaycastOffset * -1f;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        m_Rigibody.velocity = new Vector2(m_IsFacingRight ? _moveSpeed : -_moveSpeed, m_Rigibody.velocity.y);
    }
}
