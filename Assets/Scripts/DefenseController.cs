using System.Collections.Generic;
using UnityEngine;

public class DefenseController : MonoBehaviour, IBeatActor
{
    public ResourcesController resourcesController;
    public GameObject projectileGameObject;
    public GameObject upgradedDefensePrefab;
    public GameObject downgradedDefensePrefab;
    public GameObject inputFeedbackTextPrefab;
    public GameObject worldSpaceCanvasGameObject;
    public int projectilePower;
    public int attackCooldownBeat;
    public int price;

    private BeatPatternResolver _upgradeBeatPatternResolver = new BeatPatternResolver();
    private BeatPatternResolver _downgradeBeatPatternResolver = new BeatPatternResolver();
    private int _cooldownCounterBeat;
    private int _currentBeatId;
    private List<EnemyController> _enemiesInRange = new List<EnemyController>();
    private InputFeedbackTextController _inputFeedbackTextController;

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

        GameObject inputFeedbackTextGameObjectInstance = Instantiate(inputFeedbackTextPrefab,
            transform.position + new Vector3(0.0f,1.0f, 0.0f),
            Quaternion.identity,
            worldSpaceCanvasGameObject.transform);

        _inputFeedbackTextController = inputFeedbackTextGameObjectInstance.GetComponent<InputFeedbackTextController>();

        BeatEngine.BeatEvent += OnBeat;
    }

    // Update is called once per frame
    void Update()
    {
        BeatPattern.Input input = InputDetector.CheckForInput(GetComponent<BoxCollider2D>());

        BeatPatternResolver.ReturnType result = _upgradeBeatPatternResolver.Run2(input);

        if (result == BeatPatternResolver.ReturnType.Validated)
        {
            if (upgradedDefensePrefab != null)
            {
                DefenseController upgradedDefenseController = upgradedDefensePrefab.GetComponent<DefenseController>();
                if (resourcesController.resourcesNumber >= upgradedDefenseController.price)
                {
                    resourcesController.resourcesNumber -= upgradedDefenseController.price;

                    upgradedDefenseController.resourcesController = resourcesController;
                    upgradedDefenseController.worldSpaceCanvasGameObject = worldSpaceCanvasGameObject;
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

            return;
        }

        result = _downgradeBeatPatternResolver.Run2(input);

        if (result == BeatPatternResolver.ReturnType.Validated)
        {
            if (downgradedDefensePrefab != null)
            {
                DefenseController downgradedDefenseController = downgradedDefensePrefab.GetComponent<DefenseController>();

                resourcesController.resourcesNumber += downgradedDefenseController.price;

                downgradedDefenseController.resourcesController = resourcesController;
                downgradedDefenseController.worldSpaceCanvasGameObject = worldSpaceCanvasGameObject;
                Instantiate(downgradedDefenseController, transform.position, Quaternion.identity);

                Destroy(gameObject);

                _inputFeedbackTextController.ShowFeedback("Downgrade!");
            }
            else
            {
                _inputFeedbackTextController.ShowFeedback("Min level reached!");
            }

            return;
        }

        if (result != BeatPatternResolver.ReturnType.Waiting)
        {
            _inputFeedbackTextController.ShowFeedback(BeatPatternResolver.EnumToString(result));
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

    public void OnBeat()
    {
        _cooldownCounterBeat++;

        FireIfCooledDown();
    }

    void OnDestroy()
    {
        BeatEngine.BeatEvent += OnBeat;
    }
}
