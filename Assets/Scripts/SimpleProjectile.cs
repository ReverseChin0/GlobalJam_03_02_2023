using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private float _lifeTime=2;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    public void Shoot(Vector2 direction, float force)
    {
        
        _rigidbody.velocity = direction * force;
        StartCoroutine(KillProjectileRoutine());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }

    IEnumerator KillProjectileRoutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }
    
}
