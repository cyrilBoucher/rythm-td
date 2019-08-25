using System;

public class ResourcesController
{
    public delegate void ResourcesNumberChangedAction();
    public static event ResourcesNumberChangedAction ResourcesNumberChanged;

    public delegate void ResourcesTakenAction(int resourcesTaken);
    public static event ResourcesTakenAction ResourcesTaken;

    public int resourcesNumber { get; private set; }

    public static ResourcesController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ResourcesController();
            }

            return _instance;
        }
    }

    private static ResourcesController _instance;

    private bool _initialized = false;

    public void Initialize(int resourcesNumber)
    {
        if (_initialized)
        {
            return;
        }

        SetResourcesNumber(resourcesNumber);

        _initialized = true;
    }

    public static void Destroy()
    {
        _instance = null;
    }

    private void SetResourcesNumber(int resourcesNumber)
    {
        if (resourcesNumber < 0)
        {
            throw new ArgumentException("Resources number cannot be negative");
        }

        this.resourcesNumber = resourcesNumber;

        ResourcesNumberChanged?.Invoke();
    }

    public void AddResources(int resourcesNumber)
    {
        if (resourcesNumber < 0)
        {
            throw new ArgumentException("Resources number cannot be negative");
        }

        this.resourcesNumber += resourcesNumber;

        ResourcesNumberChanged?.Invoke();
    }

    public void TakeResources(int resourcesNumber)
    {
        if (resourcesNumber < 0)
        {
            throw new ArgumentException("Resources number cannot be negative");
        }

        if (resourcesNumber > this.resourcesNumber)
        {
            throw new NotEnoughResourcesException(string.Format("Cannot take {0} resources as there is only {1} remaining", resourcesNumber, this.resourcesNumber));
        }

        this.resourcesNumber -= resourcesNumber;

        ResourcesTaken?.Invoke(resourcesNumber);
        ResourcesNumberChanged?.Invoke();
    }
}
