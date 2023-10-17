using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumsAndDictionaries;
using static SpellFunctionLibrary;

public static class Effectors
{
    public static List<SpellEffector> SpellEffects = new List<SpellEffector>()
    {
        new SpellEffector()
        {
            Name = "Test",
            Colors =  ColourDict[Elements.NULL],
            Effector = new Action<iEffectorData>((edd) => { Debug.Log("works"); })
        },

    #region Damage Effectors
        new SpellEffector() {
            Name = "Fire",
            Effector = new Action<iEffectorData>((EffectorData_) => {
                Elements element = Elements.Fire;
                DamageEffector(EffectorData_, element);
            }),
            Colors = ColourDict[Elements.Fire]
        },
        new SpellEffector() {
            Name = "Ice",
            Effector = new Action<iEffectorData>((EffectorData_) => {
                Elements element = Elements.Ice;
                DamageEffector(EffectorData_, element);
            }),
            Colors = ColourDict[Elements.Ice]
        },
        new SpellEffector() {
            Name = "Life",
            Effector = new Action<iEffectorData>((EffectorData_) => {
                Elements element = Elements.Life;
                DamageEffector(EffectorData_, element);
            }),
            Colors = ColourDict[Elements.Life]
        },
        new SpellEffector() {
            Name = "Electricity",
            Effector = new Action<iEffectorData>((EffectorData_) => {
                Elements element = Elements.Electricity;
                DamageEffector(EffectorData_, element);
            }),
            Colors = ColourDict[Elements.Electricity]
        },
    #endregion

    #region Physics Other
        new SpellEffector() {
            Name = "Push",
            Effector = new Action<iEffectorData>((EffectorData_) => {

                Elements element = Elements.Push;
                PhysicsEffector(EffectorData_, element);
            }),
            Colors = ColourDict[Elements.Push]
        },
        new SpellEffector() {
            Name = "Pull",
            Effector = new Action<iEffectorData>((EffectorData_) => {
                Elements element = Elements.Pull;
                PhysicsEffector(EffectorData_, element);
            }),
            Colors = ColourDict[Elements.Pull]
        },
    #endregion

    # region Physics Self
        new SpellEffector() {
            Name = "PullPlayer",
            Effector = new Action<iEffectorData>((EffectorData_) => {                
                Elements element = Elements.Pull;
                string CallingTemp = EffectorData_.Calling_template.Name;
                if (CallingTemp.Contains("Arc")) {
                    CallingTemp = "Arc";
                }
                switch (CallingTemp) {
                    case "Ray":
                        RayData Ray_ = (RayData)EffectorData_;
                        Ray_.CasterObject.GetComponent<MonoBehaviour>().StartCoroutine(LerpSelf(Ray_.CasterObject, Ray_.Data.point, 1f));
                        break;
                                            
                    case "Arc":
                        ArcData Arc_ = (ArcData)EffectorData_;
                        ArcScript ac = Arc_.Data.AddComponent<ArcScript>();
                        ac.direction = Arc_.CasterObject.GetComponent<iFacingInterface>().GetEntityDirectionEnum();
                        ac.arcDirection = Arc_.ArcDirection;
                        ac.GetComponent<MonoBehaviour>().StartCoroutine(ArcSelf(ac, Arc_, element));
                        break;

                    case "Cone":
                        ConeData Cone_ = (ConeData)EffectorData_;
                        ConeProcess(Cone_, EffectorData_.baseStrength, element);
                        break;

                    case "Orb":
                        OrbData Orb_ = (OrbData)EffectorData_;
                        OrbScript orbScript = Orb_.Data.AddComponent<OrbScript>();
                        orbScript.element = element;
                        orbScript.targetsPlayer = true;
                        orbScript.baseDamage = EffectorData_.baseStrength;
                        Orb_.Data.GetComponent<Rigidbody2D>().AddForce(4 * Orb_.CasterObject.GetComponent<iFacingInterface>().GetEntityDirection(), ForceMode2D.Impulse);
                        break;

                    default:
                        Debug.Log($"{EffectorData_.Calling_template.Name} is not yet defined for PullPlayer");
                        break;
                }
            }),
            Colors = ColourDict[Elements.Pull]
        },
        new SpellEffector() {
            Name = "PushPlayer",
            Effector = new Action<iEffectorData>((EffectorData_) => {                
                Elements element = Elements.Push;
                string CallingTemp = EffectorData_.Calling_template.Name;
                if (CallingTemp.Contains("Arc")) {
                    CallingTemp = "Arc";
                }
                switch (CallingTemp) {
                    case "Ray":
                        RayData Ray_ = (RayData)EffectorData_;                        
                        if (Ray_.CasterObject.TryGetComponent(out iPhysicsInterface thisEntity)) {
                            Vector2 Direction = Ray_.CasterObject.transform.GetComponent<iFacingInterface>().GetEntityDirection();
                            thisEntity.UpdateForce(EffectorData_.baseStrength, -1 * Direction, element);
                        }
                        break;

                    case "Arc":
                        ArcData Arc_ = (ArcData)EffectorData_;
                        ArcScript ac = Arc_.Data.AddComponent<ArcScript>();
                        ac.direction = Arc_.CasterObject.GetComponent<iFacingInterface>().GetEntityDirectionEnum();
                        ac.arcDirection = Arc_.ArcDirection;
                        ac.GetComponent<MonoBehaviour>().StartCoroutine(ArcSelf(ac, Arc_, element));                        
                        break;

                    case "Cone":
                        ConeData Cone_ = (ConeData)EffectorData_;
                        float TotalForce = 0;
                        foreach (GameObject gameObject in Cone_.Data) {
                            if (gameObject.TryGetComponent(out iPhysicsInterface HI)) {
                                Vector2 Direction = gameObject.transform.position - Cone_.CasterObject.transform.position;
                                HI.UpdateForce(EffectorData_.baseStrength, Direction, element);
                                Debug.DrawLine(Cone_.CasterObject.transform.position, Cone_.CasterObject.transform.position, Color.magenta, 1f);
                            }
                            TotalForce += Vector2.Distance(Cone_.CasterObject.transform.position, gameObject.transform.position);
                        }
                        Vector2 direction = Cone_.CasterObject.transform.GetComponent<iFacingInterface>().GetEntityDirection();
                        Cone_.CasterObject.GetComponent<iPhysicsInterface>().UpdateForce(TotalForce, direction, element);
                        Debug.Log(TotalForce * direction);
                        break;

                    case "Orb":
                        OrbData Orb_ = (OrbData)EffectorData_;
                        OrbScript orbScript = Orb_.Data.AddComponent<OrbScript>();
                        orbScript.element = element;
                        orbScript.targetsPlayer = true;
                        orbScript.baseDamage = EffectorData_.baseStrength;
                        Orb_.Data.GetComponent<Rigidbody2D>().AddForce(4 * Orb_.CasterObject.GetComponent<iFacingInterface>().GetEntityDirection(), ForceMode2D.Impulse);
                        break;

                    default:
                        Debug.Log($"{EffectorData_.Calling_template.Name} is not yet defined for PushPlayer");
                        break;
                }
            }),
            Colors = ColourDict[Elements.Push]
        },
        #endregion
    };

