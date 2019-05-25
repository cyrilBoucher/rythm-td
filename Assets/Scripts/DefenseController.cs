using System.Collections.Generic;
using UnityEngine;

public class DefenseController : MonoBehaviour
{
    public ResourcesController resourcesController;
    public GameObject projectileGameObject;
    public GameObject upgradedDefensePrefab;
    public GameObject downgradedDefensePrefab;
    public int projectilePower;
    public int attackCooldownBeat;
    public int price;

    private BeatPatternResolver _upgradeBeatPatternResolver = new BeatPatternResolver();
    private BeatPatternResolver _downgradeBeatPatternResolver = new BeatPatternResolver();
    private int _cooldownCounterBeat;
    private int _currentBeatId;
    private List<EnemyController> _enemiesInRange = new List<EnemyController>();

    // Start is called before the first frame update
    void Start()
    {
        _cooldownCounterBeat = attackCooldownBeat;

        if (upgradedDefensePrefab != null)
        {
            upgradedDefensePrefab.GetComponent<DefenseController>().resourcesController = resourcesController;
        }

        if (downgradedDefensePrefab != null)
        {
            downgradedDefensePrefab.GetComponent<DefenseController>().resourcesController = resourcesController;
        }

        BeatPattern beatPattern = new BeatPattern();
        beatPattern.Add(BeatPattern.Input.Tap);
        beatPattern.Add(BeatPattern.Input.SlideUp);

        _upgradeBeatPatternResolver.SetPattern(beatPattern);

        beatPattern = new BeatPattern();
        beatPattern.Add(BeatPattern.Input.Tap);
        beatPattern.Add(BeatPattern.Input.SlideDown);

        _downgradeBeatPatternResolver.SetPattern(beatPattern);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentBeatId != BeatEngine.BeatId())
        {
            _currentBeatId = BeatEngine.BeatId();

            _cooldownCounterBeat++;

            FireIfCooledDown();
        }

        BeatPattern.Input input = InputDetector.CheckForInput(GetComponent<BoxCollider2D>());

        BeatPatternResolver.ReturnType result = _upgradeBeatPatternResolver.Run2(input);

        if (result == BeatPatternResolver.ReturnType.Validated)
        {
            if (upgradedDefensePrefab != null)
            {
                DefenseController upgradedDefenseController = upgradedDefensePrefab.GetComponent<DefenseController>();
                if (resourcesController.resourcesNumber >= upgradedDefenseController.price)
                {
                    Debug.Log("UPGRADE");

                    resourcesController.resourcesNumber -= upgradedDefenseController.price;

                    Instantiate(upgradedDefensePrefab, transform.position, Quaternion.identity);

                    Destroy(gameObject);
                }
            }
        }

        result = _downgradeBeatPatternResolver.Run2(input);

        if (result == BeatPatternResolver.ReturnType.Validated)
        {
            if (downgradedDefensePrefab != null)
            {
                DefenseController downgradedDefenseController = downgradedDefensePrefab.GetComponent<DefenseController>();

                Debug.Log("DOWNGRADE");

                resourcesController.resourcesNumber += downgradedDefenseController.price;

                Instantiate(downgradedDefenseController, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();

        if (enemy == null)
        {
            return;
        }

        _enemiesInRange.Add(enemy);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();

        if (enemy == null)
        {
            return;
        }

        _enemiesInRange.Remove(enemy);
    }

    void FireIfCooledDown()
    {
        if (_cooldownCounterBeat < attackCooldownBeat)
        {
            return;
        }

        // Find target closest to base
        EnemyController enemyCandidate = null;
        foreach(EnemyController enemy in _enemiesInRange)
        {
            if (enemyCandidate == null ||
                enemyCandidate.route.CurrentPosition() < enemy.route.CurrentPosition())
            {
                enemyCandidate = enemy;
            }
        }

        if (enemyCandidate == null)
        {
            return;
        }

        FireAt(enemyCandidate);

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

        projectileController.damage = projectilePower;
        projectileController.enemyTarget = enemy;

        Instantiate(projectileGameObject, transform.position, Quaternion.identity);
    }
}
