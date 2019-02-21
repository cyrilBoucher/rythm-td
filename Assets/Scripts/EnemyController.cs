﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private LinkedList<Vector3> positions = new LinkedList<Vector3>();
    private Vector3 nextPosition;
    private float smoothTime = 1.0f;
    private int currentBeatId = 0;
    private Vector3 velocity;
    public int life;
    private BoxCollider2D _boundingBox;

    bool isNextPositionReached()
    {
        return transform.position == nextPosition;
    }

	// Use this for initialization
	void Start () {
        _boundingBox = GetComponent<BoxCollider2D>();
       /* Vector3 newPosition = transform.position + new Vector3(1.0f, 0.0f, 0.0f);
        positions.AddLast(newPosition);
        newPosition += new Vector3(1.0f, 0.0f, 0.0f);
        positions.AddLast(newPosition);
        newPosition += new Vector3(0.0f, 1.0f, 0.0f);
        positions.AddLast(newPosition);

        nextPosition = positions.First.Value;*/
    }

	// Update is called once per frame
	void Update () {
        /* if (positions.Count == 0)
         {
             return;
         }

         int beatEngineId = BeatEngine.BeatId();
         if (beatEngineId != currentBeatId)
         {
             positions.RemoveFirst();

             if (positions.Count == 0)
             {
                 return;
             }

             nextPosition = positions.First.Value;
             smoothTime = BeatEngine.RemainingTimeUntilNextBeatSec() / 2.0f;
             print(string.Format("smooth time is {0}", smoothTime));
             currentBeatId = beatEngineId;

             print("changing position for " + nextPosition);
         }

         GetComponent<Transform>().position = Vector3.SmoothDamp(GetComponent<Transform>().position, nextPosition, ref velocity, smoothTime);*/

        int beatEngineId = BeatEngine.BeatId();
        if (beatEngineId != currentBeatId)
        {
            Vector3 direction = new Vector3(1.0f, 0.0f, 0.0f);
            nextPosition = GetComponent<Transform>().position + direction;
            smoothTime = BeatEngine.RemainingTimeUntilNextBeatSec() / 2.0f;
            //print(string.Format("smooth time is {0}", smoothTime));
            currentBeatId = beatEngineId;

            //print("changing position for " + nextPosition);
        }

        GetComponent<Transform>().position = Vector3.SmoothDamp(GetComponent<Transform>().position, nextPosition, ref velocity, smoothTime);
    }

    public void TakeDamage(int damage)
    {
        life -= damage;

        if (life <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
