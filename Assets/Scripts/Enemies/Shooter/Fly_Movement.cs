using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly_Movement : MonoBehaviour
{
    private Enemy_StateMachine m_StateMachine;
    [SerializeField] private float m_Speed;
    private Vector2 m_CurrentTarget;
    private Transform m_Player;

    private void Awake()
    {
        m_StateMachine = GetComponent<Enemy_StateMachine>();
        GetNextTarget();
        m_Player = GameObject.Find("P_Player").transform;
    }

    private void OnEnable()
    {
        Move();
    }

    private void GetNextTarget()
    {
        m_CurrentTarget = GetRandomScreenPosition();
    }

    private Vector2 GetRandomScreenPosition()
    {
        float spawnY = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        float spawnX = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

        Vector2 _position = new Vector2(spawnX, spawnY);

        return _position;
    }


    private void Update()
    {
        if (transform.position.x < m_Player.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void Move()
    {
        LeanTween.move(gameObject, m_CurrentTarget, m_Speed).setOnComplete(NextState);
    }

    private void NextState()
    {
        GetNextTarget();
        m_StateMachine.NextState();
    }
}
