using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumsAndDictionaries;

public class ArcScript : MonoBehaviour, iDamageInterface
{
    
    public Directions direction;
    public ArcDirections arcDirection;

    float duraScale = 1.5f;
    private float ArcDistanceScale = 3f;
    private float ArcWidthScale = 1.5f;
    private AnimationCurve ac;
    private int arcValue;
    Rigidbody2D RB_;
    float time;
    float half;
    Vector2 directionVector;
    Vector2 invertedDirection;
    public void Start() {
        
        RB_ = this.gameObject.AddComponent<Rigidbody2D>();
        RB_.gravityScale = 0;
        RB_.freezeRotation = true;
        CircleCollider2D cc = this.gameObject.AddComponent<CircleCollider2D>();
        cc.isTrigger = true;
        cc.radius = 0.1f * (GetComponent<SpriteRenderer>().bounds.size.x + GetComponent<SpriteRenderer>().bounds.size.y) / 2f;

        //initialise Variables
        duration = 2f;
        ac = SpellRenderer.Instance.arcCurve;
        //Find Value for Direction of movement
        arcValue = ArcValueDict[arcDirection];
        if (IntDict[direction] % 2 == 1) {
            arcValue *= -1;
        }
        //arcValue = Mathf.RoundToInt(Mathf.Pow(arcValue, IntDict[direction]));
        //Find Value for Movement Direction
        directionVector = VectorDict[direction];
        invertedDirection = new Vector2(directionVector.y, directionVector.x);
        //Find the Halfway time
        half = duration / 2;
        time = 0;

        //Placeholder Variables
        startPoint = transform.position;
        oldpos = startPoint;
        newPos = startPoint;
        //StartCoroutine(ArcMove(time, direction, arcDirection));
    }

    Vector2 startPoint;
    Vector2 oldpos;
    Vector2 newPos;
    float duration;
    public void Update() {
        if (time < duration * duraScale) {
            //Declare the Position Vectors
            Vector2 xPos;
            Vector2 yPos;

            //1st Half of path
            if (time <= half) {
                float x = time / half;
                float xDialate = DialateTime(x);                
                float y = ac.Evaluate(xDialate);

                xPos = directionVector * xDialate;
                yPos = (invertedDirection * y);
            }  
            //Straight After Path
            else if (time > duration) {
                float x = (duration - time) / half;
                float y = -ac.Evaluate(0);
                xPos = directionVector * x;
                yPos = (invertedDirection * y);
            }
            //2nd Half of Path
            else {
                float x = (duration - time) / half;
                float xDialate = DialateTime(x);
                float y = -ac.Evaluate(xDialate);

                xPos = directionVector * xDialate;
                yPos = invertedDirection * y;
            }

            newPos = startPoint + (ArcDistanceScale * xPos) + (ArcWidthScale * arcValue) * (yPos - invertedDirection);
            Debug.DrawLine(oldpos, newPos, Color.green, 10000);
            oldpos = newPos;
            RB_.MovePosition(newPos);
            time += Time.deltaTime;
        } else {
            Debug.LogWarning("Exited");
            Destroy(this.gameObject);
        }

    }

    private float DialateTime(float x) {
        float ret = 1 - Mathf.Pow(x - 1, 2f);
        return Mathf.Abs(ret);
    }

    IEnumerator ArcMove(float duration, Directions direction, ArcDirections arcDirection) {
        //Find Value for Direction of movement
        int arcValue = ArcValueDict[arcDirection];
        //Find Value for Movement Direction
        Vector2 directionVector = VectorDict[direction];
        Vector2 invertedDirection = new Vector2(directionVector.y, directionVector.x);
        //Find the Halfway time
        float half = duration / 2;
        float time = 0;
        //Placeholder Variables
        Vector2 startPoint = transform.position;
        Vector2 oldpos = startPoint;
        Vector2 newPos = startPoint;

        RB_ = GetComponent<Rigidbody2D>();

        while (time < duration * duraScale) {
            //Declare the Position Vectors
            Vector2 xPos;
            Vector2 yPos;

            //1st Half of path
            if (time <= half) {
                float x = time / half;
                float y = ac.Evaluate(x);
                xPos = directionVector * x;
                yPos = (invertedDirection * y);
            }
            //Straight After Path
            else if (time > duration) {
                float x = (duration - time) / half;
                float y = -ac.Evaluate(0);
                xPos = directionVector * x;
                yPos = (invertedDirection * y);
            }
            //2nd Half of Path
            else {
                float x = (duration - time) / half;
                float y = -ac.Evaluate(x);
                xPos = directionVector * x;
                yPos = invertedDirection * y;
            }

            newPos = startPoint + (ArcDistanceScale * xPos) + (ArcWidthScale * arcValue) * (yPos - invertedDirection);
            Debug.DrawLine(oldpos, newPos, Color.green, 10000);
            oldpos = newPos;
            Debug.Log(newPos);
            RB_.MovePosition(newPos);
            time += Time.deltaTime;
            yield return null;
        }
        Debug.LogWarning("Exited");
        Destroy(this.gameObject);
    }
    int[] LayerArray = new int[] {0, 6, 7, 8 };
    //public Collider2D HitCollider = null;
    private void OnTriggerEnter2D(Collider2D collision) {
        string msg = "";// ("Touched: " + collision.transform.name);
        bool b1 = !collision.transform.CompareTag("Player");
        bool b2 = LayerArray.Contains(collision.gameObject.layer);
        bool b3 = !collision.isTrigger;
        msg += b1 + " " + b2 + " ";//+ " " + b3;
        if (b1 && b2) {
            HitCollider = collision;
            CurrentPosition = transform.position;
            msg += collision.transform.position - transform.position;
            Destroy(this.gameObject, 2 * Time.deltaTime);
        } else {

        }
        Debug.Log(msg);
    }

    public void TakeDamage(float damage, Elements damageType, SpellTemplates damageSource = SpellTemplates.NULL) {
        SpellRenderer.Instance.CreateBurstFX(transform.position, ColourDict[damageType]);
        Destroy(this.gameObject);
    }

    public Collider2D HitCollider;
    public Vector3 CurrentPosition;
}
