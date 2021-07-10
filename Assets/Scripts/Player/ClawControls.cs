using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawControls : MonoBehaviour
{
    public float clawOpenSpeed = 10f;
    public float clawLiftSpeed = 10f;
    public float armMinRotation = 0;
    public float armMaxRotation = 64f;
    public float clawMaxHeight;
    public float clawMinHeight;
    public GameObject chain;
    public GameObject armOne;
    public GameObject armTwo;
    public GameObject claw;
    public DistanceJoint2D distanceJoint;
    private LineRenderer _lineRenderer;

    bool _opening;
    bool _closing;
    bool _lifting;
    bool _dropping;
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.instance.player.alive)
        {
            if (_opening)
            {
                OpenClaw();
            }
            else if (_closing)
            {
                CloseClaw();
            } 
        }
    }

    private void Update()
    {
        if (GameManager.instance.player.alive)
        {
            if (_lifting)
            {
                LiftClaw();
            }
            else if (_dropping)
            {
                DropClaw();
            }
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, claw.transform.position); 
        }
    }

    public void OnOpenClaw()
    {
        if(_opening)
        {
            _opening = false;
        }
        else
        {
            _opening = true;
        }
    }

    public void OnCloseClaw()
    {
        if(_closing)
        {
            _closing = false;
        }
        else
        {
            _closing = true;
        }
    }

    public void OnLiftClaw()
    {
        if (_lifting)
        {
            _lifting = false;
        }
        else
        {
            _lifting = true;
        }
    }

    public void OnDropClaw()
    {
        if (_dropping)
        {
            _dropping = false;
        }
        else
        {
            _dropping = true;
        }
    }

    void OpenClaw()
    {
        armOne.transform.Rotate(new Vector3(0, 0, -clawOpenSpeed * Time.deltaTime));
        armTwo.transform.Rotate(new Vector3(0, 0, clawOpenSpeed * Time.deltaTime));
        if(armTwo.transform.localEulerAngles.z > armMaxRotation)
        {
            armOne.transform.localEulerAngles = new Vector3(0, 0, -armMaxRotation);
            armTwo.transform.localEulerAngles = new Vector3(0, 0, armMaxRotation);
        }
    }

    void CloseClaw()
    {
        armOne.transform.Rotate(new Vector3(0, 0, clawOpenSpeed * Time.deltaTime));
        armTwo.transform.Rotate(new Vector3(0, 0, -clawOpenSpeed * Time.deltaTime));
        if (armTwo.transform.localRotation.z < armMinRotation)
        {
            armOne.transform.localEulerAngles = new Vector3(0, 0, armMinRotation);
            armTwo.transform.localEulerAngles = new Vector3(0, 0, armMinRotation);
        }
    }

    void LiftClaw()
    {
        if (distanceJoint.distance < clawMinHeight)
        {
            //chain.transform.localPosition = new Vector3(0, clawMaxHeight);
            distanceJoint.distance = clawMinHeight;
        }
        else
        {
            //Vector2 newPos = new Vector2(0, clawLiftSpeed * Time.deltaTime);
            //chain.transform.position += (Vector3)newPos;
            distanceJoint.distance -= clawLiftSpeed * Time.deltaTime;
        }
    }
    void DropClaw()
    {
        if (distanceJoint.distance > clawMaxHeight)
        {
            //chain.transform.localPosition = new Vector3(0, clawMinHeight);
            distanceJoint.distance = clawMaxHeight;
        }
        else
        {
            //Vector2 newPos = new Vector2(0, clawLiftSpeed * Time.deltaTime);
            //chain.transform.position -= (Vector3)newPos;
            distanceJoint.distance += clawLiftSpeed * Time.deltaTime;
        }
    }
}
