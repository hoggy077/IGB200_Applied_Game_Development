using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static iPropertyInterface;
using static EnumsAndDictionaries;
using static SpellFunctionLibrary;

public class TestSpell : MonoBehaviour
{
    //readonly SpellEffector FireRay = new SpellEffector() {
    //    Name = "Test",
    //    Effector = new Action<iEffectorData>((EffectorData_) => {
    //        RayData Ray_ = (RayData)EffectorData_;
    //        Debug.Log($"Test Spell Fire recieved: {Ray_.Data.point.x} - {Ray_.Data.point.y}");

    //        /*Fire Properties
    //         * Damage to enemies is based on their properties. this code could be recycled for other pieces, changing base damage among other things.
    //         * */
    //        float baseDmg = 10f;
    //        Debug.Log(Ray_.Data.collider.transform.name + " was hit.");
    //        if (Ray_.Data.collider.gameObject.TryGetComponent<iHealthInterface>(out iHealthInterface otherEntity)) {
    //            if (otherEntity.EntityProperties_.Contains(Properties.Flamable)) {
    //                otherEntity.TakeDamage(baseDmg * 2f, Elements.Fire);
    //            } else if (otherEntity.EntityProperties_.Contains(Properties.Fireproof)) {
    //                otherEntity.TakeDamage(0f, Elements.Fire);
    //            } else {
    //                otherEntity.TakeDamage(baseDmg, Elements.Fire);
    //            }
    //        }
    //    }),
    //    Colors = ColourDict[Elements.Fire]        
    //};


    //readonly SpellEffector FireArc = new SpellEffector() {
    //    Name = "Test",
    //    Effector = new Action<iEffectorData>((EffectorData_) => {
    //        ArcData Arc_ = (ArcData)EffectorData_;
    //        ArcScript ac = Arc_.Data.AddComponent<ArcScript>();
    //        ac.direction = Arc_.CasterObject.GetComponent<iFacingInterface>().GetEntityDirectionEnum();
    //        ac.arcDirection = Arc_.ArcDirection;
    //        ac.GetComponent<MonoBehaviour>().StartCoroutine(ArcOther(ac, Arc_.baseStrength, Elements.Fire));
    //    }),
    //    Colors = ColourDict[Elements.Fire]
    //};

    //readonly SpellEffector PushArc = new SpellEffector() {
    //    Name = "Test",
    //    Effector = new Action<iEffectorData>((EffectorData_) => {
    //        ArcData Arc_ = (ArcData)EffectorData_;
    //        ArcScript ac = Arc_.Data.AddComponent<ArcScript>();
    //        ac.direction = Arc_.CasterObject.GetComponent<iFacingInterface>().GetEntityDirectionEnum();
    //        ac.arcDirection = Arc_.ArcDirection;
    //        ac.GetComponent<MonoBehaviour>().StartCoroutine(ArcOther(ac, Arc_.baseStrength, Elements.Push));
    //    }),
    //    Colors = ColourDict[Elements.Push]
    //};



    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //        SpellRegistrySing.Instance.Registry.QueryRegistry("Ray").RunFunction(FireRay);
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //        SpellRegistrySing.Instance.Registry.QueryRegistry("Arc").RunFunction(FireArc);
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //        SpellRegistrySing.Instance.Registry.QueryRegistry("Arc").RunFunction(PushArc);
    //}
}
