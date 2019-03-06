using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private Vector3 _nextPosition;
    private float _smoothTime = 1.0f;
    private int _currentBeatId = 0;
    private Vector3 _velocity;
    public EnemyRoute route;
    public int life;
    public int damage;

    // Use this for initialization
    void Start ()
    {
        if (Map.enemyRoute != null)
        {
            route = new EnemyRoute(Map.enemyRoute);
        }

        if (route == null)
        {
            return;
        }

        transform.position = route.Next();

        _nextPosition = route.Next();
        _smoothTime = BeatEngine.RemainingTimeUntilNextBeatSec() / 2.0f;
        _currentBeatId = BeatEngine.BeatId();
    }

	// Update is called once per frame
	void Update ()
    {
        if (route == null)
        {
            return;
        }

        int beatEngineId = BeatEngine.BeatId();
        if (beatEngineId != _currentBeatId)
        {
            _nextPosition = route.Next();
            if (_nextPosition == Vector3.zero)
            {
                // destination reached
                return;
            }

            _smoothTime = BeatEngine.RemainingTimeUntilNextBeatSec() / 2.0f;
            _currentBeatId = beatEngineId;
        }

        transform.position = Vector3.SmoothDamp(transform.position, _nextPosition, ref _velocity, _smoothTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        BaseController playerBase = other.collider.GetComponent<BaseController>();

        if (playerBase == null)
        {
            return;
        }

        DealDamage(playerBase);
    }

    void DealDamage(BaseController playerBase)
    {
        playerBase.TakeDamage(damage);

        Destroy(this.gameObject);
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
