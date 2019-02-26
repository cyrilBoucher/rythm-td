using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private BoxCollider2D _boundingBox;
    private Sprite _sprite;
    public float speedMetersPerSec;
    public Vector3 direction;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        _boundingBox = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>().sprite;
        _boundingBox.size = _sprite.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        direction.Normalize();

        transform.position = transform.position + direction * (speedMetersPerSec * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController enemy = other.collider.GetComponent<EnemyController>();

        if (enemy == null)
        {
            return;
        }

        DealDamage(enemy);
    }

    void DealDamage(EnemyController enemy)
    {
        enemy.TakeDamage(damage);

        Destroy(this.gameObject);
    }
}
