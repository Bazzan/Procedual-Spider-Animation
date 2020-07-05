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
    [SerializeField] private LegStepper frontLeftLegStepper;
    [SerializeField] private LegStepper midLeftLegStepper;
    [SerializeField] private LegStepper backLeftLegStepper;
    [SerializeField] private LegStepper frontRightLegStepper;
    [SerializeField] private LegStepper midRightLegStepper;
    [SerializeField] private LegStepper backRightLegStepper;


    private Vector3 forwardSpeed = new Vector3(0, 0, 1);
    private float maxSpeed = 4;
    private Quaternion startRotation = Quaternion.identity;

    private Vector3 dir;
    private RaycastHit rayHit;

    private void Awake()
    {
        StartCoroutine(LegUpdateCoroutine());

    }



    private void Update()
    {

        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            transform.position += this.transform.forward * 10 * Time.deltaTime;

        }else if(Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            transform.position += this.transform.forward * -10 * Time.deltaTime;

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

            midLeftLegStepper.TryToMove();
            midRightLegStepper.TryToMove();


            do
            {
                frontLeftLegStepper.TryToMove();
                midRightLegStepper.TryToMove();

                backRightLegStepper.TryToMove();
                yield return null;

            } while (frontLeftLegStepper.isMoving || backRightLegStepper.isMoving);

            do
            {
                midLeftLegStepper.TryToMove();

                frontRightLegStepper.TryToMove();
                backLeftLegStepper.TryToMove();

                yield return null;
            } while (frontRightLegStepper.isMoving || backLeftLegStepper.isMoving);


        }
    }
    

    private void LateUpdate()
    {
        HeadTracking();

    }

    private void HeadTracking()
    {
        Quaternion currentLocalRotation = headBone.localRotation;

        headBone.localRotation = Quaternion.identity;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out rayHit, 100f);
        Vector3 worldTargetLookDir = rayHit.point -  headBone.position;

        //Vector3 worldTargetLookDir = target.position - headBone.position;
        Vector3 localTargetLookDir = headBone.InverseTransformDirection(worldTargetLookDir);


        localTargetLookDir = Vector3.RotateTowards(
            Vector3.forward,
            localTargetLookDir,
            Mathf.Deg2Rad * headClampAngel,
            0);

        Quaternion localTargetRotation = Quaternion.LookRotation(localTargetLookDir, transform.up);

        headBone.localRotation = Quaternion.Slerp(currentLocalRotation,
            localTargetRotation,
            1 - Mathf.Exp(-headTurningSpeed * Time.deltaTime));
    }

}
