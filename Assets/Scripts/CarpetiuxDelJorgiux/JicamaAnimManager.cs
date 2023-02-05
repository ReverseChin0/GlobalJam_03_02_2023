using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;


public class JicamaAnimManager : MonoBehaviour
{
    public SkeletonAnimation Jicama_Skeleton; //Used to control everything realted to the animations.
    private TrackEntry attackTrackEntry, aimTrackEntry, shootingTrackEntry, dashTrackEntry, hurtTrackEntry;
    public Collider2D Front_Hand_Collider, Back_Hand_Collider, Front_Foot_Collider, Back_Foot_Collider, Grab_Collider;
    public int Jump_state = 0;
    public bool Moving = false;
    [Range(0.0f, 1.0f)]
    public float Aim_Fully_Down = 0.5f;
    public bool Shooting = false;
    public int Crack_State = 0;
    public bool Grabbing = false;
    public int Attack_state = 0;
    public Transform BulletOrigin;
    public PlayerShootComponent PlayerShoot;

    private void Start()
    {
        Jicama_Skeleton.AnimationState.Event += HandleEvent; //To listen to animation events via void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    }

    private void Update()
    {
        if (Shooting)
        {
         //   aimTrackEntry.Alpha = Aim_Fully_Down;
        }
    }

    void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        /*if (e.Data.Name == "Atk_Front_Hand")
        {
            if(Front_Hand_Collider.enabled)
            {
                Front_Hand_Collider.enabled = false;
            }
            else
            {
                Front_Hand_Collider.enabled = true;
            }
        }

        if (e.Data.Name == "Atk_Back_Hand")
        {
            if (Back_Hand_Collider.enabled)
            {
                Back_Hand_Collider.enabled = false;
            }
            else
            {
                Back_Hand_Collider.enabled = true;
            }
        }

        if (e.Data.Name == "Atk_Front_Foot")
        {
            if (Front_Hand_Collider.enabled)
            {
                Front_Hand_Collider.enabled = false;
            }
            else
            {
                Front_Hand_Collider.enabled = true;
            }
        }

        if (e.Data.Name == "Atk_Back_Foot")
        {
            if (Back_Foot_Collider.enabled)
            {
                Back_Foot_Collider.enabled = false;
            }
            else
            {
                Back_Foot_Collider.enabled = true;
            }
        }*/

        if (e.Data.Name == "Shoot")
        {
            //PlayerShoot.ShootProjectile(BulletOrigin);
        }

        if (e.Data.Name == "Grab")
        {
            if (Grab_Collider.enabled)
            {
                Grab_Collider.enabled = false;
            }
            else
            {
                Grab_Collider.enabled = true;
            }
        }

        if (e.Data.Name == "Burrowed")
        {
            Crack_State = 0;
            Jicama_Skeleton.AnimationState.SetEmptyAnimation(3, 0.25f);
        }
    }

