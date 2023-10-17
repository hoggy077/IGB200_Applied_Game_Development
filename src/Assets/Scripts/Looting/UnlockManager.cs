using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UnlockManager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }


    public static UnlockManager Instance { get; private set; }
    private UnlockSingleton _registry;
    public UnlockSingleton Registry { get { return _registry; } }
    private UnlockManager() { _registry = new UnlockSingleton(); }
    static UnlockManager()
    {
        //Instance = new SpellRegistrySing(); 
        GameObject gme = new GameObject() { name = "UnlockManagerSing" };
        gme.AddComponent(typeof(UnlockManager));
        Instance = gme.GetComponent<UnlockManager>();
        gme.tag = "UnlockManager";
    }

}



public class UnlockSingleton
{
    public delegate void ItemUnlocked_(UnlockArgs args);
    public event ItemUnlocked_ ItemUnlocked;

    Dictionary<string, aUnlockManagerWrap> Items = new Dictionary<string, aUnlockManagerWrap>();

    public void AddUnlockItem(aUnlockManagerWrap item)
    {
        switch (item.Type())
        {
            case UnlockType.TEMPLATE:
                Items.Add(item.GetValue().Template.Name, item);
                break;

            case UnlockType.EFFECTOR:
                Items.Add(item.GetValue().Effector.Name, item);
                break;
        }
    }

    public aUnlockManagerWrap QueryByName(string name)
    {
        if (name == "")
            return null;
        return Items.Where((kvp) => { return kvp.Value.Type() == UnlockType.TEMPLATE ? kvp.Value.GetValue().Template.Name == name : kvp.Value.GetValue().Effector.Name == name; }).First().Value;
    }

    public string[] AllKeys()
    {
        return Items.Keys.ToArray();
    }

    public bool UnlockItem(string KeyName)
    {
        if (Items.ContainsKey(KeyName))
        {
            Items[KeyName].Unlock();
            ItemUnlocked.Invoke(new UnlockArgs() { Item = Items[KeyName] });
            return true;
        }
        return false;
    }

    public bool IsUnlocked(string KeyName)
    {
        if (Items.ContainsKey(KeyName))
        {
            return Items[KeyName].Unlocked();
        }
        throw new System.Exception($"Items does not contain a value for {KeyName}, please check it does using AllKeys");
    }
}

#region Args for unlock del
public class UnlockArgs
{
    public aUnlockManagerWrap Item;
    public string name { get { return Item.Type() == UnlockType.TEMPLATE ? Item.GetValue().Template.Name : Item.GetValue().Effector.Name; } }
}
#endregion

#region Wrapping and identifying registry items
public enum UnlockType
{
    TEMPLATE,
    EFFECTOR
}

public abstract class aUnlockManagerWrap
{
    public abstract UnlockType Type();
    public abstract bool Unlocked();
    public abstract void Unlock();
    public abstract ReturnVal GetValue();
}

public class SpellWrapper : aUnlockManagerWrap
{
    public UnlockType ActiveType = UnlockType.TEMPLATE;
    public bool UnlockedState { get; private set; } = false;

    public override bool Unlocked() => UnlockedState;
    public override void Unlock() => UnlockedState = !UnlockedState;

    private SpellTemplate Template;
    private SpellEffector Effector;

    public SpellWrapper(UnlockType type, SpellTemplate template, SpellEffector effector)
    {
        ActiveType = type;
        switch (type)
        {
            case UnlockType.TEMPLATE:
                Template = template;
                break;

            case UnlockType.EFFECTOR:
                Effector = effector;
                break;
        }
    }

    public override ReturnVal GetValue()
    {
        return new ReturnVal() { Effector = Effector, Template = Template, Type = ActiveType };
    }

    public override UnlockType Type() => ActiveType;
}

public struct ReturnVal
{
    public SpellTemplate Template;
    public SpellEffector Effector;
    public UnlockType Type;
}
#endregion