using System.Collections.Generic;
using UnityEngine;

public class DefenseController : MonoBehaviour, IBeatActor
{
    public ResourcesController resourcesController;
    public GameObject projectileGameObject;
    public GameObject upgradedDefensePrefab;
    public GameObject downgradedDefensePrefab;
    public GameObject inputFeedbackTextPrefab;
    public GameObject defenseSpawnerPrefab;
    public int projectilePower;
    public int attackCooldownBeat;
    public int price;

    private BeatPattern _upgradeBeatPattern = new BeatPattern();
    private BeatPattern _downgradeBeatPattern = new BeatPattern();
    private BeatPattern _sellLeftBeatPattern = new BeatPattern();
    private BeatPattern _sellRightBeatPattern = new BeatPattern();
    private int _cooldownCounterBeat;
    private int _currentBeatId;
    private List<EnemyController> _enemiesInRange = new List<EnemyController>();
    private InputFeedbackTextController _inputFeedbackTextController;
    private BeatPatternButton _beatPatternButton;

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

        defenseSpawnerPrefab.GetComponent<DefenseSpawner>().resourcesController = resourcesController;

        _beatPatternButton = GetComponent<BeatPatternButton>();

        _upgradeBeatPattern.pattern.Add(BeatPattern.Input.Tap);
        _upgradeBeatPattern.pattern.Add(BeatPattern.Input.SlideUp);

        _downgradeBeatPattern.pattern.Add(BeatPattern.Input.Tap);
        _downgradeBeatPattern.pattern.Add(BeatPattern.Input.SlideDown);

        _sellLeftBeatPattern.pattern.Add(BeatPattern.Input.Tap);
        _sellLeftBeatPattern.pattern.Add(BeatPattern.Input.SlideLeft);

        _sellRightBeatPattern.pattern.Add(BeatPattern.Input.Tap);
        _sellRightBeatPattern.pattern.Add(BeatPattern.Input.SlideRight);

        _beatPatternButton.AddPattern(_upgradeBeatPattern, OnUpgradeBeatPatternResolved, OnBeatPatternInput);
        _beatPatternButton.AddPattern(_downgradeBeatPattern, OnDowngradeBeatPatternResolved, OnBeatPatternInput);
        _beatPatternButton.AddPattern(_sellLeftBeatPattern, OnSellBeatPatternResolved, OnBeatPatternInput);
        _beatPatternButton.AddPattern(_sellRightBeatPattern, OnSellBeatPatternResolved, OnBeatPatternInput);

        GameObject inputFeedbackTextGameObjectInstance = Instantiate(inputFeedbackTextPrefab,
            transform.position + new Vector3(0.0f,1.0f, 0.0f),
            Quaternion.identity,
            GameController.worldSpaceCanvasInstance.transform);

        _inputFeedbackTextController = inputFeedbackTextGameObjectInstance.GetComponent<InputFeedbackTextController>();

        BeatEngine.BeatEvent += OnBeat;
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

    public void OnBeat()
    {
        _cooldownCounterBeat++;

        FireIfCooledDown();
    }

    void OnDestroy()
    {
        BeatEngine.BeatEvent -= OnBeat;

        _beatPatternButton.RemovePattern(_upgradeBeatPattern);
        _beatPatternButton.RemovePattern(_downgradeBeatPattern);
    }

    void OnUpgradeBeatPatternResolved()
    {
        if (upgradedDefensePrefab != null)
        {
            DefenseController upgradedDefenseController = upgradedDefensePrefab.GetComponent<DefenseController>();
            int priceForUpgrade = upgradedDefenseController.price - price;
            if (resourcesController.resourcesNumber >= priceForUpgrade)
            {
                resourcesController.resourcesNumber -= priceForUpgrade;

                Instantiate(upgradedDefensePrefab, transform.position, Quaternion.identity);

                Destroy(gameObject);

                _inputFeedbackTextController.ShowFeedback("Upgrade!");
            }
            else
            {
                _inputFeedbackTextController.ShowFeedback("Not enough resources!");
            }
        }
        else
        {
            _inputFeedbackTextController.ShowFeedback("Max level reached!");
        }
    }

    void OnDowngradeBeatPatternResolved()
    {
        if (downgradedDefensePrefab != null)
        {
            DefenseController downgradedDefenseController = downgradedDefensePrefab.GetComponent<DefenseController>();

            int earnedResourcesForDowngrade = (price - downgradedDefenseController.price) / 2;
            resourcesController.resourcesNumber += downgradedDefenseController.price;

            Instantiate(downgradedDefensePrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);

            _inputFeedbackTextController.ShowFeedback("Downgrade!");
        }
        else
        {
            _inputFeedbackTextController.ShowFeedback("Min level reached!");
        }
    }

    void OnSellBeatPatternResolved()
    {
        int earnedResourcesForSelling = price / 2;
        resourcesController.resourcesNumber += earnedResourcesForSelling;

        Instantiate(defenseSpawnerPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);

        _inputFeedbackTextController.ShowFeedback("Sold!");
    }

    void OnBeatPatternInput(BeatPatternResolver.ReturnType returnType)
    {
        _inputFeedbackTextController.ShowFeedback(BeatPatternResolver.EnumToString(returnType));
    }
}
