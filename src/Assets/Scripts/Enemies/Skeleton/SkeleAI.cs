using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEditor;
using UnityEngine;

using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class SkeleAI : MonoBehaviour
{
    Rigidbody2D rb2d;

    enum ValidStates
    {
        Idle,
        Targeting
    }

    ValidStates NextState;
    ValidStates CurrentState;

    Coroutine ActiveFunction;


    public float DetectionRange;
    public float shuffle_delay = 0.5f;
    public float shuffle_time = 5;

    private GameObject PlayerRef;

    private float MoveSpeed { get => GetComponent<iCreatureInterface>().EntitySpeed_; }

    Vector2 LastViewedPos;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        PlayerRef = GameObject.FindGameObjectWithTag("Player");
        NextState = ValidStates.Idle;
    }

    private void Update()
    {
        if (Vector2.Distance(PlayerRef.transform.position, gameObject.transform.position) <= DetectionRange)
        {
            RaycastHit2D[] hit2d_a = Physics2D.RaycastAll(gameObject.transform.position, PlayerRef.transform.position - gameObject.transform.position, DetectionRange);
            hit2d_a = Array.FindAll(hit2d_a, (h) => { return h.collider.gameObject.layer == LayerMask.NameToLayer("WALL") || h.collider.gameObject == PlayerRef; });
            Array.Sort<RaycastHit2D>(hit2d_a, (ray1, ray2) => { return ray1.distance.CompareTo(ray2.distance); });

            bool WallFirst = false;
            RaycastHit2D hit2d = new RaycastHit2D();
            for (int i = 0; i < hit2d_a.Length; i++)
            {
                if (hit2d_a[i].collider.gameObject.layer == LayerMask.NameToLayer("WALL"))
                {
                    //Debug.Log("Wall first");
                    WallFirst = true;
                    break;
                }
                else if (hit2d_a[i].collider.gameObject == PlayerRef)
                {
                    //Debug.Log("Player");
                    hit2d = hit2d_a[i];
                }
                else if (hit2d_a[i].collider.gameObject == gameObject)
                {
                    //this is just to ignore self
                }
            }

            if (!WallFirst && hit2d.collider != null)
            {
                Debug.DrawRay(gameObject.transform.position, PlayerRef.transform.position - gameObject.transform.position);
                if (hit2d.collider != null && hit2d.collider.gameObject == PlayerRef)
                {
                    if (CurrentState != ValidStates.Targeting)
                    {
                        NextState = ValidStates.Targeting;
                    }
                    LastViewedPos = hit2d.collider.gameObject.transform.position;
                }
            }
        }
        else
        {
            if (CurrentState != ValidStates.Idle)
            {
                NextState = ValidStates.Idle;
            }
        }


        if (CurrentState != NextState)
        {
            CurrentState = NextState;
            if (ActiveFunction != null)
                StopCoroutine(ActiveFunction);

            if (CurrentState == ValidStates.Idle)
            {
                ActiveFunction = StartCoroutine(Idle());
            }
            if (CurrentState == ValidStates.Targeting)
            {
                ActiveFunction = StartCoroutine(Chasing());
            }
        }
    }

    public IEnumerator Chasing()
    {
        bool Move_ = true;
        Stopwatch stp = new Stopwatch();
        stp.Start();

        while (true)
        {
            if (Move_)
            {
                gameObject.transform.position += ((Vector3)LastViewedPos - gameObject.transform.position) * MoveSpeed * Time.deltaTime;
                if (stp.ElapsedMilliseconds >= shuffle_time)
                    Move_ = false;

                yield return new WaitForEndOfFrame();
            }
            else
            {

                yield return new WaitForSeconds(shuffle_delay);
                Move_ = true;
                stp.Reset();
                stp.Start();
            }
        }

        //while (!crw.IsDone)
        //{
        //    Corontine_wrap crw = new Corontine_wrap(this, MoveMore(gameObject.transform.position));
        //    gameObject.transform.position += ((Vector3)LastViewedPos - gameObject.transform.position) * MoveSpeed * Time.deltaTime;

            
        //    yield return new WaitUntil(() => crw.IsDone);

        //    yield return new WaitForSeconds(shuffle_delay);
        //    //yield return new WaitForFixedUpdate();
        //}
    }

    public IEnumerator MoveMore(Vector3 pos)
    {
        pos += ((Vector3)LastViewedPos - gameObject.transform.position) * MoveSpeed * Time.deltaTime;
        yield return pos;
    }


    public IEnumerator MoveDelay()
    {
        yield return new WaitForSecondsRealtime(shuffle_delay);
        yield return "e_";
    }


    public IEnumerator Idle()
    {
        yield return new WaitForEndOfFrame();
    }
}
