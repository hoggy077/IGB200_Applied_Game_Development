using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumsAndDictionaries;
using static SpellFunctionLibrary;
using static SoundManager;
using System;

public class OrbScript : MonoBehaviour, iPhysicsInterface
{
    public float baseDamage = 1f;
    public Elements element = Elements.NULL;

    private Animator Anim_ { get => GetComponent<Animator>(); }
    public Rigidbody2D RB_ => GetComponent<Rigidbody2D>();
    public float Deceleration_ { get => 1f; set => _ = value; }
    public Properties[] EntityProperties_ { get => new Properties[] { Properties.Light }; set => _ = value; }
    public EntityTypes EntityType_ => EntityTypes.Object;
    private float radius = 2.5f;
    private float duration = 5f;
    public bool targetsPlayer = false;

    // Start is called before the first frame update
    void Start() {
        Anim_.speed = 1f;
        gameObject.layer = 6;
    }
    public void AlertObservers(AnimationEvents message) {
        if (message == AnimationEvents.Start) {
            Anim_.SetTrigger("start");
            Anim_.speed = 1 / duration;
        } else if (message == AnimationEvents.Explode) {
            Anim_.SetTrigger("explode");
            Anim_.speed = 1;
            
        } else if (message == AnimationEvents.Death) {
            Explode();
        }
    }

    private void Explode() {
        if (element == Elements.Push || element == Elements.Pull) {
            if (targetsPlayer) {
                ExplodePlayer();
            } else {
                ExplodePhysics();
            }
        } else {
            ExplodeDamage();
        }
        Destroy(this.gameObject);
    }

    private void ExplodePlayer() {
        float DistanceToPlayer = Vector2.Distance(transform.position, PlayerEntity.Instance.transform.position);
        Vector2 direction = transform.position - PlayerEntity.Instance.transform.position;

        if (PlayerEntity.Instance && DistanceToPlayer < 2 * 2 * radius) {
            if (element == Elements.Push) {
                PlayerEntity.Instance.UpdateForce(3 * baseDamage, direction, element);
            } else {
                PlayerEntity.Instance.StartCoroutine(LerpSelf(PlayerEntity.Instance.gameObject, transform.position, 1f));
            }            
        }
    }

    private void ExplodePhysics() {
        List<GameObject> collider2Ds = new List<GameObject>();
        int layerMask = 1 << 5;
        layerMask = ~layerMask;
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, radius, layerMask)) {
            if (collider.TryGetComponent(out iPhysicsInterface iPhysics_) && !collider2Ds.Contains(collider.gameObject)) {
                collider2Ds.Add(collider.gameObject);
                Vector2 direction = transform.position - collider.transform.position;
                iPhysics_.UpdateForce(3 * baseDamage, -direction, element);
            }
        }
    }
    private void ExplodeDamage() {
        List<GameObject> collider2Ds = new List<GameObject>();
        int layerMask = 1 << 5;
        layerMask = ~layerMask;
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, radius, layerMask)) {
            if (collider.TryGetComponent(out iDamageInterface health_) && !collider2Ds.Contains(collider.gameObject)) {
                collider2Ds.Add(collider.gameObject);
                health_.TakeDamage(baseDamage, element, SpellTemplates.Orb);
            }
        }
    }
    public void UpdateForce(float magnitude, Vector3 direction, Elements elementType) {
        if(elementType == Elements.Pull) {
            magnitude *= -1f;
        }
        if (EntityProperties_.Contains(Properties.Immovable)) {
            return;
        }
        gameObject.layer = 6;
        Instantiate(SoundDict[elementType.ToString() + "Sound"]);
        RB_.AddForce(magnitude * RB_.mass * direction, ForceMode2D.Impulse);
        Debug.Log(magnitude * RB_.mass * direction);
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
}