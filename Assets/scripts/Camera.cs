using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    public GameObject followObject;
    // Use this for initialization
    Vector3 delta;
	void Start () {
        if(followObject != null)
            delta = transform.position - followObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (followObject != null)
            transform.position = followObject.transform.position + delta;
	}
}
