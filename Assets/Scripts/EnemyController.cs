using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IBeatActor
{

    private Vector3 _nextPosition;
    private float _smoothTime = 1.0f;
    private Vector3 _velocity;
    private int _numberOfBeatsWaiting;

    public ResourcesController resourcesController;
    public EnemyRoute route;
    public int life;
    public int damage;
    public int reward;
    public int speedBeatPerUnit;

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

        _nextPosition = Vector3.zero;

        _numberOfBeatsWaiting = speedBeatPerUnit;

        BeatEngine.BeatEvent += OnBeat;
    }

	// Update is called once per frame
	void Update ()
    {
        if (route == null)
        {
            return;
        }

        if (_nextPosition == Vector3.zero)
        {
            return;
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
            resourcesController.resourcesNumber += reward;

            Destroy(this.gameObject);
        }
    }

    public void OnBeat()
    {
        if (route == null)
        {
            return;
        }

        if (_numberOfBeatsWaiting < (speedBeatPerUnit - 1))
        {
            _numberOfBeatsWaiting++;

            return;
        }

        _numberOfBeatsWaiting = 0;

        _nextPosition = route.Next();

        if (_nextPosition == Vector3.zero)
        {
            // destination reached
            return;
        }

        _smoothTime = (float)(BeatEngine.instance.RemainingTimeUntilNextBeatSec() / 2.0);
    }

    void OnDestroy()
    {
        BeatEngine.BeatEvent -= OnBeat;
    }
}
