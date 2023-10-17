using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumsAndDictionaries;

public class RangedEnemy : AbstractEnemy
{
    private Vector3 lastAttackDir = Vector3.zero;
    public override Vector3 CalculateFacing() {
        return lastAttackDir;
    }

    public GameObject Projectile;
    public float ProjectileSpeed = 3f;  
    public void Attack(Vector2 Direction) {
        AttackTime_ = AttackDelay;
        UpdateAnimation(Direction);
        Anim_.SetTrigger("attack");
        Vector3 Dir3 = Direction.normalized;
        GameObject newProjectile = Instantiate(Projectile, transform.position + Dir3, transform.rotation);
        ProjectileScript projectile = newProjectile.AddComponent<ProjectileScript>();
        projectile.Shooter = this.gameObject;
        projectile.Velocity = ProjectileSpeed;
        projectile.Direction = Direction;
        projectile.Damage = EntityDamage_;
        projectile.element = DamageType;
        projectile.StartProj();
        lastAttackDir = Direction;
    }

    public float Range = 20f;
    public new void  Update() {     

        if (Vector3.Distance(transform.position, PlayerEntity.Instance.transform.position) < Range && AttackTime_ < Time.timeSinceLevelLoad && !isFrozen_) {
            RaycastHit2D[] hit2d_a = Physics2D.RaycastAll(transform.position, PlayerEntity.Instance.transform.position - transform.position, Range);
            //Debug.DrawRay(gameObject.transform.position, PlayerRef.transform.position - gameObject.transform.position);
            hit2d_a = Array.FindAll(hit2d_a, (h) => { return h.collider.gameObject.layer == LayerMask.NameToLayer("WALL") || h.collider.gameObject == PlayerEntity.Instance.gameObject; });
            Array.Sort<RaycastHit2D>(hit2d_a, (ray1, ray2) => { return ray1.distance.CompareTo(ray2.distance); });

            for (int i = 0; i < hit2d_a.Length; i++) {
                //Debug.Log($"{hit2d_a[i].collider.name}");

                if (hit2d_a[i].collider.gameObject.layer == LayerMask.NameToLayer("WALL")) {
                    Debug.Log("Wall first");
                    break;
                } else if (hit2d_a[i].collider.gameObject == PlayerEntity.Instance.gameObject) {
                    //Debug.Log("Player");
                    if(AttackTime_ < Time.timeSinceLevelLoad) {
                        Attack(PlayerEntity.Instance.transform.position - transform.position);
                    }
                } else if (hit2d_a[i].collider.gameObject == gameObject) {
                    //this is just to ignore self
                }
            }
        }
    }
}
