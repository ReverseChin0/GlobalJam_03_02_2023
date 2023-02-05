using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class Basic_Movement : MonoBehaviour
{
    public SkeletonAnimation Locust_Skeleton;
    public Transform _Sprite;
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
        Locust_Skeleton.AnimationState.SetAnimation(0, "Attack", false);
        Locust_Skeleton.AnimationState.AddAnimation(0, "Idle", true, 0.0f);
    }

    private void Update()
    {
        if(!Physics2D.Raycast(transform.position + m_RaycastOffset, Vector2.down, 2, m_Ground))
        {
            m_IsFacingRight = !m_IsFacingRight;
            m_RaycastOffset = m_RaycastOffset * -1f;
        }

        if (m_IsFacingRight)
            _Sprite.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
        else
            _Sprite.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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
