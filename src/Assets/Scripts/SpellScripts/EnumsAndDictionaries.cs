using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumsAndDictionaries
{
    public enum ArcDirections
    {
        Left, Right
    }
    public static Dictionary<ArcDirections, int> ArcValueDict = new Dictionary<ArcDirections, int>() {
        {ArcDirections.Left, 1 },
        {ArcDirections.Right, -1 }
    };

    public static Dictionary<Directions, Vector2> VectorDict = new Dictionary<Directions, Vector2>()
    {
        { Directions.Up,      new Vector2(0, 1) },
        { Directions.Down,    new Vector2(0,-1) },
        { Directions.Left,    new Vector2(-1,0) },
        { Directions.Right,   new Vector2(1, 0) },
    };

    public static Dictionary<Directions, int> IntDict = new Dictionary<Directions, int>()
{
        { Directions.Up,      0},
        { Directions.Down,    2},
        { Directions.Left,    3 },
        { Directions.Right,   1 },
    };

    public static Dictionary<Directions, float> RotationDict = new Dictionary<Directions, float>() {
        { Directions.Up,      0},
        { Directions.Down,    180},
        { Directions.Left,    -270 },
        { Directions.Right,   -90 }
    };

    public static readonly Dictionary<Elements, Color[]> ColourDict = new Dictionary<Elements, Color[]>() {
         {Elements.NULL, new Color[]
            {
            new Color32(58,68,102,255),
            new Color32(105,120,140,255),
            new Color32(157,183,194,255),
            new Color32(255,255,255,255)
            }
        },
        {Elements.Fire, new Color[]
            {
            new Color32(138,31,19,255),
            new Color32(234,19,1,255),
            new Color32(255,98,0,255),
            new Color32(247,174,45,255)
            }
        },
        {Elements.Ice, new Color[]
            {
            new Color32(19, 78, 138, 255),
            new Color32(1, 149, 234, 255),
            new Color32(137, 142, 251, 255),
            new Color32(45, 232, 247, 255)
            }
        },
        {Elements.Pull, new Color[]
            {
            new Color32(36,46,71,255),
            new Color32(104,56,108,255),
            new Color32(181,80,136,255),
            new Color32(246,117,122,255)
            }
        },
        {Elements.Push, new Color[]
            {
            new Color32(36,46,71,255),
            new Color32(181,80,136,255),
            new Color32(137,143,250,255),
            new Color32(104,56,108,255)
            }
        },
        {Elements.Life, new Color[]
            {
            new Color32(39,92,68,255),
            new Color32(64,137,73,255),
            new Color32(100,199,77,255),
            new Color32(255,255,255,255)
            }
        },
        {Elements.Death, new Color[]
            {
            new Color32(25,20,38,255),
            new Color32(40,43,68,255),
            new Color32(59,68,104,255),
            new Color32(90,104,136,255)
            }
        },
        {Elements.Earth, new Color[]
            {
            new Color32(26,59,62,255),
            new Color32(64,39,51,255),
            new Color32(117,62,57,255),
            new Color32(185,111,81,255)
            }
        },
        {Elements.Electricity, new Color[]
            {
            new Color32(248,118,35,255),
            new Color32(255,173,52,255),
            new Color32(254,230,98,255),
            new Color32(255,255,255,255)
            }
        }
    };

    [System.Serializable]
    public enum Properties
    {
        Heavy, Light, Flamable, Fireproof, Metal, Indestructable, Immovable, Undead, Frozen
    }
    [System.Serializable]
    public enum EntityTypes
    {
        Object,
        Creature
    }
    [System.Serializable]
    public enum AnimationEvents
    {
        Death, Fall, Explode, Finish, Start,
        Attack
    }
    [System.Serializable]
    public enum Directions
    {
        Up,
        Right,
        Down,
        Left
    }
    [System.Serializable]
    public enum Elements
    {
        Fire, Ice, Earth, Electricity, Pull, Push, Life, Death,
        NULL
    }
    [System.Serializable]
    public enum SpellTemplates
    {
        Ray, Orb, Arc, Cone, Shield, Runner,
        NULL
    }
    [System.Serializable]
    public enum VoidType
    {
        Water, Void
    }
    public static Directions VectorToDirection(Vector3 vector) {
        return VectorToDirection((Vector2) vector);
    }

    public static Directions VectorToDirection(Vector2 vector) {
        Directions direction;
        if (Mathf.Abs(vector.x) < Mathf.Abs(vector.y)) {
            if (vector.y > 0) {
                direction = Directions.Up;
            } else {
                direction = Directions.Down;
            }
        } else {
            if (vector.x > 0) {
                direction = Directions.Right;
            } else {
                direction = Directions.Left;
            }
        }
        return direction;
    }
}
