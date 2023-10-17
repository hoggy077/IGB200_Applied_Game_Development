using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumsAndDictionaries;
using static SoundManager;
using static SpellFunctionLibrary;

public abstract class AbstractAquaticCreature : MonoBehaviour, iHealthInterface, iCreatureInterface, iPhysicsInterface, iPropertyInterface, iFacingInterface, iReloadInterface
{
    public bool isException_ => false;
    public int Health_ { get => Health; set { Health = value; } }
    private int Health;
    public int MaxHealth_ { get => MaxHealth; set => MaxHealth = value; }
    public int MaxHealth = 1;
    public Elements[] ElementImmunities_ { get => DamageImmunities; set => DamageImmunities = value; }
    [SerializeField]
    private Elements[] DamageImmunities = new Elements[0];
    public Properties[] EntityProperties_ { get => EntityProperties; set => EntityProperties = value; }
    [SerializeField]
    private Properties[] EntityProperties = new Properties[0];
    public EntityTypes EntityType_ { get => EntityType; set => EntityType = value; }
    private EntityTypes EntityType = EntityTypes.Creature;
    public float EntitySpeed_ { get => EntitySpeed; set => EntitySpeed = value; }
    [SerializeField]
    private float EntitySpeed = 0.5f;
    public Animator Anim_ { get => GetComponent<Animator>(); }
    public Directions CurrentDirection_ { get => CurrentDirection; set => CurrentDirection = value; }
    private Directions CurrentDirection;
    public Rigidbody2D RB_ { get => GetComponent<Rigidbody2D>(); }
    public float Deceleration_ { get => Deceleration; set => Deceleration = value; }
    private float Deceleration = 1;
    private bool isFrozen_;
    private IEnumerator SpriteRoutine;
    private bool hasFallen;
    private bool[] EdgeChecks;
    internal bool isInWater = true;

    public Vector2 GetEntityDirection() {
        return VectorDict[CurrentDirection_];
    }
    public Directions GetEntityDirectionEnum() {
        return CurrentDirection_;
    }
    public int GetEntityFacing() {
        return IntDict[CurrentDirection_];
    }

    public void UpdateVelocity(float magnitude, Vector3 direction) {
        throw new System.NotImplementedException();
    }

    public void UpdateForce(float magnitude, Vector3 direction, Elements elementType) {
        magnitude = ComputeValue(elementType, EntityProperties_);
        if (EntityProperties_.Contains(Properties.Immovable)) return;
        if (elementType == Elements.Pull) magnitude *= -1f;
        Instantiate(SoundDict[elementType.ToString() + "Damage"]);
        Vector2 Velocity = 2 * magnitude * RB_.mass * direction.normalized;
        StartCoroutine(CheckVelocityCanBridgeGaps(gameObject, Velocity));

    }

    public void Decelerate() {
        if (RB_.velocity != Vector2.zero && (gameObject.layer != 6 || gameObject.layer != 2)) {
            RB_.velocity *= 0.1f;
        }
    }

    public void UpdateSorting() {
        GetComponent<Renderer>().sortingOrder = -Mathf.RoundToInt(transform.position.y);
        
    }

    public void UpdateDirection(Vector3 change) {
        if (change != Vector3.zero) {
            CurrentDirection_ = VectorToDirection(change);
        }
    }

    public void AlertObservers(AnimationEvents message) {
        Debug.Log(transform.name + " recieved message " + message);
        if (message.Equals(AnimationEvents.Death)) {
            EntityDeath();
            hasFallen = false;
        } else if (message.Equals(AnimationEvents.Fall)) {
            EntityFall();
            hasFallen = false;
        }
    }

    public void UpdateAnimation(Vector3 change) {
        if (change != Vector3.zero) {
            Anim_.SetFloat("moveX", change.x);
            Anim_.SetFloat("moveY", change.y);
            Anim_.SetBool("moving", true);
        } else {
            Anim_.SetBool("moving", false);
        }
    }

    public void EntityDeath() {
        //Debug.Log(transform.name + " Died");
        Instantiate(SoundManager.SoundDict["EnemyDeathSound"]);
        Destroy(this.gameObject);
    }

    protected void EntityFall() {
        //Debug.Log(transform.name + " Fell");
        EntityDeath();
    }

    public void TakeDamage(float damage, Elements elementType, SpellTemplates damageSource = SpellTemplates.NULL) {
        damage *= ComputeValue(elementType, EntityProperties_);
        int damageInt = Mathf.RoundToInt(damage);
        string SoundName = elementType.ToString() + "Damage";
        //Check if this does damage
        if (ElementImmunities_.Contains(elementType) || damageInt == 0) {
            SoundName = "AttackFail";
            Instantiate(SoundDict[SoundName]);
            return;
        }
        if (elementType == Elements.Life) {
            damageInt *= -1;
        }
        //Instantiate Damage Sound        
        Instantiate(SoundDict[SoundName]);
        //Create Damage Effect
        SpellRenderer.Instance.CreateBurstFX(transform.position, ColourDict[elementType]);
        // Discolour Sprite Freeze
        if (SpriteRoutine != null) { StopCoroutine(SpriteRoutine); }

        if (elementType.Equals(Elements.Ice)) {
            //Debug.Log($"{transform.name} is frozen!");
            SpriteRoutine = TintSprite(2.5f, Color.cyan);
            StartCoroutine(SpriteRoutine);
            StartCoroutine(MovePause(2.5f));
        } else {
            if (damageInt > 0) {
                //Debug.Log($"{transform.name} takes {damageInt} {elementType} damage!");
                SpriteRoutine = TintSprite(0.1f, Color.red);
                StartCoroutine(SpriteRoutine);
            } else if (damageInt < 0) {
                //Debug.Log($"{transform.name} is healed for {-1 * damageInt} points!");
                SpriteRoutine = TintSprite(0.3f, Color.green);
                StartCoroutine(SpriteRoutine);
            }
            //Process Health
            Health_ -= damageInt;
        }
        ValidateHealth();
    }

    internal void ValidateHealth() {
        Health = Mathf.Clamp(Health, 0, MaxHealth_);
        //Check if the entity should die
        if (0 >= Health_) {
            //Debug.Log($"{transform.name} dies!!!");
            EntitySpeed_ = 0;
            Anim_.SetTrigger("death");
            DebugBox.Instance.inputs.Add($"Object.Destroy({transform.name.Replace("(Clone)", "")});");
        }
    }

    public IEnumerator MovePause(float Wait) {
        float SpeedStore = EntitySpeed_;
        float time = 0;
        isFrozen_ = true;
        while (time < Wait) {
            EntitySpeed_ = 0f;
            time += Time.deltaTime;
            yield return null;
        }
        isFrozen_ = false;
        EntitySpeed_ = SpeedStore;
    }

    public IEnumerator TintSprite(float duration, Color color) {
        float time = 0;
        byte Alpha = 192;
        GetComponent<SpriteRenderer>().material = SpriteManager.Instance.CreateTint(color, Alpha);
        while (time < duration) {
            time += Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().material = SpellRenderer.Instance.defaultUnlit;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(!collision.isTrigger && collision.transform.TryGetComponent(out EmptySpaceScript ess)) {
            if(ess.VoidType_ == VoidType.Water) {
                isInWater = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (!collision.isTrigger && collision.transform.TryGetComponent(out EmptySpaceScript ess)) {
            if (ess.VoidType_ == VoidType.Water) {
                isInWater = false;
            }
        }
    }
}
