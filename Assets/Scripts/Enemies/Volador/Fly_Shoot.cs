using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly_Shoot : MonoBehaviour
{
    private Enemy_StateMachine m_StateMachine;
    [SerializeField] private GameObject m_ProjectilePrefab;
    [SerializeField] private Transform m_StartPosition;
    [SerializeField] private int m_ProjectileNumber;
    [SerializeField] private float m_Cadence;
    private WaitForSeconds m_Wait;
    [SerializeField] private float m_AnticipationTime;
    private WaitForSeconds m_Anticipation;
    [SerializeField] private float m_WaitToStart;
    private WaitForSeconds m_WaitTo;
    private Transform m_TargetPosition;

    private void Awake()
    {
        m_StateMachine = GetComponent<Enemy_StateMachine>();
        m_TargetPosition = GameObject.Find("P_Player").transform;
        m_Wait = new WaitForSeconds(m_Cadence);
        m_Anticipation = new WaitForSeconds(m_AnticipationTime);
        m_WaitTo = new WaitForSeconds(m_WaitToStart);
    }

    private void OnEnable()
    {
        m_TargetPosition = GameObject.Find("P_Player").transform;
        StartCoroutine(AnticipationStart());
    }


    private IEnumerator AnticipationStart()
    {
        yield return m_WaitTo;
        Shoot();
    }

    private void Shoot()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        //Animacion o algo de anticipacion
        yield return m_Anticipation;

        for (int i = 0; i < m_ProjectileNumber; i++)
        {
            ShootProjectile();
            yield return m_Wait;
        }

        m_StateMachine.NextState();
    }

    public void ShootProjectile()
    {
        Vector3 lookDir = m_TargetPosition.position - transform.position;
        lookDir.Normalize();
        float _aimAngle = Vector2.SignedAngle(Vector2.right, lookDir);
        float radians = _aimAngle * (Mathf.PI / 180);
        Vector3 shootDir = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        GameObject _bullet = Instantiate(m_ProjectilePrefab, m_StartPosition.position, Quaternion.identity);
        Projectile simpleProjectile = _bullet.GetComponent<Projectile>();
        simpleProjectile?.Shoot(shootDir, 15);
    }
}
