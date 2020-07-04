using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [SerializeField] private Transform target;


    //head
    [SerializeField] private Transform headBone;
    [SerializeField] private float headTurningSpeed;
    [SerializeField] private float headClampAngel;

    //Legs
    [SerializeField] LegStepper frontLeftLegStepper;
    [SerializeField] LegStepper midLeftLegStepper;
    [SerializeField] LegStepper backLeftLegStepper;
    [SerializeField] LegStepper frontRightLegStepper;
    [SerializeField] LegStepper midRightLegStepper;
    [SerializeField] LegStepper backRightLegStepper;


    Rigidbody rigidbody;
    Vector3 forwardSpeed = new Vector3(0, 0, 1);
    float maxSpeed = 4;
    Quaternion startRotation = Quaternion.identity;

    Vector3 dir;

    private void Awake()
    {
        StartCoroutine(LegUpdateCoroutine());
        rigidbody = GetComponent<Rigidbody>();

    }



    private void Update()
    {
        //transform.rotation.z = startRotation.z;


        if (Input.GetKey(KeyCode.W))
        {
            transform.position += this.transform.forward * 10 * Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * -100 * Time.deltaTime);
        }else if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        }
        

    }


    IEnumerator LegUpdateCoroutine()
    {

        while (true)
        {
            do
            {
                frontLeftLegStepper.TryToMove();
                backLeftLegStepper.TryToMove();

                frontRightLegStepper.TryToMove();
                backRightLegStepper.TryToMove();
                yield return null;

            } while (backLeftLegStepper.isMoving || frontLeftLegStepper.isMoving || backRightLegStepper.isMoving || frontRightLegStepper.isMoving);
            do
            {
                midLeftLegStepper.TryToMove();
                midRightLegStepper.TryToMove();
                yield return null;
            } while (midLeftLegStepper.isMoving || midRightLegStepper.isMoving);
        }
    }
    

    private void LateUpdate()
    {
        Quaternion currentLocalRotation = headBone.localRotation;

        headBone.localRotation = Quaternion.identity;

        Vector3 worldTargetLookDir = target.position - headBone.position;
        Vector3 localTargetLookDir = headBone.parent.InverseTransformDirection(worldTargetLookDir);


        localTargetLookDir = Vector3.RotateTowards(
            Vector3.forward,
            localTargetLookDir,
            Mathf.Deg2Rad * headClampAngel,
            0);

        Quaternion localTargetRotation = Quaternion.LookRotation(localTargetLookDir, transform.up);

        headBone.rotation = Quaternion.Slerp(currentLocalRotation, 
            localTargetRotation,
            1 - Mathf.Exp(-headTurningSpeed * Time.deltaTime));


    }


}
