using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsAndDictionaries;

public interface iPropertyInterface {
    public Elements[] ElementImmunities_ { get; set; }
    public Properties[] EntityProperties_ { get; set; }
    public EntityTypes EntityType_ { get;}
}
public interface iDamageInterface
{
    public void TakeDamage(float damage, Elements damageType, SpellTemplates damageSource = SpellTemplates.NULL);
}
public interface iHealthInterface : iDamageInterface
{
    public int Health_ { get; set; }
    public int MaxHealth_ { get; set; }        
    public void EntityDeath();
}

public interface iPhysicsInterface {
    public Rigidbody2D RB_ { get;}
    public float Deceleration_ { get; set; }
    public void UpdateVelocity(float magnitude, Vector3 direction);
    public void UpdateForce(float magnitude, Vector3 direction, Elements elementType);
    public void Decelerate();
    public void UpdateSorting();
}

public interface iCreatureInterface : iAnimationInterface {
    public float EntitySpeed_ { get; set; }
    public void UpdateDirection(Vector3 change);
}

public interface iAnimationInterface
{
    public void AlertObservers(AnimationEvents message);
    Animator Anim_ { get; }
    public void UpdateAnimation(Vector3 change);

}

public interface iFacingInterface {
    Directions CurrentDirection_ { get; set; }
    public Directions GetEntityDirectionEnum();
    public int GetEntityFacing();
    public Vector2 GetEntityDirection();
}

public interface iEnemyInterface {
    public float EntityDamage_ { get; set; }
}

public interface iPropertyManager {
    public void AddProperty(Properties property);
    public void RemovePropery(Properties property);
    public void AddProperty(Properties property, float duration);
    public void AddImmunity(Elements element);
    public void RemoveImmunity(Elements property);
    public void AddImmunity(Elements element, float duration);
}

public interface iReloadInterface
{
    public bool isException_{ get; }
}
