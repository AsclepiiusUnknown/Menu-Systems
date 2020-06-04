using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SmoothedFollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 7.5f, 0f);
    public float smoothSpeed = .005f;
    public float largeLevelSmoothSpeed = .05f;

    public MapGenerator mapGen;

    private void Start()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (!target && target != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition;

            if (mapGen.currentMap.largeMap)
            {
                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, largeLevelSmoothSpeed);
            }
            else
            {
                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            }

            transform.position = smoothedPosition;
        }
    }
}
