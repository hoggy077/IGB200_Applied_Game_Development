using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumsAndDictionaries;
using static SpellFunctionLibrary;
using static SoundManager;

public class ProjectileScript : MonoBehaviour , iPhysicsInterface, iDamageInterface
{
    public float Velocity;
    public Vector3 Direction;
    public float Damage;
    public EnumsAndDictionaries.Elements element;
    public GameObject Shooter;

    public Rigidbody2D RB_ => GetComponent<Rigidbody2D>();
    public float Deceleration_ { get => 1; set => _ = value; }
    public Properties[] EntityProperties_ { get => new Properties[] { Properties.Light }; set => _ = value; }
    public EntityTypes EntityType_ => EntityTypes.Object;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer != 2 && collision.gameObject != Shooter) {
            if (collision.transform.TryGetComponent(out iDamageInterface iHealth)) {
                iHealth.TakeDamage(Damage, element);
                Debug.Log("Projectile Hit " + collision.transform.name);
            }
            if(collision.TryGetComponent(out ProjectileScript otherProj)) {
                SpellRenderer.Instance.CreateBurstFX(transform.position, ColourDict[element]);
            }
            Destroy(this.gameObject);
        }
    }
    public void StartProj() {
        gameObject.layer = 6;
        CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = 0.5f * (GetComponent<SpriteRenderer>().bounds.size.x + GetComponent<SpriteRenderer>().bounds.size.y) / 2f;
        circleCollider.isTrigger = true;
        RB_.velocity = Velocity * Direction.normalized;
    }
    public void UpdateForce(float magnitude, Vector3 direction, Elements elementType) {
        Shooter = null;
        magnitude = ComputeValue(elementType, EntityProperties_);
        if (elementType == Elements.Pull) magnitude *= -1f;
        Vector2 velocity = 2 * magnitude * direction.normalized;
        Debug.Log(velocity);
        RB_.velocity = velocity;
    }
    public void UpdateVelocity(float magnitude, Vector3 direction) {
        throw new System.NotImplementedException();
    }
    public void Decelerate() {
        throw new System.NotImplementedException();
    }
    public void UpdateSorting() {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage, Elements damageType, SpellTemplates damageSource = SpellTemplates.NULL) {
        SpellRenderer.Instance.CreateBurstFX(transform.position, ColourDict[damageType]);
        Destroy(this.gameObject);
    }
}
