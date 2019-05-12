using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElementType : MonoBehaviour {

    public string type = "Fire";

    private void Start()
    {
        Destroy(gameObject,10f);
    }
}
