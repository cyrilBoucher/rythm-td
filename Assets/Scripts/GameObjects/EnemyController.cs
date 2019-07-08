using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IBeatActor
{
    private Vector3 _nextPosition;
    private float _smoothTime = 1.0f;
    private Vector3 _velocity;
    private int _numberOfBeatsWaiting;
    private GameObject _lifeBarInstance;

    public delegate void OnDeathAction();
    static public event OnDeathAction DeathEvent;

    public GameObject lifeBarPrefab;
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

        LifeBarController controller = lifeBarPrefab.GetComponent<LifeBarController>();
        controller.maxLife = life;
        _lifeBarInstance = Instantiate(lifeBarPrefab, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity, GameController.worldSpaceCanvasInstance.transform);
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
        _lifeBarInstance.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
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

        Death();
    }

    public void TakeDamage(int damage)
    {
        life -= damage;

        _lifeBarInstance.GetComponent<LifeBarController>().TakeDamage(damage);

        if (life <= 0)
        {
            ResourcesController.AddResources(reward);

            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
        Destroy(_lifeBarInstance);

        DeathEvent();
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
