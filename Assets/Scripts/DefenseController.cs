using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseController : MonoBehaviour
{
    private CircleCollider2D _areaOfAttackCollider2D;
    public GameObject projectileGameObject;
    public float attackCooldownSec;
    private float _cooldownCounterSec;

    // Start is called before the first frame update
    void Start()
    {
        _areaOfAttackCollider2D = GetComponent<CircleCollider2D>();
        _cooldownCounterSec = attackCooldownSec;
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

        FireIfCooledDown(enemy);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy == null)
        {
            return;
        }

        FireIfCooledDown(enemy);
    }

    void FireIfCooledDown(EnemyController enemy)
    {
        if (_cooldownCounterSec != attackCooldownSec)
        {
            return;
        }

        FireAt(enemy);

        _cooldownCounterSec = 0.0f;
    }

    void FireAt(EnemyController enemy)
    {
        Debug.Log("Baboom bitch!");
    }
}
