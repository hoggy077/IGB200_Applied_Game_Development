using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumsAndDictionaries;
using static SpriteManager;
using static SoundManager;

public abstract class AbstractDoor : MonoBehaviour, iHealthInterface, iReloadInterface
{
    public GameObject walkThroughSoundEffect;
    //public int triggerID;
    public bool isSolveTrigger;
    public bool isException_ => isException;
    public bool isException;
    private bool firstEnter = true;

    public AbstractDoor ExitDoor;
    [Range(-1, 10)]
    public int sceneIndex = -1;
    protected bool isInvulnerable;
    private float DelayTimer_ {get => delayTimer; set => delayTimer = Time.timeSinceLevelLoad + value; }
    private float delayTimer;
    public bool IsOpen { get; set; }
    public Properties[] EntityProperties_ { get => EntityProperties; set => EntityProperties = value; }
    private Properties[] EntityProperties = new Properties[0];
    public EntityTypes EntityType_ { get => EntityTypes.Object; }
    public Directions CurrentDirection_ { get => CurrentDirection; set => CurrentDirection = value; }
    public Directions CurrentDirection;
    public int Health_ { get => Health; set => Health = value; }
    private int Health;
    public int MaxHealth_ { get => MaxHealth; set => MaxHealth = value; }
    private int MaxHealth;
    public Elements[] ElementImmunities_ { get => null; set => _ = value; }
    private void Awake() {
        ValidateFunction();
    }
    public RoomData RoomData_ { get => RoomData; set => RoomData = value; }
    private RoomData RoomData;
    private void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log(collision.transform.name + " entered");
        if (collision.gameObject.TryGetComponent(out iFacingInterface em) && !collision.isTrigger) {
            if (em.GetEntityDirectionEnum() == CurrentDirection_ && IsOpen) {
                if (sceneIndex < 0 && DelayTimer_ < Time.timeSinceLevelLoad) {
                    DelayTimer_ = 2 * Time.deltaTime;
                    Vector3 offset = VectorDict[CurrentDirection_];
                    collision.gameObject.transform.position = ExitDoor.transform.position + offset;
                    Instantiate(walkThroughSoundEffect);

                    //New Room Resets
                    if (collision.TryGetComponent(out PlayerEntity playerEntity) && !isException && RoomData_) {
                        playerEntity.LastDoor_ = this.ExitDoor;
                        if (isSolveTrigger) {
                            RoomData_.isSolved_ = true;
                            if (firstEnter == true) { Instantiate(SoundDict["PuzzleSolveSound"]); firstEnter = false; }
                        }
                        if (ExitDoor.RoomData_) {
                            if (!ExitDoor.RoomData_.isSolved_) {
                                ExitDoor.RoomData_.Load();
                            }
                        }
                        if (!RoomData_.isSolved_) {
                            RoomData_.Unload();
                        }
                    }
                } else {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
                }
            }
        }
    }
    public Vector3 RespawnPoint {get => transform.position - (Vector3) VectorDict[CurrentDirection_]; }

    public void InitExitDoor() {
        if (!ExitDoor)
            return;
        if (IsOpen) {
            ExitDoor.IsOpen = IsOpen;
        }
        if (isInvulnerable) {
            ExitDoor.isInvulnerable = isInvulnerable;
        }
        ExitDoor.UpdateSprite();
    }
    public void SyncExitDoor() {
        if (!ExitDoor)
            return;
        ExitDoor.IsOpen = IsOpen;
        ExitDoor.isInvulnerable = isInvulnerable;
        ExitDoor.UpdateSprite();
    }
    public void OpenCloseDoor(bool newState) {
        if (IsOpen != newState) 
        { 
            if (ExitDoor.IsOpen == false && isInvulnerable) 
            { 
                DebugBox.Instance.inputs.Add("Object.isOpen(metalDoor);");
                Instantiate(SoundDict["DoorOpenSound"]);
            }
            else if (ExitDoor.IsOpen == false) 
            { 
                DebugBox.Instance.inputs.Add("Object.isOpen(woodDoor);");
                Instantiate(SoundDict["FireDamage"]);
            }
        }
        IsOpen = newState;
        UpdateSprite();
        SyncExitDoor();
    }
    protected Sprite currentSprite;
    public void UpdateSprite() {
        try {
            GetComponent<SpriteRenderer>().sortingOrder = - (int)transform.position.y;
            GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            GetComponent<Collider2D>().isTrigger = true;
            if (IsOpen || ExitDoor.IsOpen) {
                GetComponent<SpriteRenderer>().sprite = SpriteDict["OpenDoor"][IntDict[CurrentDirection_]];
            } else if (isInvulnerable || ExitDoor.isInvulnerable) {
                GetComponent<SpriteRenderer>().sprite = SpriteDict["MetalDoor"][IntDict[CurrentDirection_]];
            } else {
                GetComponent<SpriteRenderer>().sprite = SpriteDict["WoodDoor"][IntDict[CurrentDirection_]];
            }
            if (TryGetComponent(out BoxCollider2D boxCollider2D)) {
                Vector2 SpriteSize = GetComponent<SpriteRenderer>().bounds.size;
                Vector2 Mag = new Vector2(Mathf.Abs(VectorDict[CurrentDirection_].x), Mathf.Abs(VectorDict[CurrentDirection_].y));
                boxCollider2D.size = 0.5f * Mag * SpriteSize;
                boxCollider2D.size += 1.0f * SpriteSize;
            }
        }
        catch (System.Exception) {
            //Debug.LogWarning(ex.Message);
        }
    }
    public void TakeDamage(float damage, Elements elementType, SpellTemplates damageSource = SpellTemplates.NULL) {
        if (isInvulnerable || elementType != Elements.Fire) {
            return;
        }
        Health_ -= Mathf.RoundToInt(damage);
        Health_ = Mathf.Clamp(Health_, 0, MaxHealth_);
        SpellRenderer.Instance.CreateBurstFX(transform.position, ColourDict[elementType]);
        if (0 >= Health_) {
            EntityDeath();
        }
    }
    public void EntityDeath() {
        OpenCloseDoor(true);
        // TODO: Impliment Colour change
    }

    public abstract void ValidateFunction();
}