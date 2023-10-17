using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SpellRenderer;

public class HotbarHandler : MonoBehaviour
{
    public GameObject Crafting_Menu;
    public bool Crafting_Menu_Active { get { return Crafting_Menu.activeSelf; } set { Crafting_Menu.SetActive(value); } }

    public Sprite Active_slot;
    public Sprite Inactive_slot;

    public Image[] Slots = new Image[5];
    HotbarItem[] Hotbar = new HotbarItem[5];
    public class HotbarItem
    {
        public SpellEffector effector;
        public SpellTemplate template;
        public void run()
        {
            template.RunFunction(effector);     
        }
    }

    public Shader PalleteShader;
    private PlayerEntity Player;

    public CraftingSyst craftsyst;

    private void Awake()
    {
        Player = FindObjectOfType<PlayerEntity>();
        UpdateSlots();
    }

    float casttime_;
    float CastTime { get { return casttime_; } set { casttime_ = Time.timeSinceLevelLoad + value; } }//Automatically update the cast time to the new time
    

    int activeslot = 0;
    void Update()
    {
        if (!Crafting_Menu_Active)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                activeslot = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                activeslot = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3))
                activeslot = 2;
            if (Input.GetKeyDown(KeyCode.Alpha4))
                activeslot = 3;
            if (Input.GetKeyDown(KeyCode.Alpha5))
                activeslot = 4;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log($"{activeslot} - {CastTime} : {Time.timeSinceLevelLoad}");
                if (Hotbar[activeslot] != null && CastTime <= Time.timeSinceLevelLoad)
                {
                    Hotbar[activeslot].run();
                    Player.CastSpell(Hotbar[activeslot].template);
                    CastTime = Hotbar[activeslot].template.CastDelay;
                }
            }

            for (int i = 0; i < Slots.Length; i++)
                if (i != activeslot)
                    Slots[i].gameObject.transform.parent.GetComponent<Image>().sprite = Inactive_slot;
            Slots[activeslot].gameObject.transform.parent.GetComponent<Image>().sprite = Active_slot;
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Crafting_Menu_Active = !Crafting_Menu_Active;
            Time.timeScale = Crafting_Menu_Active ? 0 : 1;
            if (!Crafting_Menu_Active)
                craftsyst.ResetUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Crafting_Menu_Active && !craftsyst.CurrentlyAssigning)
        {
            Crafting_Menu_Active = !Crafting_Menu_Active;
            Time.timeScale = Crafting_Menu_Active ? 0 : 1;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && Crafting_Menu_Active && craftsyst.CurrentlyAssigning)
        {
            craftsyst.CancelCraft();
        }
    }

    void UpdateSlots()
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            if(Hotbar[i] == null)
            {
                Slots[i].color = new Color32(255,255,255,0);
            }
            else
            {
                //if (Hotbar[i].template.Name.Contains("Right")) {
                //    Slots[i].GetComponent<SpriteRenderer>().flipY = true;
                //} else {
                //    Slots[i].GetComponent<SpriteRenderer>().flipY = false;
                //}
                Slots[i].color = new Color32(255, 255, 255, 255);
                Slots[i].sprite = Hotbar[i].template.icon;
                Slots[i].material = CreateMaterial(Hotbar[i].effector.Colors, PalleteShader);
            }
        }
    }

    public void AssignSpell( HotbarItem item, int placement)
    {
        Hotbar[placement] = item;
        //Debug.LogWarning($"{Hotbar[placement].template.Name} - {Hotbar[placement].effector.Name}");
        UpdateSlots();
    }
}
