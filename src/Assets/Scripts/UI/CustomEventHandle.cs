using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class CustomEventHandle : MonoBehaviour
{
    #region crafting Evnt syst
    #region custom evnt handling
    public class EventData
    {
        public enum EvntType
        {
            Template,
            Effect
        }

        public string eventName;
        public GameObject SentBy;
        public Sprite SlotSprite;
        public SpellEffector effector;
        public SpellTemplate template;
        public EvntType type;

        public Sprite scriptbox_sprite;

        public bool HasAdditionalParam;
        public GameObject AdditionalParam_menu;
        public bool IsAdditionalParam;
    }

    public class EvntHndl_args
    {
        public EvntHndl_args() { }
        public EventData eventData;
    }

    public delegate void CustomEventHandler(object sender, EvntHndl_args e);
    public event CustomEventHandler onBroadcast;
    #endregion

    [Header("Crafting system Event items")]
    public EventData.EvntType dataType;
    public string eventName;
    public string effector_name;
    public string template_name;
    public Sprite Crafting_Slot_img;

    public Sprite ScriptBox_sprite;

    [Header("Additional params - Leave unless needed")]
    public bool HasAdditionalParam = false;
    public GameObject AdditionalParamMenu;
    public bool IsAdditionalParam = false;


    public void BroadcastEvent()
    {
        if (HasAdditionalParam && IsAdditionalParam)
            throw new Exception($"{eventName} Is marked as both having an additional param, and being one. Please un mark one of these.");
        if ((HasAdditionalParam || IsAdditionalParam) && dataType == EventData.EvntType.Template)
            throw new Exception($"{eventName} Is an additional param, or has one as a template, which is not allowed.");

        EventData eventData = new EventData()
        {
            SentBy = gameObject,
            type = dataType,
            eventName = eventName,
            SlotSprite = Crafting_Slot_img,
            HasAdditionalParam = HasAdditionalParam,
            AdditionalParam_menu = AdditionalParamMenu,
            IsAdditionalParam = IsAdditionalParam,
            scriptbox_sprite = ScriptBox_sprite
        };

        if (dataType == EventData.EvntType.Effect)
            eventData.effector = Effectors.Find(effector_name);
        if (dataType == EventData.EvntType.Template)
            eventData.template = SpellRegistrySing.Instance.Registry.QueryRegistry(template_name);


        onBroadcast.Invoke(this, new EvntHndl_args() { eventData = eventData });
    }
    #endregion

    #region Unlock syst
    
    [Header("ignore")]
    public Button Mine;
    [Header("Loot system shit")]
    public bool Disabled = true;

    private void Start()
    {
        if (gameObject.GetComponent<Button>() && Mine == null)
        {
            Mine = gameObject.GetComponent<Button>();
        }

        //register the template or effect  - THIS IS NOW HANDLED ELSE WHERE
        //if(!Array.Exists(UnlockManager.Instance.Registry.AllKeys(), (i) => { return i == effector_name ? true : i == template_name ? true : false; })){
        //    UnlockManager.Instance.Registry.AddUnlockItem(new SpellWrapper(dataType == EventData.EvntType.Template ? UnlockType.TEMPLATE : UnlockType.EFFECTOR, dataType == EventData.EvntType.Template ? SpellRegistrySing.Instance.Registry.QueryRegistry(template_name) : null, dataType == EventData.EvntType.Effect ? Effectors.Find(effector_name) : null));
        //}

        //UnlockManager.Instance.Registry.ItemUnlocked += Registry_ItemUnlocked;

        UpdateImg();
    }

    public void Registry_ItemUnlocked(UnlockArgs args)
    {
        if(dataType == EventData.EvntType.Template && args.Item.Type() == UnlockType.TEMPLATE)
        {
            if (args.name == template_name)
            {
                Disabled = false;
                UpdateImg();
            }
        }
        else if (dataType == EventData.EvntType.Effect && args.Item.Type() == UnlockType.EFFECTOR)
        {
            if (args.name == effector_name)
            {
                Disabled = false;
                UpdateImg();
            }
        }
    }

    private void UpdateImg()
    {
        if (gameObject.GetComponent<Button>() && Mine == null)
        {
            Mine = gameObject.GetComponent<Button>();
        }

        try
        {
            Mine.interactable = !Disabled;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    #endregion
}
