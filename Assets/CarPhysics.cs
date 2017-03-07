using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarPhysics : MonoBehaviour {

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public Vector2 steeringSpeed;
    public Vector2 steeringAngle;


    public GameObject tp;
    Rigidbody rb;
    public float maxSpeed;
    public float reverseMaxSpeed;

    public Text carText;

    float currentSpeed = 0;
    float engineRPM = 0;

    private float interp(float x, float x0, float x1, float y0, float y1)
    {
        return y0 + (y1 - y0) * (x - x0) / (x1 - x0);
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = tp.transform.localPosition;

        var wheelColliders = GetComponentsInChildren<WheelCollider>();


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

        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.y);
        angle = Mathf.Repeat(angle, 360);

        var throttle = Input.GetAxis("Vertical");
        throttle = 0.1f*Mathf.Min(direction.sqrMagnitude, 1f);

        float carAngle = Quaternion.LookRotation(transform.forward).eulerAngles.y;
        carText.text += ", Car angle " + carAngle.ToString("0") + ", Angle: " + angle.ToString("0");

        float steering = steeringAngle[0] * Input.GetAxis("Horizontal");
        steering = Mathf.Repeat(angle - carAngle, 360);


        if (throttle < 0 && currentSpeed > 1f || (throttle > 0 && currentSpeed < -1f))
        {
            Debug.Log("Brake");
                motor = 0;
                brake = maxMotorTorque;
        }
        else
        {
            brake = 0;
            motor = throttle * maxMotorTorque;
        }
        
        if(currentSpeed > maxSpeed || currentSpeed < -reverseMaxSpeed)
        {
            motor = 0;
        }

        

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
            }
            axleInfo.leftWheel.brakeTorque = brake;
            axleInfo.rightWheel.brakeTorque = brake;
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
    public void Update()
    {
        ;

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
