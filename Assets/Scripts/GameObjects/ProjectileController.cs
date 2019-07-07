using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float _speedMetersPerSec;
    private Vector3 _previousDirection;

    public EnemyController enemyTarget;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = enemyTarget.transform.position - transform.position;
        _previousDirection = direction.normalized;

        _speedMetersPerSec = direction.magnitude / (float)BeatEngine.instance.RemainingTimeUntilNextBeatSec();
    }

    // Update is called once per frame
    void Update()
    {
        // if enemy target is null and we have already fired
        // just let it go out of the camera view and destroy it
        if (enemyTarget == null)
        {
            transform.position = transform.position + _previousDirection * (_speedMetersPerSec * Time.deltaTime);

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();

            if (!renderer.isVisible)
            {
                Destroy(gameObject);
            }

            return;
        }

        // CHECK ME: Updating the direction each time ensures us
        // we will hit the target but can lead to a noticeable
        // curve in the projectile's trajectory
        Vector3 direction = enemyTarget.transform.position - transform.position;
        direction.Normalize();

        _previousDirection = direction;

        transform.position = transform.position + direction * (_speedMetersPerSec * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController enemy = other.collider.GetComponent<EnemyController>();

        if (enemy == null)
        {
            return;
        }

        if (enemy != enemyTarget)
        {
            return;
        }

        DealDamage(enemy);
    }

    void DealDamage(EnemyController enemy)
    {
        enemy.TakeDamage(damage);

        Destroy(gameObject);
    }
}
