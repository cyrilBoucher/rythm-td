using UnityEngine;

public class BaseController : MonoBehaviour
{
    public delegate void OnDestroyedAction();
    static public OnDestroyedAction DestroyedEvent;

    public int maxLife;
    public GameObject lifeBarPrefab;

    private int _life;
    private GameObject _lifeBarInstance;
    private LifeBarController _lifeBarController;

    // Start is called before the first frame update
    void Start()
    {
        lifeBarPrefab.GetComponent<LifeBarController>().maxLife = maxLife;
        _lifeBarInstance = Instantiate(lifeBarPrefab, new Vector3(transform.position.x, transform.position.y - 0.75f, transform.position.z), Quaternion.identity, GameController.worldSpaceCanvasInstance.transform);
        _lifeBarController = _lifeBarInstance.GetComponent<LifeBarController>();

        _life = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        _life -= damage;

        _lifeBarController.TakeDamage(damage);

        if (_life <= 0)
        {
            DestroyedEvent();

            Destroy(gameObject);
            Destroy(_lifeBarInstance);
        }
    }
}
