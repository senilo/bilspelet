using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PointAtTarget : MonoBehaviour {
    Transform car;
    BuildingType target;
    float activateTime;
	// Use this for initialization
	void Start () {
       car = transform.parent;
        
	}
	
	// Update is called once per frame
	void Update () {
        var currentTarget = Game.instance.nextTarget;
        if (currentTarget != target)
        {
            target = currentTarget;
            transform.GetChild(0).gameObject.SetActive(false);
            activateTime = Time.time + 1f;
        } else
        {
            if(!transform.GetChild(0).gameObject.activeInHierarchy && Time.time > activateTime)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.DOPunchPosition(transform.forward, 0.3f);
            }
        }
        transform.DORotate(Quaternion.LookRotation(Game.instance.getNextTarget().transform.position - transform.position).eulerAngles, 0.5f);
	}
}
