using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarPhysics : MonoBehaviour {

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public Vector2 steeringSpeed;
    public Vector2 steeringAngle;

    public int player;
    public GameObject tp;
    Rigidbody rb;
    public float maxSpeed;
    public float reverseMaxSpeed;

    public Text carText;

    float currentSpeed = 0;
    Vector2 oldDirection;
    float upsideDownCounter;
    float maxRPM;

    private float interp(float x, float x0, float x1, float y0, float y1)
    {
        return y0 + (y1 - y0) * (x - x0) / (x1 - x0);
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = tp.transform.localPosition;

        oldDirection = new Vector2();
        //maxRPM = wheelColliders
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        currentSpeed = Vector3.Dot(rb.velocity, transform.forward) * 3.6f;
        carText.text = "Speed: " + currentSpeed.ToString("0.0");
        float motor = 0;
        float brake = 0;
        float steering = 0;

        Vector2 controlDirection = new Vector2(Input.GetAxis("Horizontal" + player.ToString()), 
            Input.GetAxis("Vertical" + player.ToString()));
        if (controlDirection.sqrMagnitude == 0f)
        {
            controlDirection = oldDirection;
        }
        oldDirection = controlDirection;

        float wantedAngle = Mathf.Rad2Deg * Mathf.Atan2(controlDirection.x, controlDirection.y);
        wantedAngle = Mathf.Repeat(wantedAngle, 360);

        float carAngle = Quaternion.LookRotation(transform.forward).eulerAngles.y;
        carText.text += ", Car angle " + carAngle.ToString("0") + ", Angle: " + wantedAngle.ToString("0");

        float angleRelativeCar = Mathf.DeltaAngle(0, Mathf.Repeat(wantedAngle - carAngle, 360));

        // steering of wheels

        if (Mathf.Abs(angleRelativeCar) > 170)
        {
            steering = 0;
        }
        else {
            steering = Mathf.MoveTowardsAngle(0, angleRelativeCar,
                Mathf.Lerp(steeringAngle[0], steeringAngle[1], (currentSpeed - steeringSpeed[0]) / (steeringSpeed[1] - steeringSpeed[0]))
                );
        }

        float throttle = Mathf.Min(controlDirection.magnitude, 1f);

        if(Mathf.Abs(angleRelativeCar) > 150)
        {
            if(currentSpeed > 0f)
            {
                brake = throttle * 0.5f * maxMotorTorque;
            } else
            {
                motor = -0.5f * throttle * maxMotorTorque;
            }
        }
        else if(Mathf.Abs(angleRelativeCar) > 135)
        {
            if(currentSpeed > maxSpeed * 0.25f)
            {
                brake = throttle * 0.2f * maxMotorTorque;
            } else
            {
                motor = throttle * 0.5f * maxMotorTorque;
            }
        } else if (Mathf.Abs(angleRelativeCar) > 80)
        {
            if (currentSpeed < maxSpeed * 0.5f)
            {
                motor = throttle * 0.5f * maxMotorTorque;
            }
        } else if (Mathf.Abs(angleRelativeCar) > 15)
        {
            motor = throttle * 0.5f * maxMotorTorque;
        } else
        {
            motor = throttle * maxMotorTorque;
        }

        if (currentSpeed > maxSpeed) motor = 0f;


        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                
                axleInfo.rightWheel.motorTorque = motor;
                if (Mathf.Abs(axleInfo.leftWheel.rpm) > 1000)
                    axleInfo.leftWheel.motorTorque = 0;
                if (Mathf.Abs(axleInfo.rightWheel.rpm) > 1000)
                    axleInfo.rightWheel.motorTorque = 0;
            }
            
            axleInfo.leftWheel.brakeTorque = brake;
            axleInfo.rightWheel.brakeTorque = brake;
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        
    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
    public bool front;
}
