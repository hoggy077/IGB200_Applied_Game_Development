using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsAndDictionaries;

public class SwitchObject : MonoBehaviour, iSenderObject, iHealthInterface
{
    public bool currentState_ {
        get { return currentState; }
        set { currentState = value; UpdateSprite(); UpdateReciever(); }
    }
    public bool currentState;
    public List<iRecieverObject> targetObjects_ { get => targetObjects; set => targetObjects = value; }
    private List<iRecieverObject> targetObjects = new List<iRecieverObject>();
    public Properties[] EntityProperties_ { get => EntityProperties; set => EntityProperties = value; }
    private Properties[] EntityProperties;
    public EntityTypes EntityType_ { get => EntityType; set => EntityType = value; }
    private EntityTypes EntityType;
    public Directions CurrentDirection_ { get => CurrentDirection; set => CurrentDirection = value; }
    private Directions CurrentDirection;
    public int Health_ { get => Health; set => Health = value; }
    private int Health;
    public int MaxHealth_ { get => MaxHealth; set => MaxHealth = value; }
    private int MaxHealth = 1;
    public Elements[] ElementImmunities_ { get => new Elements[0]; set => _ = value; }
    public Elements activeElement = Elements.Electricity; public Elements inactiveElement = Elements.NULL;

    // Start is called before the first frame update
    private void Start() {
        Health_ = MaxHealth_;
        EntityProperties_ = new Properties[] { Properties.Immovable };
        currentState_ = false;
        UpdateSprite();    
    }
    public void UpdateReciever() {
        foreach (iRecieverObject target in targetObjects_) {
            target.CheckSenders(this);
        }
    }
    private void UpdateSprite() {
        if (!currentState_) {
            GetComponent<SpriteRenderer>().material = SpellRenderer.Instance.CreateMaterial(ColourDict[activeElement]);
        } else {
            GetComponent<SpriteRenderer>().material = SpellRenderer.Instance.CreateMaterial(ColourDict[inactiveElement]);
        }
    }
    public void TakeDamage(float damage, Elements damageType, SpellTemplates damageSource = SpellTemplates.NULL) {
        if(damageType == activeElement && currentState_) {
            EntityDeath();
        } else if (damageType != inactiveElement && !currentState_) {
            EntityDeath();
        }
    }
    public void EntityDeath() {
        currentState_ = !currentState_;
        UpdateSprite();
    }    
    public void ValidateFunction() {
        UpdateSprite();
    }
    public void ResetSender() {
        currentState_ = false;
    }
}
