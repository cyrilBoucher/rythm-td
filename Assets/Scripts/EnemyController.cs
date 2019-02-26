using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private Vector3 nextPosition;
    private float smoothTime = 1.0f;
    private int currentBeatId = 0;
    private Vector3 velocity;
    private BoxCollider2D _boundingBox;
    public int life;
    private Vector3 _spawnPosition;

	// Use this for initialization
	void Start ()
    {
        _spawnPosition = transform.position;
    }

	// Update is called once per frame
	void Update ()
    {
        int beatEngineId = BeatEngine.BeatId();
        if (beatEngineId != currentBeatId)
        {
            Vector3 direction = new Vector3(1.0f, 0.0f, 0.0f);
            nextPosition = transform.position + direction;
            smoothTime = BeatEngine.RemainingTimeUntilNextBeatSec() / 2.0f;
            currentBeatId = beatEngineId;
        }

        transform.position = Vector3.SmoothDamp(transform.position, nextPosition, ref velocity, smoothTime);
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
