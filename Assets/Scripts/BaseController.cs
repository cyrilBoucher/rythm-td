﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public int life;

    // Start is called before the first frame update
    void Start()
    {
        if (Map.basePosition != null)
        {
            transform.position = Map.basePosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        life -= damage;

        if (life <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
