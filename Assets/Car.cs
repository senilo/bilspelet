﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using DG.Tweening;

public class Car : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 100, NavMesh.AllAreas);
        transform.position = hit.position;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = transform.position;
        newPos.x += Input.GetAxis("Horizontal") * speed;
        newPos.z += Input.GetAxis("Vertical") * speed;

        NavMeshHit hit;
        NavMesh.SamplePosition(newPos, out hit, 100, NavMesh.AllAreas);
        Vector3 diff = hit.position - transform.position;
        if (diff.magnitude > 0.1f)
        {
            var direction = Quaternion.LookRotation(diff).eulerAngles;
            transform.DORotate(direction, 0.5f);
        }
        transform.position = hit.position;
        
    }
}
