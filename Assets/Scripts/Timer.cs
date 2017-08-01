using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Transform target;

    public float maxTime = 2f;
    public float currentTime;

    private bool runTimer;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, 0.1f);

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    runTimer = !runTimer;
        //}

        //if (runTimer)
        //{
        //    currentTime += Time.deltaTime;
        //    if (currentTime >= maxTime)
        //    {
        //        Debug.Log("You die now");
        //        //Do stuff here
        //        currentTime = 0f;
        //    }
        //}
    }
}
