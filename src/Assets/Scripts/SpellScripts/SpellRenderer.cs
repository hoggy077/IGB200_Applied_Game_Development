using UnityEngine;
using System.Collections;
using static EnumsAndDictionaries;
using System;
using System.Collections.Generic;
[ExecuteInEditMode]
public class SpellRenderer : MonoBehaviour
{
    #region Singleton Things
    private static SpellRenderer _instance;
    public static SpellRenderer Instance { get { return _instance; } } 
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    #endregion
    public Material defaultUnlit;
    public Shader shader;
    #region Ray Drawer
    public Sprite[] rayPieces;
    GameObject spellMaster;
    public void CreateRay(Transform Origin, Vector3 point, Color[] colors) {
        spellMaster = new GameObject("Ray Master");
        Directions DirectionEnum = Origin.GetComponent<iFacingInterface>().GetEntityDirectionEnum();
        Vector2 Direction = VectorDict[DirectionEnum];
        float rotationAmount = RotationDict[DirectionEnum];
        Vector3 offset = Origin.GetComponent<SpriteRenderer>().bounds.size * Direction;
        spellMaster.transform.position = Origin.position;

        //Set Sprite Colours
        Material material = CreateMaterial(colors);

        // Create the laser start
        GameObject start = CreateObject(rayPieces[0], material, offset);
        start.transform.Rotate(Vector3.forward * rotationAmount);
        start.GetComponent<SpriteRenderer>().sortingOrder = Origin.GetComponent<SpriteRenderer>().sortingOrder;
        // Laser middle
        GameObject middle = CreateObject(rayPieces[1], material, offset);
        middle.transform.Rotate(Vector3.forward * rotationAmount);
        middle.GetComponent<SpriteRenderer>().sortingOrder = start.GetComponent<SpriteRenderer>().sortingOrder - 1;

        // -- Create the end sprite
        GameObject end = CreateObject(rayPieces[2], material, offset);
        end.transform.Rotate(Vector3.forward * rotationAmount);
        end.GetComponent<SpriteRenderer>().sortingOrder = start.GetComponent<SpriteRenderer>().sortingOrder + 1;

        // Define an the maximum size, not too big but enough to go off screen
        

        // Place things
        // -- Gather some data
        float startSpriteWidth =    (start.GetComponent<Renderer>().bounds.size * Direction).magnitude;
        float endSpriteWidth =      (end.GetComponent<Renderer>().bounds.size * Direction).magnitude;
        float currentLaserSize = Vector2.Distance(point, Origin.position);

        start.transform.localPosition = 0.5f * offset;//Direction * (Mathf.Min(endSpriteWidth, currentLaserSize));
        end.transform.localPosition = Direction * Vector2.Distance(point, Origin.position);
        middle.transform.localScale = new Vector3(middle.transform.localScale.x, currentLaserSize, middle.transform.localScale.z);
        middle.transform.localPosition = (end.transform.localPosition + start.transform.localPosition) /2;


        // -- the middle is after start and, as it has a center pivot, have a size of half the laser (minus start and end)

        //middle.transform.localPosition += 0.5f * offset;
        spellMaster.AddComponent<DestroyThis>();
        start.AddComponent<DestroyThis>();
        middle.AddComponent<DestroyThis>();
        end.AddComponent<DestroyThis>();
    }
    #endregion
    public AnimationCurve arcCurve;
    public GameObject ArcSprite;
    #region Arc Drawer
    public GameObject CreateArc(Transform Origin, Color[] colors) {
        spellMaster = new GameObject("Arc Master");
        spellMaster.transform.position = Origin.position;
        Vector3 offset = Origin.GetComponent<SpriteRenderer>().bounds.size * Origin.GetComponent<iFacingInterface>().GetEntityDirection();
        GameObject arcObject = Instantiate(ArcSprite);
        arcObject.transform.position = Origin.position + offset;
        arcObject.GetComponent<SpriteRenderer>().material = CreateMaterial(colors);
        arcObject.GetComponent<SpriteRenderer>().sortingLayerName = "VFX";
        arcObject.GetComponent<SpriteRenderer>().sortingOrder = 6;
        TrailRenderer tr = arcObject.AddComponent<TrailRenderer>();
        tr.startColor = colors[0];
        tr.endColor = colors[2];
        tr.sortingLayerName = "Objects";
        tr.time = 0.8f;
        tr.startWidth = 0.4f;
        tr.endWidth = 0.2f;
        tr.material = Origin.GetComponent<Renderer>().material;
        tr.sortingLayerName = "VFX";
        tr.sortingOrder = 5;
        Destroy(spellMaster, 1f);
        return arcObject;
    }
    #endregion

