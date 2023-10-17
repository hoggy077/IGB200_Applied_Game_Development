using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumsAndDictionaries;
using static SpriteManager;
using static SoundManager;
public class EmptySpaceScript : MonoBehaviour, iDamageInterface
{
    public VoidType VoidType_;

    private string VoidFills = "VoidFills"; // 0 - Block Fill, 1 - Frozen Water
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    public bool isFrozen_ { 
        get { return isFrozen; }
        set { isFrozen = value; ToggleFrozen(value); }
    }
    private bool isFrozen;
    float Duration = 7f;
    public bool isElectric_ { get => isElectric; set => isElectric = value; }
    private bool isElectric;

    private void Start() {
        gameObject.layer = 2;
        if (!TryGetComponent(out rb)) {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Static;
        rb.gravityScale = 0;
        rb.simulated = true;

        if (!TryGetComponent(out sr)) {
            sr = gameObject.AddComponent<SpriteRenderer>();
        }
        sr.sprite = null;
        sr.sortingLayerID = 0;
        sr.material = SpellRenderer.Instance.defaultUnlit;

        if (!TryGetComponent(out bc)) {
            bc = gameObject.AddComponent<BoxCollider2D>();
        }
        bc.size = new Vector2( 1.0f, 1.0f );
        bc.isTrigger = false;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.TryGetComponent(out BlockScript _)){
            if (VoidType_ == VoidType.Water && !bc.isTrigger) 
            {
                sr.sprite = SpriteDict[VoidFills][0];
                Instantiate(SoundDict[VoidType_ + "DropSound"]);
                Destroy(collision.gameObject);
                Destroy(bc); Destroy(rb);
            } else if (VoidType_ == VoidType.Void && !bc.isTrigger) 
            {
                Instantiate(SoundDict[VoidType_ + "DropSound"]);
                Destroy(collision.gameObject);
            }
        }
    }

    private void ToggleFrozen(bool boo){
        if (boo) {
            sr.sprite = SpriteDict[VoidFills][1];
            bc.isTrigger = true;
            StartCoroutine(FreezeForTime(Duration));
        } else {
            sr.sprite = null;
            bc.isTrigger = false;
        }
    }

    private void ChainReactionIce() {
        int mask = 1 << 2;
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 1f, mask)) {
            if(collider.TryGetComponent(out EmptySpaceScript ESS)) {
                if (!ESS.isFrozen_) {
                    ESS.TakeDamage(1f, Elements.Ice, SpellTemplates.Orb);
                }
            }
        }
    }

    private void ChainReactionElecticity() {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 1f, 1 << 2)) {
            if (collider.TryGetComponent(out iDamageInterface _)) {
                if (collider.TryGetComponent(out EmptySpaceScript ESS)) {
                    if (!ESS.isFrozen_ && !ESS.isElectric_) {
                        ESS.TakeDamage(1f, Elements.Electricity, SpellTemplates.Orb);
                    }
                }
            }
        }
        int mask = 1 << 6 | 1 << 7;
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 1f, mask)) {
            if (collider.TryGetComponent(out iDamageInterface iHealth)) {
                iHealth.TakeDamage(1f, Elements.Electricity);
            }
        }
    }

    private IEnumerator FreezeForTime(float duration) {
        yield return new WaitForSeconds(duration);
        isFrozen_ = false;
    }

    private IEnumerator ElectricForTime(float duration) {
        isElectric_ = true;
        Debug.Log("Elec Start");
        yield return new WaitForSeconds(duration);
        isElectric_ = false;
        Debug.Log("Elec Start");
    }

    public void TakeDamage(float damage, Elements damageType, SpellTemplates damageSource = SpellTemplates.NULL) {
        if(VoidType_ == VoidType.Water) {
            if (damageType == Elements.Ice) {
                isFrozen_ = true;
                if (damageSource == SpellTemplates.Orb) {
                    ChainReactionIce();
                }
            } else if (damageType == Elements.Fire) {
                isFrozen_ = false;
            } else if (damageType == Elements.Electricity) {
                if(!isFrozen_ && !isElectric_ && damageSource == SpellTemplates.Orb) {
                    StartCoroutine(ElectricForTime(0.1f));
                    ChainReactionElecticity();
                }
            }
        }
    }
}