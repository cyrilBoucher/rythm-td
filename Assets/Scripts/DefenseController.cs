using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseController : MonoBehaviour
{
    public GameObject projectileGameObject;
    public int attackCooldownBeat;
    public int price;

    private int _cooldownCounterBeat;
    private int _currentBeatId;
    private int _enemiesInRange;

    // Start is called before the first frame update
    void Start()
    {
        _cooldownCounterBeat = attackCooldownBeat;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();

        if (enemy == null)
        {
            return;
        }

        if (_enemiesInRange == 0)
        {
            _currentBeatId = BeatEngine.BeatId();
        }

        _enemiesInRange++;

        FireIfCooledDown(enemy);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();

        if (enemy == null)
        {
            return;
        }

        if (_currentBeatId != BeatEngine.BeatId())
        {
            _cooldownCounterBeat++;

            _currentBeatId = BeatEngine.BeatId();
        }

        FireIfCooledDown(enemy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();

        if (enemy == null)
        {
            return;
        }

        _enemiesInRange--;
    }

    void FireIfCooledDown(EnemyController enemy)
    {
        if (_cooldownCounterBeat < attackCooldownBeat)
        {
            return;
        }

        FireAt(enemy);

        _cooldownCounterBeat = 0;
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
