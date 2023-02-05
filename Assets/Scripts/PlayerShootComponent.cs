using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootComponent : MonoBehaviour
{
    [SerializeField]
    private Transform _aimArrow;
    [SerializeField]
    private float _shootRate = 0.2f, _shootForce = 10, _attackRangeRadius = 10, _maxUpShootAngle=90, _minDownShootAngle=45;

    [SerializeField]
    bool _facingRight = true;

    /*SHOOTING*/
    private float _aimAngle;
    private float _lastShot=0;
    private bool _isShooting;
    /*END SHOOTING*/

    Vector3 _targetEnemie;

    List<Transform> _currentEnemies = new List<Transform>();

    private void Awake()
    {
        foreach(var obj in GameObject.FindGameObjectsWithTag("Enemies"))
        {
            _currentEnemies.Add(obj.transform);
        }        
    }

    private void Update()
    {
        AimBehaviour();
        ShootBehaviour();
    }


    public void ShootBehaviour()//llamar cuando se presione el disparo
    {
        _lastShot -= Time.deltaTime;
        if (_isShooting)
        {
            if (_lastShot <= 0)
            {
                ShootProjectile();
                _lastShot = _shootRate;
            }
        }       
    }


    public void AimBehaviour()
    {
        _targetEnemie = GetClosestEnemiePosition();
        if (_targetEnemie == Vector3.zero)
        {
            _targetEnemie = _facingRight ? Vector2.right : -Vector2.right;
            _targetEnemie += transform.position;
        }
        Vector3 lookDir = _targetEnemie - transform.position;

        lookDir.Normalize();
        _aimAngle = Vector2.SignedAngle(Vector2.right, lookDir);
        _aimArrow.eulerAngles = new Vector3(0, 0, _aimAngle);
    }

    private Vector3 GetClosestEnemiePosition()
    {
        Vector3 closest = Vector3.zero;
        float closestDistance = 0;
        foreach (Transform enemie in _currentEnemies)
        {/*Prioridad =>  distancia -> dentro de los angulos ->*/

            bool inAngle = false;
            //angulo entre mi transform right y el vector que nos une al enemigo
            float _angleToPlayer = FindDegree(Vector2.right, enemie.position - transform.position);                                    

            if (_facingRight)
            {
                if (_angleToPlayer < _maxUpShootAngle || _angleToPlayer > 360 - _minDownShootAngle) inAngle = true;
            }
            else
            {
                if (_angleToPlayer > 180 - _maxUpShootAngle && _angleToPlayer < 180 + _minDownShootAngle) inAngle = true;
            }

            float distance = Vector3.Distance(enemie.position, transform.position);//obtenemos distancia
            
            if (inAngle && distance < _attackRangeRadius && (closest == Vector3.zero || distance < closestDistance))
            {
                closestDistance = distance;
                closest = enemie.position;
            }
        }
        return closest;
    }

    public void ShootProjectile()
    {
        float radians = _aimAngle * (Mathf.PI / 180);
        Vector3 shootDir = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        GameObject bullet = ObjPooler._instance.Spawn("NormalBullet", transform.position + shootDir, Quaternion.identity);
        SimpleProjectile simpleProjectile = bullet.GetComponent<SimpleProjectile>();
        simpleProjectile?.Shoot(shootDir, 100);
    }

    float FindDegree(Vector2 from, Vector2 to)
    {
        float angle = Vector2.SignedAngle(from,to);
        if (angle < 0)
        {
            angle = 360 - angle * -1;
        }
        return angle;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (isActiveAndEnabled)
        {
            if (context.performed)
                _isShooting = true;

            if (context.canceled)
                _isShooting = false;                
        }
    }


    private void OnDrawGizmosSelected()
    {        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_targetEnemie, 0.5f);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _attackRangeRadius);


        Gizmos.color = Color.yellow;
        float upperRadians = _maxUpShootAngle * (Mathf.PI / 180);
        Vector3 upShootDir = new Vector2(Mathf.Cos(upperRadians), Mathf.Sin(upperRadians));
        float lowerRadians = _minDownShootAngle * -(Mathf.PI / 180);
        Vector3 downShootDir = new Vector2(Mathf.Cos(lowerRadians), Mathf.Sin(lowerRadians));
        if (!_facingRight)
        {
            upShootDir.x *= -1;
            downShootDir.x *= -1;
        }

        Gizmos.DrawLine(transform.position, transform.position + (upShootDir * _attackRangeRadius));
        Gizmos.DrawLine(transform.position, transform.position + (downShootDir * _attackRangeRadius));


    }
}
