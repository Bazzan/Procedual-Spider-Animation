using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegStepper : MonoBehaviour
{
    [SerializeField] public Transform HomeTransform;
    [SerializeField] private float wantToStepAtDistance;
    [SerializeField] private float moveDuration;

    public bool isMoving;




    private void Update()
    {
        if (isMoving) return;

        float distanceFromHome = Vector3.Distance(transform.position, HomeTransform.position);

        if(distanceFromHome > wantToStepAtDistance)
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
        Vector3 endPoint = HomeTransform.position;

        float timePassed = 0;

        while(timePassed < moveDuration)
        {
            timePassed += Time.deltaTime;

            float normalizedTime = timePassed / moveDuration;

            transform.position = Vector3.Lerp(startPoint, endPoint, normalizedTime);
            transform.rotation = Quaternion.Slerp(startRot, endRot, normalizedTime);

            yield return null;
        }

        isMoving = false;


    }

}
