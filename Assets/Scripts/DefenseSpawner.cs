using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DefenseSpawner : MonoBehaviour {

    private BeatPattern _beatPattern = new BeatPattern();
    private BeatPatternResolver _beatPatternResolver = new BeatPatternResolver();
    public GameObject defenseGameObject;

    // Use this for initialization
    void Start () {
        _beatPattern.Add(BeatPattern.Input.OnBeat);
        _beatPattern.Add(BeatPattern.Input.OnBeat);

        _beatPatternResolver.SetPattern(_beatPattern);
    }
	
	// Update is called once per frame
	void Update () {

        bool input = CheckForInput();

        BeatPatternResolver.ReturnType result = _beatPatternResolver.Run2(input);

        if (result != BeatPatternResolver.ReturnType.Waiting)
        {
            Debug.Log(_beatPatternResolver.EnumToString(result));
        }

        if (result == BeatPatternResolver.ReturnType.Validated)
        {
            Instantiate(defenseGameObject, transform.position, Quaternion.identity);
        }
    }

    bool CheckForInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldMousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldMousPos, -Vector2.up);

            if (hit.collider != null)
            {
                if (hit.transform.name == "DefenseSpawner")
                {
                    return true;
                }
            }
        }

        return false;
    }
}
