using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float _speedMetersPerSec;

    public EnemyController enemyTarget;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = enemyTarget.transform.position - transform.position;
        _speedMetersPerSec = direction.magnitude / BeatEngine.RemainingTimeUntilNextBeatSec();
    }

    // Update is called once per frame
    void Update()
    {
        // CHECK ME: Updating the direction each time ensures us
        // we will hit the target but can lead to a noticeable
        // curve in the projectile's trajectory
        Vector3 direction = enemyTarget.transform.position - transform.position;
        direction.Normalize();

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
