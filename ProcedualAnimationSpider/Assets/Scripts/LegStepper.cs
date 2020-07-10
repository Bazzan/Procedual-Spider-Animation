using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegStepper : MonoBehaviour
{
    //[SerializeField] public Transform HomeTransform;
    //[SerializeField] private float wantToStepAtDistance;
    //[SerializeField] private float moveDuration;

    //public bool isMoving;




    //private void Update()
    //{
    //    if (isMoving) return;

    //    float distanceFromHome = Vector3.Distance(transform.position, HomeTransform.position);

    //    if (distanceFromHome > wantToStepAtDistance)
    //    {
    //        StartCoroutine(MoveToHome());
    //    }

    //}

    //IEnumerator MoveToHome()
    //{

    //    isMoving = true;

    //    Quaternion startRot = transform.rotation;
    //    Vector3 startPoint = transform.position;

    //    Quaternion endRot = HomeTransform.rotation;
    //    Vector3 endPoint = HomeTransform.position;

    //    float timePassed = 0;

    //    while (timePassed < moveDuration)
    //    {
    //        timePassed += Time.deltaTime;

    //        float normalizedTime = timePassed / moveDuration;

    //        transform.position = Vector3.Lerp(startPoint, endPoint, normalizedTime);
    //        transform.rotation = Quaternion.Slerp(startRot, endRot, normalizedTime);

    //        yield return null;
    //    }

    //    isMoving = false;


    //}



    [SerializeField] public Transform HomeTransform;
    [SerializeField] private float wantToStepAtDistance;
    [SerializeField] private float moveDuration;

    [SerializeField] private float stepOverShootFraction;

    public bool isMoving;




    public void TryToMove()
    {
        if (isMoving) return;

        float distanceFromHome = Vector3.Distance(transform.position, HomeTransform.position);

        if (distanceFromHome > wantToStepAtDistance)
        {
            StartCoroutine(MoveToHome());
        }

    }

    IEnumerator MoveToHome()
    {

        isMoving = true;

        Quaternion startRot = transform.rotation;
        Vector3 startPoint = transform.position;

        Quaternion endRot = HomeTransform.rotation;

        // offsett step
        Vector3 homeDir = HomeTransform.position - transform.position;

        float overShootDistance = wantToStepAtDistance * stepOverShootFraction;

        Vector3 overShootVector = homeDir * overShootDistance;

        overShootVector = Vector3.ProjectOnPlane(overShootVector, Vector3.up);

        Vector3 endPoint = HomeTransform.position + overShootVector;


        // bezier max point
        Vector3 centerPoint = (startPoint + endPoint) / 2;

        centerPoint += HomeTransform.up * Vector3.Distance(startPoint, endPoint) / 2;

        float timePassed = 0;



        while (timePassed < moveDuration)
        {
            timePassed += Time.deltaTime;

            float normalizedTime = timePassed / moveDuration;

            normalizedTime = Easing.Cubic.InOut(normalizedTime);

            transform.position = Vector3.Lerp(
                Vector3.Lerp(startPoint, centerPoint, normalizedTime),
                Vector3.Lerp(centerPoint, endPoint, normalizedTime),
                normalizedTime);



            transform.rotation = Quaternion.Slerp(startRot, endRot, normalizedTime);

            yield return null;
        }

        isMoving = false;


    }

}
