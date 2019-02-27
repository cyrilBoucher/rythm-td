using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoute
{
    private List<Vector3> _positions = new List<Vector3>();
    private int _currentIndex = 0;

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
}
