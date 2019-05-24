using System.Collections.Generic;
using UnityEngine;

public class EnemyRoute
{
    private List<Vector3> _positions = new List<Vector3>();
    private int _currentIndex = 0;

    public EnemyRoute()
    {

    }

    public EnemyRoute(EnemyRoute toCopy)
    {
        foreach (Vector3 position in toCopy._positions)
        {
            _positions.Add(new Vector3(position.x, position.y, position.z));
        }
    }

    public void Set(List<Vector3> positions)
    {
        _positions = positions;
    }

    public Vector3 Next()
    {
        if (_currentIndex == _positions.Count)
        {
            return Vector3.zero;
        }

        return _positions[_currentIndex++];
    }

    public void Reset()
    {
        _currentIndex = 0;
    }

    public int Length()
    {
        return _positions.Count;
    }

    public int CurrentPosition()
    {
        return _currentIndex + 1;
    }
}
