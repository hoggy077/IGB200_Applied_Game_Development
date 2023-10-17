using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLootCont : MonoBehaviour
{
    [SerializeField]
    List<CustomEventHandle> Events = new List<CustomEventHandle>();
    public bool Self_Populate_Events = false;

    public CraftingSyst Craftingsyst;


    public void Prepared()
    {
        if (Self_Populate_Events)
        {
            //Events.Clear();
            Events.AddRange(gameObject.GetComponentsInChildren<CustomEventHandle>(true));
        }

        foreach (CustomEventHandle customEvent in Events)
        {
            //Debug.Log($"{customEvent.eventName} registered");

            customEvent.onBroadcast += Craftingsyst.onRecieved;
            UnlockManager.Instance.Registry.ItemUnlocked += customEvent.Registry_ItemUnlocked;

            if (customEvent.dataType == CustomEventHandle.EventData.EvntType.Template)
            {
                SpellTemplate template = SpellRegistrySing.Instance.Registry.QueryRegistry(customEvent.template_name);
                SpellWrapper spw = new SpellWrapper(UnlockType.TEMPLATE, template, null);
                if (!Array.Exists(UnlockManager.Instance.Registry.AllKeys(), (e) => e == template.Name))
                    UnlockManager.Instance.Registry.AddUnlockItem(spw);
                if (customEvent.Disabled == false)
                    UnlockManager.Instance.Registry.UnlockItem(spw.GetValue().Template.Name);
            }
            else
            {
                SpellEffector effector = Effectors.Find(customEvent.effector_name);
                SpellWrapper spw = new SpellWrapper(UnlockType.EFFECTOR, null, effector);
                if (!Array.Exists(UnlockManager.Instance.Registry.AllKeys(), (e) => e == effector.Name))
                    UnlockManager.Instance.Registry.AddUnlockItem(spw);
                if (customEvent.Disabled == false)
                    UnlockManager.Instance.Registry.UnlockItem(spw.GetValue().Effector.Name);
            }
        }
    }
}
