﻿using System;

public class ResourcesController
{
    public delegate void ResourcesNumberChangedAction();
    public static event ResourcesNumberChangedAction ResourcesNumberChanged;

    private static int _resourcesNumber;
    private static bool _initialized = false;

    public static void Initialize(int resourcesNumber)
    {
        if (_initialized)
        {
            return;
        }

        SetResourcesNumber(resourcesNumber);

        _initialized = true;
    }

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

        FireResourcesNumberChangedEvent();
    }

    public static void AddResources(int resourcesNumber)
    {
        if (resourcesNumber < 0)
        {
            throw new ArgumentException("Resources number cannot be negative");
        }

        _resourcesNumber += resourcesNumber;

        FireResourcesNumberChangedEvent();
    }

    public static void TakeResources(int resourcesNumber)
    {
        if (resourcesNumber < 0)
        {
            throw new ArgumentException("Resources number cannot be negative");
        }

        if (resourcesNumber > _resourcesNumber)
        {
            throw new NotEnoughResourcesException(string.Format("Cannot take {0} as there is only {1} remaining", resourcesNumber, _resourcesNumber));
        }

        _resourcesNumber -= resourcesNumber;

        FireResourcesNumberChangedEvent();
    }

    private static void FireResourcesNumberChangedEvent()
    {
        if (ResourcesNumberChanged != null)
        {
            ResourcesNumberChanged();
        }
    }
}