    private static void DamageEffector(iEffectorData EffectorData_, Elements element) {
        string CallingTemp = EffectorData_.Calling_template.Name;
        if (CallingTemp.Contains("Arc")) {
            CallingTemp = "Arc";
        }
        switch (CallingTemp) {
            case "Ray":
                RayData Ray_ = (RayData)EffectorData_;
                if (Ray_.Data.collider.gameObject.TryGetComponent(out iDamageInterface otherEntity)) {
                    otherEntity.TakeDamage(EffectorData_.baseStrength, element);
                }
                break;

            case "Arc":
                ArcData Arc_ = (ArcData)EffectorData_;
                ArcScript ac = Arc_.Data.AddComponent<ArcScript>();
                ac.direction = Arc_.CasterObject.GetComponent<iFacingInterface>().GetEntityDirectionEnum();
                ac.arcDirection = Arc_.ArcDirection;
                ac.GetComponent<MonoBehaviour>().StartCoroutine(ArcOther(ac, EffectorData_.baseStrength, element));
                break;

            case "Cone":
                ConeData Cone_ = (ConeData)EffectorData_;
                ConeProcess(Cone_, EffectorData_.baseStrength, element);
                break;

            case "Orb":
                OrbData Orb_ = (OrbData)EffectorData_;
                OrbScript orbScript = Orb_.Data.AddComponent<OrbScript>();
                orbScript.element = element;
                orbScript.baseDamage = EffectorData_.baseStrength;
                Orb_.Data.GetComponent<Rigidbody2D>().AddForce(4 * Orb_.CasterObject.GetComponent<iFacingInterface>().GetEntityDirection(), ForceMode2D.Impulse);
                break;

            default:
                Debug.Log($"{EffectorData_.Calling_template.Name} is not yet defined for {element}");
                break;
        }
    }

    private static void PhysicsEffector(iEffectorData EffectorData_, Elements element) {
        string CallingTemp = EffectorData_.Calling_template.Name;
        if (CallingTemp.Contains("Arc")) {
            CallingTemp = "Arc";
        }
        switch (CallingTemp) {
            case "Ray":
                RayData Ray_ = (RayData)EffectorData_;
                Vector2 direction = Ray_.CasterObject.transform.GetComponent<iFacingInterface>().GetEntityDirection();
                if (Ray_.Data.collider.gameObject.TryGetComponent(out iPhysicsInterface otherEntity)) {
                    otherEntity.UpdateForce(EffectorData_.baseStrength, direction, element);
                }
                break;

            case "Arc":
                ArcData Arc_ = (ArcData)EffectorData_;
                ArcScript ac = Arc_.Data.AddComponent<ArcScript>();
                ac.direction = Arc_.CasterObject.GetComponent<iFacingInterface>().GetEntityDirectionEnum();
                ac.arcDirection = Arc_.ArcDirection;
                ac.GetComponent<MonoBehaviour>().StartCoroutine(ArcOther(ac, EffectorData_.baseStrength, element));
                break;

            case "Cone":
                ConeData Cone_ = (ConeData)EffectorData_;
                ConeProcess(Cone_, EffectorData_.baseStrength, element);
                break;

            case "Orb":
                OrbData Orb_ = (OrbData)EffectorData_;
                OrbScript orbScript = Orb_.Data.AddComponent<OrbScript>();
                orbScript.element = element;
                orbScript.baseDamage = EffectorData_.baseStrength;
                Orb_.Data.GetComponent<Rigidbody2D>().AddForce(4 * Orb_.CasterObject.GetComponent<iFacingInterface>().GetEntityDirection(), ForceMode2D.Impulse);
                break;

            default:
                Debug.Log($"{EffectorData_.Calling_template.Name} is not yet defined for {element}");
                break;
        }
    }

    public static SpellEffector Find(string name) {
        return SpellEffects.Find((e) => { return e.Name == name; });
    }
}
