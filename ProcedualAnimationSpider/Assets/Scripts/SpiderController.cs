using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [SerializeField] private Transform target;



    [SerializeField] private Transform headBone;
    [SerializeField] private float headTurningSpeed;
    [SerializeField] private float headClampAngel;



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
