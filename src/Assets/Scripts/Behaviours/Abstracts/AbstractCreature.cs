using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using static EnumsAndDictionaries;
using static SoundManager;
using static SpellFunctionLibrary;
public abstract class AbstractCreature : MonoBehaviour, iHealthInterface, iCreatureInterface, iPhysicsInterface, iPropertyInterface,iPropertyManager, iFacingInterface, iReloadInterface
{
    public abstract bool IsEnemy { get; }
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
    public Animator Anim_ { get => GetComponent<Animator>();}
    public Directions CurrentDirection_ { get => CurrentDirection; set => CurrentDirection = value; }
    private Directions CurrentDirection;
    public Rigidbody2D RB_ { get => GetComponent<Rigidbody2D>();}
    public float Deceleration_ { get => Deceleration; set => Deceleration = value; }
    private float Deceleration = 1;
    private Collider2D PhysicsCollider {
        get {
            Collider2D[] AllColliders = GetComponents<Collider2D>();
            foreach (Collider2D collider in AllColliders) {
                if (!collider.isTrigger) {
                    return collider;
                }
            }
            return GetComponent<Collider2D>();
        }
    }

    public abstract bool isException_ { get; }

    protected Material DefaultMat;
    private void FixedUpdate() {
        Decelerate();
        UpdateSorting();
    }
    private void Awake() {
        GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
    }
    public Vector2 GetEntityDirection() {
        return VectorDict[CurrentDirection_];
    }
    public Directions GetEntityDirectionEnum() {
        return CurrentDirection_;
    }
    public int GetEntityFacing() {
        return IntDict[CurrentDirection_];
    }
    IEnumerator SpriteRoutine = null;
    internal bool isFrozen_ = false;
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
        if(elementType == Elements.Life) {
            damageInt *= -1;
        }
        //Instantiate Damage Sound        
        Instantiate(SoundDict[SoundName]);
        //Create Damage Effect
        SpellRenderer.Instance.CreateBurstFX(transform.position, ColourDict[elementType]);
        // Discolour Sprite Freeze
        if(SpriteRoutine != null) { StopCoroutine(SpriteRoutine); }

        if (elementType.Equals(Elements.Ice)) {
            //Debug.Log($"{transform.name} is frozen!");
            SpriteRoutine = TintSprite(2.5f, Color.cyan);
            StartCoroutine(SpriteRoutine);
            StartCoroutine(MovePause(2.5f));
        } else {
            if (damageInt > 0) {
                //Debug.Log($"{transform.name} takes {damageInt} {elementType} damage!");
                SpriteRoutine =  TintSprite(0.1f, Color.red);
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
            DebugBox.Instance.inputs.Add($"Object.Destroy({transform.name.Replace("(Clone)","")});");
        }
    }
    public abstract void UpdateAnimation(Vector3 change);
    public void UpdateDirection(Vector3 change) {
        if (change != Vector3.zero) {
            CurrentDirection_ = VectorToDirection(change);
        }
    }
    public void UpdateSorting() {
        if (PhysicsCollider) {
            GetComponent<Renderer>().sortingOrder = -Mathf.RoundToInt(PhysicsCollider.bounds.center.y);
        } else {
            GetComponent<Renderer>().sortingOrder = -Mathf.RoundToInt(transform.position.y);
        }
    }
    public abstract void  UpdateVelocity(float magnitude, Vector3 direction);
    public void UpdateForce(float magnitude, Vector3 direction, Elements elementType) {
        magnitude = ComputeValue(elementType, EntityProperties_);
        if (EntityProperties_.Contains(Properties.Immovable)) return;
        if (elementType == Elements.Pull) magnitude *= -1f;
        Instantiate(SoundDict[elementType.ToString() + "Damage"]);
        Vector2 Velocity = 2 * magnitude * RB_.mass * direction.normalized;
        StartCoroutine(CheckVelocityCanBridgeGaps(gameObject, Velocity));
        
    }
    public void AlertObservers(AnimationEvents message) {
        Debug.Log(transform.name + " recieved message " + message);
        EdgeChecks = new bool[EdgeChecks.Length];
        if (message.Equals(AnimationEvents.Death)) {
            EntityDeath();
            hasFallen = false;
        } else if (message.Equals(AnimationEvents.Fall)){
            EntityFall();
            hasFallen = false;
        }
    }
    private void OnCollisionStay2D(Collision2D collision) {
        EnterVoid(collision);
    }
    internal bool[] EdgeChecks = new bool[4];
    internal bool hasFallen = false;