    public void LightAttack()
    {
        //tiltTrackEntry.MixBlend = MixBlend.Add;
        switch(Attack_state)
        {
            case 1:
                attackTrackEntry = Jicama_Skeleton.AnimationState.AddAnimation(4, "Attack_Light_B", false, 0);
                Attack_state = 2;
                break;
            case 2:
                attackTrackEntry = Jicama_Skeleton.AnimationState.AddAnimation(4, "Attack_Light_C", false, 0);
                Attack_state = 0;
                break;
            default:
                attackTrackEntry = Jicama_Skeleton.AnimationState.SetAnimation(4, "Attack_Light_A", false);
                Attack_state = 1;
                break;
        }
        attackTrackEntry.Complete += delegate //To run what's inside the brackets when the last queued animation finishes.
        {
            Jicama_Skeleton.AnimationState.SetEmptyAnimation(4, 0.25f);
            attackTrackEntry = null;
        };

    }
    public void HeavyAttack()
    {
        if(Jump_state != 0) //Aerial
        {
            attackTrackEntry = Jicama_Skeleton.AnimationState.SetAnimation(4, "Attack_Heavy_Air_A", false);
        }
        else
        {
            attackTrackEntry = Jicama_Skeleton.AnimationState.SetAnimation(4, "Attack_Heavy_Ground_A", false);
        }
        attackTrackEntry.Complete += delegate //To run what's inside the brackets when the last queued animation finishes.
        {
            Jicama_Skeleton.AnimationState.SetEmptyAnimation(4, 0.25f);
            attackTrackEntry = null;
        };
    }
    public void Jump()
    {
        if(Jump_state == 0)
        {
            Jicama_Skeleton.AnimationState.SetAnimation(0, "Jump", false);
            Jicama_Skeleton.AnimationState.AddAnimation(0, "Airborne_Up", true, 0.0f);
            Jump_state = 1;
        }
        else if (Jump_state == 1)
        {
            Jicama_Skeleton.AnimationState.SetAnimation(0, "Jump_Extra", false);
            Jicama_Skeleton.AnimationState.AddAnimation(0, "Airborne_Up", true, 0.0f);
        }
    }
    public void Crack()
    {
        switch(Crack_State)
        {
            case 0:
                Jicama_Skeleton.AnimationState.SetAnimation(3, "Pot_Crack_0", false);
                Crack_State = 1;
                break;
            case 1:
                Jicama_Skeleton.AnimationState.SetAnimation(3, "Pot_Crack_1", false);
                Crack_State = 2;
                break;
            case 2:
                Jicama_Skeleton.AnimationState.SetAnimation(0, "Death", false);
                Crack_State = 3;
                break;
            default:
                if (Moving)
                {
                    Jicama_Skeleton.AnimationState.AddAnimation(0, "Move", true, 0.0f);
                }
                else
                {
                    Jicama_Skeleton.AnimationState.AddAnimation(0, "Idle", true, 0.0f);
                }
                break;
        }
    }
    public void Burrow()
    {
        Jicama_Skeleton.AnimationState.SetAnimation(0, "Burrow_and_New_Pot", false);
        if (Moving)
        {
            Jicama_Skeleton.AnimationState.AddAnimation(0, "Move", true, 0.0f);
        }
        else
        {
            Jicama_Skeleton.AnimationState.AddAnimation(0, "Idle", true, 0.0f);
        }
    }
    public void Hurt()
    {
        hurtTrackEntry = Jicama_Skeleton.AnimationState.SetAnimation(2, "Hurt", false); //"Dash_Backward" also exists.
        hurtTrackEntry.Complete += delegate //To run what's inside the brackets when the last queued animation finishes.
        {
             Jicama_Skeleton.AnimationState.SetEmptyAnimation(2, 0.25f);
            hurtTrackEntry = null;
        };
    }
    public void Grab()
    {
        if(!Grabbing)
        {
            Jicama_Skeleton.AnimationState.SetAnimation(4, "Grab", false);
            Jicama_Skeleton.AnimationState.AddAnimation(4, "Grab_Idle", true, 0.0f);
            Grabbing = true;
        }
        else
        {
            attackTrackEntry = Jicama_Skeleton.AnimationState.SetAnimation(4, "Toss", false);
            attackTrackEntry.Complete += delegate //To run what's inside the brackets when the last queued animation finishes.
            {
                Jicama_Skeleton.AnimationState.SetEmptyAnimation(4, 0.25f);
                attackTrackEntry = null;
            };
            Grabbing = false;
        }
    }
    public void Move()
    {
        if(Moving)
        {
            Jicama_Skeleton.AnimationState.SetAnimation(0, "Idle", true);
            Moving = false;
        }
        else
        {
            Jicama_Skeleton.AnimationState.SetAnimation(0, "Move", true);
            Moving = true;
        }
    }
    public void Turret()
    {
        if(!Shooting)
        {
            Shooting = true;
            Jicama_Skeleton.AnimationState.SetAnimation(5, "Turret_On", false);
            Jicama_Skeleton.AnimationState.AddAnimation(5, "Turret_Shoot", true, 0.0f);
            aimTrackEntry = Jicama_Skeleton.AnimationState.AddAnimation(6, "Turret_Aim_Full_Down", true, 0.0f);
            aimTrackEntry.MixBlend = MixBlend.Replace;
            aimTrackEntry.Alpha = Aim_Fully_Down;
        }
        else
        {
            Shooting = false;
            shootingTrackEntry = Jicama_Skeleton.AnimationState.SetAnimation(5, "Turret_Off", false);
            shootingTrackEntry.Complete += delegate //To run what's inside the brackets when the last queued animation finishes.
            {
                Jicama_Skeleton.AnimationState.SetEmptyAnimation(5, 0.25f);
                Jicama_Skeleton.AnimationState.SetEmptyAnimation(6, 0.25f);
                shootingTrackEntry = null;
                aimTrackEntry = null;
            };
        }
    }
    public void Dash()
    {
        dashTrackEntry = Jicama_Skeleton.AnimationState.SetAnimation(1, "Dash_Forward", false); //"Dash_Backward" also exists.
        dashTrackEntry.Complete += delegate //To run what's inside the brackets when the last queued animation finishes.
        {
            Jicama_Skeleton.AnimationState.SetEmptyAnimation(1, 0.25f);
            dashTrackEntry = null;
        };
        if (Moving)
        {
            Jicama_Skeleton.AnimationState.AddAnimation(0, "Move", true, 0.0f);
        }
        else
        {
            Jicama_Skeleton.AnimationState.AddAnimation(0, "Idle", true, 0.0f);
        }
    }
    public void Fall()
    {
        if (Jump_state == 2)
        {
            Jicama_Skeleton.AnimationState.SetAnimation(0, "Land", false);
            Jicama_Skeleton.AnimationState.AddAnimation(0, "Idle", true, 0.0f);
        }
        else if (Jump_state != 0)
        {
            Jicama_Skeleton.AnimationState.SetAnimation(0, "Airborne_Down", true);
            Jump_state = 2;
        }
    }    

}
