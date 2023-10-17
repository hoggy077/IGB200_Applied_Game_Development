using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumsAndDictionaries;
public abstract class AbstractEnemy : AbstractCreature, iEnemyInterface
{
    public override bool isException_ => false;
    public static float AttackDelay = 3;
    public float EntityDamage;
    public Elements DamageType = Elements.NULL;
    public float EntityDamage_ { get => EntityDamage; set => EntityDamage = value; }
    public float AttackTime_ { get { return AttackTime; } set { AttackTime = Time.timeSinceLevelLoad + value; } }//Automatically update the cast time to the new time
    private float AttackTime = 0;
    public override bool IsEnemy => true;
    public void Start() {
        AttackTime_ = AttackDelay;
        DefaultMat = GetComponent<SpriteRenderer>().material;
        Health_ = MaxHealth_;
    }
    public void Update() {
        Vector3 change = CalculateFacing();
        UpdateAnimation(change);
    }
    public abstract Vector3 CalculateFacing();
    public override void Decelerate() {
        if (RB_.velocity != Vector2.zero && gameObject.layer != 6) {
            RB_.velocity *= 0.1f;
        }
    }
    public override void EntityDeath() {
        Debug.Log(transform.name + " Died");
        Instantiate(SoundManager.SoundDict["EnemyDeathSound"]);
        Destroy(this.gameObject);
    }        
    protected override void EntityFall() {
        Debug.Log(transform.name + " Fell");
        EntityDeath();
    }
    public override void UpdateAnimation(Vector3 change) {
        if (change != Vector3.zero) {
            Anim_.SetFloat("moveX", change.x);
            Anim_.SetFloat("moveY", change.y);
            Anim_.SetBool("moving", true);
        } else {
            Anim_.SetBool("moving", false);
        }
    }
    public override void UpdateVelocity(float magnitude, Vector3 direction) {
        RB_.velocity = magnitude * direction;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        EnterVoid(collision);
    }
    private void OnCollisionExit2D(Collision2D collision) {
        //LeaveVoid(collision);
    }
}
