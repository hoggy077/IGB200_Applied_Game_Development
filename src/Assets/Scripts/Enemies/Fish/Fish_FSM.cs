using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Fish_FSM : AbstractEnemy
{
    private GameObject this_gm_obj { get => gameObject; }
    private GameObject PlayerRef { get => GameObject.FindGameObjectWithTag("Player"); }

    public float DetectionRange = 5;
    private int NumShotsFired = 0;
    public int MaxShots_B4_Hide = 3;
    public float Delay_Between_shots = 1.5f;
    public float HideTime = 4;
    public bool Hiding = false;

    private float time { get { return time_; } set { time_ = Time.timeSinceLevelLoad + value; } }
    private float time_;

    private FSM_Struct MyMachine = new FSM_Struct();
    private void Define_states()
    {
        MyMachine.States = new Dictionary<int, FSM_States>()
        {
            {
                0, new FSM_States()
                {
                    Description = "Fish Fire condition",
                    StateLogic = ShootAt(),
                    StatePredicate = (fsmd) => { return fsmd.Target != null && fsmd.Distance < DetectionRange; }
                }
            },
            {
                1, new FSM_States()
                {
                    Description = "Idle state (should be last as a final condition)",
                    StateLogic = Idle(),
                    StatePredicate = (fsmd) => { return true; }
                }
            }
        };
    }


    bool HitPlayerFirst()
    {
        RaycastHit2D[] hit2d_a = Physics2D.RaycastAll(gameObject.transform.position, PlayerRef.transform.position - gameObject.transform.position, DetectionRange);
        hit2d_a = Array.FindAll(hit2d_a, (h) => { return h.collider.gameObject.layer == LayerMask.NameToLayer("WALL") || h.collider.gameObject == PlayerRef; });
        Array.Sort<RaycastHit2D>(hit2d_a, (ray1, ray2) => { return ray1.distance.CompareTo(ray2.distance); });

        RaycastHit2D hit2d = new RaycastHit2D();
        for (int i = 0; i < hit2d_a.Length; i++)
        {
            if (hit2d_a[i].collider.gameObject.layer == LayerMask.NameToLayer("WALL"))
                return false;
            else if (hit2d_a[i].collider.gameObject == PlayerRef)
            {
                hit2d = hit2d_a[i];
                break;
            }
            else if (hit2d_a[i].collider.gameObject == gameObject)
                continue;
        }

        if (hit2d.collider != null)
            return true;
        return false;
    }

    private FSM_Datapass fsmdp = new FSM_Datapass();


    bool InView { get => HitPlayerFirst(); }
    bool wasInView = false;


    IEnumerator Idle()
    {
        NumShotsFired = 0;
        Hiding = false;

        while (true)
        {
            if (InView)
            {
                fsmdp.Target = PlayerRef;
                fsmdp.UsePos = false;
                wasInView = true;
            }
            else if (wasInView)
            {
                fsmdp.Target = null;
                fsmdp.UsePos = false;
                wasInView = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }


    public GameObject Projectile;
    public float ProjectileSpeed = 3f;

    IEnumerator ShootAt()
    {
        while (true)
        {
            if (fsmdp.Target != null && !Hiding)//dont need this bc the run condition requires the player, and the idle state only supplies the player if in view, but better safe than sorry
            {

                Vector3 dir = (fsmdp.Target.transform.position - transform.position);

                UpdateAnimation(dir);

                GameObject proj = Instantiate<GameObject>(Projectile, transform.position + dir.normalized, transform.rotation);
                ProjectileScript projectile = proj.AddComponent<ProjectileScript>();
                projectile.Shooter = this.gameObject;
                projectile.Velocity = ProjectileSpeed;
                projectile.Direction = dir;
                projectile.Damage = EntityDamage_;
                projectile.element = DamageType;
                projectile.StartProj();
                NumShotsFired++;
                Hiding = NumShotsFired >= 3;

                yield return new WaitForSecondsRealtime(Delay_Between_shots);
            }
            else if (Hiding)
            {
                //fish hide logic
                Debug.Log("fuckin hell");
                yield return new WaitForSecondsRealtime(HideTime);
                //fish popup logic
                Debug.Log("fuckin pog");

                NumShotsFired = 0;
                Hiding = false;
            }
            
            yield return new WaitForEndOfFrame();

        }
    }

    void Start()
    {
        Define_states();
        fsmdp = new FSM_Datapass()
        {
            Caller = this_gm_obj,
            Target = null,
            UsePos = false
        };
    }

    void LateUpdate()
    {
        int re = MyMachine.UpdateState(fsmdp);
        MyMachine.RunState(this);
        //Debug.Log(fsmdp.Target != null && fsmdp.Distance < DetectionRange && Hiding == false);
        //Debug.Log(MyMachine.getState().Description);
        //Debug.Log(Hiding);
    }

    Vector3 lastFacing = Vector3.right;
    public override Vector3 CalculateFacing()
    {
        Vector3 n = (PlayerRef.transform.position - this_gm_obj.transform.position).normalized;
        if (Math.Abs(n.y) < Math.Abs(n.x))
        {
            lastFacing = n.y < 0 ? Vector3.up : Vector3.down;
            return lastFacing;
        }
        else if (Math.Abs(n.x) < Math.Abs(n.y))
        {
            lastFacing = n.x < 0 ? Vector3.left : Vector3.right;
            return lastFacing;
        }
        else
            return lastFacing;
    }
}
