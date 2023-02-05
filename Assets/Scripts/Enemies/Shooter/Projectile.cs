using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    private float m_LifeTime = 2;
    [SerializeField] private byte m_Damage;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }
    public void Shoot(Vector2 direction, float force)
    {

        m_Rigidbody.velocity = direction * force;
        StartCoroutine(KillProjectileRoutine());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(m_Damage);
        }
        Destroy(gameObject);
    }

    IEnumerator KillProjectileRoutine()
    {
        yield return new WaitForSeconds(m_LifeTime);
        Destroy(gameObject);
    }
}