    internal void EnterVoid(Collision2D collision) {
        if ((collision.gameObject.TryGetComponent(out EmptySpaceScript _)/* || collision.gameObject.layer == 8*/) && gameObject.layer == 7) {
            //check NE corner
            Vector2 NE = PhysicsCollider.bounds.max;
            if (CheckIfContained(NE, collision.collider)) EdgeChecks[0] = true;
            //check NW corner
            Vector2 NW = new Vector2(PhysicsCollider.bounds.min.x, PhysicsCollider.bounds.max.y);
            if (CheckIfContained(NW, collision.collider)) EdgeChecks[1] = true;
            //check SE corner
            Vector2 SE = new Vector2(PhysicsCollider.bounds.max.x, PhysicsCollider.bounds.min.y);
            if (CheckIfContained(SE, collision.collider)) EdgeChecks[2] = true;
            //check SW corner
            Vector2 SW = GetComponent<Collider2D>().bounds.min;
            if (CheckIfContained(SW, collision.collider)) EdgeChecks[3] = true;

            int i = 0;
            foreach (bool boo in EdgeChecks) { if (boo) { i++; } }
            if (i > 2 && !hasFallen) { Anim_.SetTrigger("fall"); EntitySpeed_ = 0; hasFallen = true; }

            //string msg = "";
            //foreach (bool boo in EdgeChecks) msg += boo + " ";
            //Debug.Log(msg + transform.name);
            //if (!EdgeChecks.Contains(false)) Debug.LogWarning(transform.name + " has fallen into the River");
            //Debug.Log($"{NE} {NW} {SE} {SW}");
        }
    }
    internal void LeaveVoid(Collision2D collision) {
        if ((collision.gameObject.TryGetComponent(out EmptySpaceScript _)/* || collision.gameObject.layer == 8*/) && gameObject.layer == 7) {
            //check NE corner
            Vector2 NE = PhysicsCollider.bounds.max;
            if (!CheckIfContained(NE, collision.collider)) EdgeChecks[0] = false;
            //check NW corner
            Vector2 NW = new Vector2(PhysicsCollider.bounds.min.x, PhysicsCollider.bounds.max.y);
            if (!CheckIfContained(NW, collision.collider)) EdgeChecks[1] = false;
            //check SE corner
            Vector2 SE = new Vector2(PhysicsCollider.bounds.max.x, PhysicsCollider.bounds.min.y);
            if (!CheckIfContained(SE, collision.collider)) EdgeChecks[2] = false;
            //check SW corner
            Vector2 SW = GetComponent<Collider2D>().bounds.min;
            if (!CheckIfContained(SW, collision.collider)) EdgeChecks[3] = false;
        }
    }
    private bool CheckIfContained(Vector2 vector2, Collider2D collider) {
        bool XBounds = vector2.x > collider.bounds.min.x && vector2.x < collider.bounds.max.x;
        bool YBounds = vector2.y > collider.bounds.min.y && vector2.y < collider.bounds.max.y;
        return XBounds && YBounds;
    }
    protected abstract void EntityFall();
    public abstract void Decelerate();
    public abstract void EntityDeath();

    #region Property Management
    public void AddProperty(Properties property) {
        if (!EntityProperties_.Contains(property)) {
            //Add Property
            Array.Resize(ref EntityProperties, EntityProperties_.Length + 1);
            EntityProperties_[EntityProperties_.Length - 1] = property;
        }
    }
    public void RemovePropery(Properties property) {
        if (EntityProperties_.Contains(property)) {
            int index = Array.FindIndex(EntityProperties, 0, EntityProperties_.Length, EntityProperties_.Contains);
            for (; index < EntityProperties_.Length - 1; index++) {
                EntityProperties_[index] = EntityProperties_[index + 1];
            }
            Array.Resize(ref EntityProperties, EntityProperties_.Length - 1);
        }
    }
    public void AddProperty(Properties property, float duration) {
        if (!EntityProperties_.Contains(property)) {
            StartCoroutine(AddPropertyForDuration(property, duration));
        }
    }
    private IEnumerator AddPropertyForDuration(Properties property, float duration) {
        float t = 0;
        Array.Resize(ref EntityProperties, EntityProperties_.Length + 1);
        EntityProperties_[EntityProperties_.Length - 1] = property;

        while (t < duration) {
            t += Time.deltaTime;
            yield return null;
        }
        Array.Resize(ref EntityProperties, EntityProperties_.Length - 1);
    }
    public void AddImmunity(Elements element) {
        if (!ElementImmunities_.Contains(element)) {
            //Add Property
            Array.Resize(ref DamageImmunities, ElementImmunities_.Length + 1);
            ElementImmunities_[ElementImmunities_.Length - 1] = element;
        }
    }
    public void RemoveImmunity(Elements property) {
        if (ElementImmunities_.Contains(property)) {
            int index = Array.FindIndex(DamageImmunities, 0, ElementImmunities_.Length, ElementImmunities_.Contains);
            for (; index < ElementImmunities_.Length - 1; index++) {
                ElementImmunities_[index] = ElementImmunities_[index + 1];
            }
            Array.Resize(ref EntityProperties, EntityProperties_.Length - 1);
        }
    }
    public void AddImmunity(Elements element, float duration) {
        if (!ElementImmunities_.Contains(element)) {
            StartCoroutine(AddImmunityForDuration(element, duration));
        }
    }
    private IEnumerator AddImmunityForDuration(Elements element, float duration) {
        float t = 0;
        Array.Resize(ref DamageImmunities, ElementImmunities_.Length + 1);
        ElementImmunities_[ElementImmunities_.Length - 1] = element;

        while (t < duration) {
            t += Time.deltaTime;
            yield return null;
        }
        Array.Resize(ref DamageImmunities, ElementImmunities_.Length - 1);
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
    #endregion
}