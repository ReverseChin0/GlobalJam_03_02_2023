using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootComponent : MonoBehaviour
{
    public JicamaAnimManager m_Animations;
    public Transform m_BulletOrigin;
    //private Transform _aimArrow;
    [SerializeField]
    private float _shootRate = 0.2f, _shootForce = 10, _attackRangeRadius = 10, _maxUpShootAngle=90, _minDownShootAngle=45;
    private PlayerMovement _Movement;

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
        _Movement = GetComponent<PlayerMovement>();
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
            _targetEnemie = _Movement.IsFacingRight ? Vector2.right : -Vector2.right;
            _targetEnemie += m_BulletOrigin.position;
        }
        Vector3 lookDir = _targetEnemie - transform.position;

        lookDir.Normalize();
        _aimAngle = Vector2.SignedAngle(Vector2.right, lookDir);
        //_aimArrow.eulerAngles = new Vector3(0, 0, _aimAngle);
    }

    private Vector3 GetClosestEnemiePosition()
    {
        Vector3 closest = Vector3.zero;
        float closestDistance = 0;
        foreach (Transform enemie in _currentEnemies)
        {/*Prioridad =>  distancia -> dentro de los angulos ->*/

            bool inAngle = false;
            //angulo entre mi transform right y el vector que nos une al enemigo
            float _angleToPlayer = FindDegree(Vector2.right, enemie.position - m_BulletOrigin.position);                                    

            if (_Movement.IsFacingRight)
            {
                if (_angleToPlayer < _maxUpShootAngle || _angleToPlayer > 360 - _minDownShootAngle) inAngle = true;
            }
            else
            {
                if (_angleToPlayer > 180 - _maxUpShootAngle && _angleToPlayer < 180 + _minDownShootAngle) inAngle = true;
            }

            float distance = Vector3.Distance(enemie.position, m_BulletOrigin.position);//obtenemos distancia
            
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
        Vector2 superior = (Vector2)(Quaternion.Euler(0, 0, _maxUpShootAngle) * Vector2.right);
        Vector2 inferior = (Vector2)(Quaternion.Euler(0, 0, 360-_minDownShootAngle) * Vector2.right);
        float porcentaje = InverseLerp(superior, inferior, shootDir);
        m_Animations.Aim_Fully_Down = porcentaje + 0.25f;
        GameObject bullet = ObjPooler._instance.Spawn("NormalBullet", m_BulletOrigin.position + shootDir, Quaternion.identity);
        //GameObject bullet = ObjPooler._instance.Spawn("NormalBullet", _origin.position, Quaternion.identity);
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
            {
                m_Animations.Turret();
                _isShooting = true;
            }

            if (context.canceled)
            {
                m_Animations.Turret();
                _isShooting = false;
            }
        }
    }

    public float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    public void RemoveMeFromList(Transform t)
    {
        _currentEnemies.Remove(t);
    }

    private void OnDrawGizmosSelected()
    {        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_targetEnemie, 0.5f);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(m_BulletOrigin.position, _attackRangeRadius);


        Gizmos.color = Color.yellow;
        float upperRadians = _maxUpShootAngle * (Mathf.PI / 180);
        Vector3 upShootDir = new Vector2(Mathf.Cos(upperRadians), Mathf.Sin(upperRadians));
        float lowerRadians = _minDownShootAngle * -(Mathf.PI / 180);
        Vector3 downShootDir = new Vector2(Mathf.Cos(lowerRadians), Mathf.Sin(lowerRadians));
        if (_Movement != null && !_Movement.IsFacingRight)
        {
            upShootDir.x *= -1;
            downShootDir.x *= -1;
        }

        Gizmos.DrawLine(transform.position, m_BulletOrigin.position + (upShootDir * _attackRangeRadius));
        Gizmos.DrawLine(transform.position, m_BulletOrigin.position + (downShootDir * _attackRangeRadius));


    }
}