    #region Orb Drawer
    public GameObject OrbSprite;
    public GameObject CreateOrb(Transform Origin, Color[] colors) {
        spellMaster = new GameObject("Orb Master");
        spellMaster.transform.position = Origin.position;
        Vector3 offset = Origin.GetComponent<SpriteRenderer>().bounds.size * Origin.GetComponent<iFacingInterface>().GetEntityDirection();
        GameObject orbObject = Instantiate(OrbSprite);
        orbObject.transform.position = Origin.position + offset;
        orbObject.GetComponent<SpriteRenderer>().material = CreateMaterial(colors);
        orbObject.GetComponent<SpriteRenderer>().sortingLayerName = "VFX";
        orbObject.GetComponent<SpriteRenderer>().sortingOrder = 6;
        Destroy(spellMaster, 1f);
        return orbObject;
    }
    #endregion
    public GameObject ConeSprite;
    #region Cone Drawer
    public void CreateCone(Transform Origin, Color[] colors) {
        spellMaster = new GameObject("Cone Master");
        spellMaster.transform.position = Origin.transform.position;
        GameObject coneObject = Instantiate(ConeSprite);
        coneObject.transform.parent = spellMaster.transform;
        Vector2 offset = Origin.GetComponent<SpriteRenderer>().bounds.size * Origin.GetComponent<iFacingInterface>().GetEntityDirection();
        coneObject.transform.localPosition = Vector2.zero + offset;
        coneObject.GetComponent<Renderer>().material = CreateMaterial(colors);
        coneObject.transform.Rotate(Vector3.forward * RotationDict[Origin.GetComponent<iFacingInterface>().GetEntityDirectionEnum()]);
        Destroy(spellMaster, 1f);
    }

    #endregion
    public Sprite ShieldSprite;
    #region Shield Drawer

    #endregion

    #region Runner Drawer

    #endregion

    #region Effects and Particles
    public GameObject PulseFXObject;
    public void CreateBurstFX(Vector3 position, Color[] colors) {
        Material material = CreateMaterial(colors);
        GameObject PFXO =  Instantiate(PulseFXObject);
        PFXO.GetComponent<Renderer>().material = material;
        PFXO.transform.position = position;
        Destroy(PFXO, 0.5f);
    }
    #endregion
    #region Recycled Code
    private GameObject CreateObject(Sprite sprite, Material material, Vector2 offset) {
        GameObject obj = new GameObject();
        obj.AddComponent<SpriteRenderer>().sprite = sprite;
        obj.GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
        obj.GetComponent<SpriteRenderer>().sortingOrder = -1* (int) obj.transform.position.y;
        obj.GetComponent<Renderer>().material = material;
        obj.transform.parent = spellMaster.transform;
        obj.transform.localPosition = Vector2.zero + offset;
        return obj;
    }
    public Material CreateMaterial(Color[] colors) {
        return CreateMaterial(colors, shader);
    }
    public static Material CreateMaterial(Color[] colors, Shader shader) {
        Material material = new Material(shader);
        material.SetColor("_PrimaryColour", colors[0]);
        material.SetColor("_SecondaryColour", colors[1]);
        material.SetColor("_AccentColour1", colors[2]);
        material.SetColor("_AccentColour2", colors[3]);
        return material;
    }
    #endregion
}