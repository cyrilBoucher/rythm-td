using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseController : MonoBehaviour
{
    public GameObject projectileGameObject;
    private float _cooldownCounterSec;
    public float attackCooldownSec;

    // Start is called before the first frame update
    void Start()
    {
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
        ProjectileController projectileController = projectileGameObject.GetComponent<ProjectileController>();

        if (projectileController == null)
        {
            Debug.LogError("Could not find ProjectileController script from prpjectile GameObject");

            return;
        }

        Vector3 direction = enemy.transform.position - transform.position;

        projectileController.direction = direction;

        Instantiate(projectileGameObject, transform.position, Quaternion.identity);
    }
}
