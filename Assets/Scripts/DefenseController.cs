using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseController : MonoBehaviour
{
    private CircleCollider2D _areaOfAttackCollider2D = new CircleCollider2D();
    public float attackRadius;
    public float attackCooldownSec;
    private float _cooldownCounterSec;

    // Start is called before the first frame update
    void Start()
    {
        _areaOfAttackCollider2D.isTrigger = true;
        _areaOfAttackCollider2D.radius = attackRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (_cooldownCounterSec < attackCooldownSec)
        {
            _cooldownCounterSec += Time.deltaTime;
        }
        else
        {
            _cooldownCounterSec = attackCooldownSec;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy == null)
        {
            return;
        }

        if (_cooldownCounterSec != attackCooldownSec)
        {
            return;
        }

        FireAt(enemy);
    }

    void FireAt(EnemyController enemy)
    {

    }
}
