using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject car;
    Vector3 diff;
	// Use this for initialization
	void Start () {
        diff = transform.position - car.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = car.transform.position + diff;
	}
}
