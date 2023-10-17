using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFX : MonoBehaviour, iAnimationInterface
{
    public Animator Anim_ => throw new System.NotImplementedException();

    public void AlertObservers(EnumsAndDictionaries.AnimationEvents message) {
        Destroy(this.gameObject);
    }

    public void UpdateAnimation(Vector3 change) {
        throw new System.NotImplementedException();
    }
}
