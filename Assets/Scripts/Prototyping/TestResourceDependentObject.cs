﻿using UnityEngine;
using System.Collections;
using System;

public class TestResourceDependentObject : ResourceDependentObject
{
    protected override void CheckResourceStoreStatus()
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
	}

    //  Testing!
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Trigger Entered!");
        ResourceObject res = col.GetComponent<ResourceObject>();

        if(res != null)
        {
            Consume(res);
        }
    }
}
