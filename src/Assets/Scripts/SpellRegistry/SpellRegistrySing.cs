using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;


public interface SpellEffector_in
{

}


[Serializable]
public class SpellEffector : SpellEffector_in
{
    public string Name { get; set; }
    public Color[] Colors { get; set; }
    public Action<iEffectorData> Effector;
}

public class SpellTemplate
{

    public Action<SpellEffector> RunFunction;
    public Sprite icon;
    public string Name;
    public long CastingId;
    public float CastDelay;

    public SpellTemplate(string Name, Sprite sprite, Action<SpellEffector> onRun, float Delay)
    {
        this.Name = Name;
        RunFunction = onRun;
        icon = sprite;
        CastDelay = Delay;
    }
}

public sealed class SpellRegistrySing : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public static SpellRegistrySing Instance { get; private set; }
    private SpellRegistry _registry;
    public SpellRegistry Registry { get { return _registry; } }
    private SpellRegistrySing() { _registry = new SpellRegistry(); }
    static SpellRegistrySing() { 
        //Instance = new SpellRegistrySing(); 
        GameObject gme = new GameObject() { name="SpellRegistrySing" };
        gme.AddComponent(typeof(SpellRegistrySing));
        Instance = gme.GetComponent<SpellRegistrySing>();
        gme.tag = "SpellReg";
    }
}

public class SpellRegistry
{
    public delegate void onAdd_d(SpellRegArgs args);
    public event onAdd_d onAdd;
    public delegate void onRemove_d();
    public event onRemove_d onRemove;


    private Dictionary<string, SpellTemplate> S_Registry = new Dictionary<string, SpellTemplate>();

    private ObjectIDGenerator iDGenerator = new ObjectIDGenerator();

    public void AddItemToregistry(SpellTemplate template)
    {
        template.CastingId = iDGenerator.GetId(template, out _);
        if (!S_Registry.Values.Contains(template))
        {
            S_Registry.Add(template.Name, template);
            if (onAdd != null)
                onAdd.Invoke( new SpellRegArgs() { template = template });
        }
    }

    public void RemoveItemToregistry(SpellTemplate template)
    {
        if (S_Registry.ContainsKey(template.Name))
        {
            S_Registry.Remove(template.Name);
            onRemove.Invoke();
        }
    }

    public void RemoveItemToregistry(string Name)
    {
        if (S_Registry.ContainsKey(Name))
        {
            S_Registry.Remove(Name);
            onRemove.Invoke();
        }
    }

    public void RemoveItemToregistry(int index)
    {
        if(index >= 0 && index < S_Registry.Count)
        {
            S_Registry.Remove(S_Registry.Keys.ElementAt(index));
            onRemove.Invoke();
        }
    }

    public SpellTemplate QueryRegistry(string Name)
    {
        return S_Registry.ContainsKey(Name) ? S_Registry[Name] : null;
    }

    public SpellTemplate QueryRegistry(int Index)
    {
        return S_Registry.Count < Index && Index >= 0 ? S_Registry.Values.ToList()[Index] : null;
    }

    public long QueryForSid(string Name)
    {
        return S_Registry.ContainsKey(Name) ? S_Registry[Name].CastingId : -1;
    }

    public SpellTemplate QueryWithSid(long Sid)
    {
        return S_Registry.Values.First((val) => { return val.CastingId == Sid; });
    }

    public SpellTemplate[] QueryAll()
    {
        return S_Registry.Values.ToArray();
    }

    #region old
    //public int QuerySidRegistry(string Name)
    //{
    //    return Sid_Registry.ContainsKey(Name) ? Sid_Registry[Name] : -1;
    //}

    //public int QuerySidRegistry(int Index)
    //{
    //    return Sid_Registry.Count < Index && Index >= 0 ? Sid_Registry.Values.ToList()[Index] : -1 ;
    //}

    //public Dictionary<string, int> PresentSidRegistry()
    //{
    //    return Sid_Registry;
    //}
    #endregion
}

public class SpellRegArgs
{
    public SpellTemplate template;
    public string name { get { return template.Name; } }
}





public interface ExternalSpell
{
    void Initialize();
}




#region Data formats

public interface iEffectorData 
{
    public GameObject CasterObject { get; set; }
    SpellTemplate Calling_template { get; set; }

    //The Base Strength of a Spell; this is used for determining damage or force
    public float baseStrength { get;}
}

public class RayData : iEffectorData
{
    public GameObject CasterObject { get; set; }
    public SpellTemplate Calling_template { get; set; }
    public RaycastHit2D Data { get; set; }
    public float baseStrength { get => 2f;}
}

public class ArcData : iEffectorData
{
    public GameObject CasterObject { get; set; }
    public SpellTemplate Calling_template { get; set; }
    public GameObject Data { get; set; }
    public EnumsAndDictionaries.ArcDirections ArcDirection { get; set; }
    public float baseStrength { get => 3f; }
}

public class ConeData : iEffectorData
{
    public GameObject CasterObject { get; set; }
    public SpellTemplate Calling_template { get; set; }
    public GameObject[] Data { get; set; }

    public float baseStrength { get => 1f; }
}

public class OrbData : iEffectorData
{
    public GameObject CasterObject { get; set; }
    public SpellTemplate Calling_template { get; set; }
    public GameObject Data { get; set; }

    public float baseStrength { get => 1.5f; }
}

public class ShieldData : iEffectorData
{
    public GameObject CasterObject { get; set; }
    public SpellTemplate Calling_template { get; set; }
    public GameObject Data { get => CasterObject; set => _ = value; }
    public float baseStrength { get => 1.5f; }
}

public class GeneralData : iEffectorData
{
    public GameObject CasterObject { get; set; }
    public SpellTemplate Calling_template { get; set; }
    public GameObject Target { get; set; }
    public float baseStrength { get; }
}

#endregion