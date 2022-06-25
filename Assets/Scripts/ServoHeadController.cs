using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ArticulationBody))]
public class ServoHeadController : MonoBehaviour
{
    ArticulationBody Articulationbody = null;

    [HeaderAttribute("Motor Physics attributes")]

    [TooltipAttribute("Represnets the friction coffienect")]
    [SerializeField] float dampingValue = 0.07f;
    [TooltipAttribute("Represnets the spring force (spring that returns motor to orignial postion)")]
    [SerializeField] float stiffness = 0.0f;

    [SerializeField] bool hasRotationLimit = false;
    [SerializeField] float minAngle = 0;
    [SerializeField] float maxAngle = 180f;
    [SerializeField] float startAngle =0;

    [SerializeField] float targetVelocity;
    [SerializeField] float torqueLimit;


    void Awake()
    {
        Articulationbody = GetComponent<ArticulationBody>();
        ResetServo();
    }

    public void ResetServo() {
        ArticulationDrive Tempdrive = new ArticulationDrive();
        Tempdrive.damping = dampingValue;
        Tempdrive.stiffness = stiffness;
        Tempdrive.target = startAngle;
        Tempdrive.targetVelocity = targetVelocity;
        if (hasRotationLimit) {
            Articulationbody.twistLock = ArticulationDofLock.LimitedMotion;
            Tempdrive.lowerLimit = minAngle;
            Tempdrive.upperLimit = maxAngle;
        }
        Tempdrive.forceLimit = torqueLimit;
        Articulationbody.xDrive = Tempdrive;
    }



    public void MoveToAngle(float angle)
    {

        if(AngleNotValid(angle))
        {
            Debug.LogWarning("MoveToAngle : entered angle is not valid");
            Mathf.Clamp(angle, minAngle, maxAngle -1);
            return;
        }

        ArticulationDrive tempDrive = new ArticulationDrive();
        tempDrive = Articulationbody.xDrive;

        tempDrive.target = angle;
        Articulationbody.xDrive = tempDrive;

    }

    public void MoveWithAngle(float angle , bool clockwise)
    {
        float targetAngle = Articulationbody.xDrive.target + ( (clockwise )? 1:-1 ) * angle;

        if(AngleNotValid(targetAngle))
            targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle -1);

        ArticulationDrive tempDrive = new ArticulationDrive();
        tempDrive = Articulationbody.xDrive;

        tempDrive.target = targetAngle;

        Articulationbody.xDrive = tempDrive;

    }


    private bool AngleNotValid(float angle) {
       return angle > maxAngle || angle < minAngle;
    }
}
