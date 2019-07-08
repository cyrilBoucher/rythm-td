using System;

public class ResourcesController
{
    private static int _resourcesNumber;

    public static int GetResourcesNumber()
    {
        return _resourcesNumber;
    }

    public static void SetResourcesNumber(int resourcesNumber)
    {
        if (resourcesNumber < 0)
        {
            throw new ArgumentException("Resources number cannot be negative");
        }

        _resourcesNumber = resourcesNumber;
    }

    public static void AddResources(int resourcesNumber)
    {
        if (resourcesNumber < 0)
        {
            throw new ArgumentException("Resources number cannot be negative");
        }

        _resourcesNumber += resourcesNumber;
    }

    public static void TakeResources(int resourcesNumber)
    {
        if (resourcesNumber < 0)
        {
            throw new ArgumentException("Resources number cannot be negative");
        }

        _resourcesNumber -= resourcesNumber;
    }
}
