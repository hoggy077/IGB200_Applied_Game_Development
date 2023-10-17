//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;
//using System;
//using static EnumsAndDictionaries;

//public class EntityManager : MonoBehaviour
//{
//    public Properties[] entityProperties;

//    public EntityTypes entityType;

//    Directions CurrentDirection = Directions.Down;

//    protected Rigidbody2D rb;
//    [HideInInspector]
//    public int health;
//    public int maxHealth;

//    //[SerializeField]
//    public float entitySpeed = 1f;
//    protected Animator anim;
//    public float Deceleration = 5f;

//    // Start is called before the first frame update
//    void Awake() {
//        if (!gameObject.TryGetComponent(out rb) && entityType.Equals(EntityTypes.Creature)) {
//            rb = gameObject.AddComponent<Rigidbody2D>();
//        }
//        if(rb != null){
//            rb.gravityScale = 0;
//            rb.freezeRotation = true;
            
//        }
//        health = maxHealth;
//        gameObject.TryGetComponent(out anim);
//    }

//    Vector2 previousVelocity;
//    private void FixedUpdate() {
//        if (rb) {
//            if(rb.velocity.magnitude <= previousVelocity.magnitude) {
//                rb.velocity = Decelerate(rb.velocity);
//            }
//            previousVelocity = rb.velocity;
//        }
//        //Update 
//        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(-transform.position.y);
//    }

//    private Vector2 Decelerate(Vector2 velocity) {
//        //Debug.Log(vector2);
//        if(velocity == Vector2.zero) {
//            return velocity;
//        }

//        velocity -= Deceleration * Time.deltaTime * velocity;

//        if (velocity.magnitude < 0.25f) {
//            velocity *= 0f;
//        }
//        if (anim && velocity == Vector2.zero) {
//            UpdateAnimation(velocity);
//        }
//        return velocity;
//    }

//    #region Update Variables Externally
//    public void UpdateVelocity(Vector3 change) {
//        if(rb){
//            rb.velocity = change;
//        }
//    }

//    public void UpdateAnimation(Vector3 change) {
//        if (change != Vector3.zero) {
//            anim.SetFloat("moveX", change.x);
//            anim.SetFloat("moveY", change.y);
//            anim.SetBool("moving", true);
//        } else {
//            anim.SetBool("moving", false);
//        }
//    }

//    public void UpdateDirection(Vector3 change) {   //Up ; Right; Down; Left
//        if (change != Vector3.zero) {

//            CurrentDirection = VectorToDirection(change);
//        }
//    }


//#endregion

//    #region Entity Health and Death
//    public void TakeDamage(float damage) {
//        if (entityProperties.Contains(Properties.Indestructable)) {
//            return;
//        }
//        health -= Mathf.RoundToInt(damage);
//        health = Mathf.Clamp(health, 0, maxHealth);
//        if (0 >= health) {
//            EntityDeath();
//        }
//    }

//    public virtual void EntityDeath() {
//        Destroy(this.gameObject, Time.deltaTime);
//        // TODO: Impliment Colour change

//    }
//    #endregion

//    #region Property Management
//    public void AddProperty(Properties property) {
//        if (!entityProperties.Contains(property)) {
//            //Add Property
//            Array.Resize(ref entityProperties, entityProperties.Length + 1);
//            entityProperties[entityProperties.Length - 1] = property;
//        }
//    }

//    public void RemovePropery(Properties property) {
//        if (entityProperties.Contains(property)) {
//            int index = Array.FindIndex(entityProperties, 0, entityProperties.Length, entityProperties.Contains);
//            for( ; index < entityProperties.Length - 1; index++) {
//                entityProperties[index] = entityProperties[index + 1];
//            }
//            Array.Resize(ref entityProperties, entityProperties.Length - 1);
//        }
//    }

//    public void AddProperty(Properties property, float duration) {
//        if (!entityProperties.Contains(property)) {
//            StartCoroutine(AddPropertyForDuration(property, duration));
//        }
//    }

//    private IEnumerator AddPropertyForDuration(Properties property, float duration) {
//        float t = 0;
//        Array.Resize(ref entityProperties, entityProperties.Length + 1);
//        entityProperties[entityProperties.Length - 1] = property;

//        while (t < duration) {
//            t += Time.deltaTime;
//            yield return null;
//        }
//        Array.Resize(ref entityProperties, entityProperties.Length - 1);

//    }
//    #endregion

//    #region Facing Getters
//    public Directions GetEntityDirectionEnum() {
//        return CurrentDirection;
//    }
//    public int GetEntityFacing() {
//        return IntDict[CurrentDirection];
//    }
//    public Vector2 GetEntityDirection() {
//        return VectorDict[CurrentDirection];
//    }
//    #endregion
//}
