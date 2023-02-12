using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum PlatformType
    {
        Moving,
        Trigger
    }
    public PlatformType platformType;
    
    public List<Transform> ends;
    public float waitTime,speed;

    [SerializeField] private Transform target;

    [SerializeField] private int targetIdx;
    // Start is called before the first frame update
    void Start()
    {
        targetIdx = 0;
        target = ends[targetIdx];
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null && platformType == PlatformType.Moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed*Time.deltaTime);
            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                StartCoroutine(Switch());
            }
        }
    }
/*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player") && platformType == PlatformType.Trigger)
        {
            platformType = PlatformType.Moving;
        }
    }*/

    IEnumerator Switch()
    {
        target = null;
        yield return new WaitForSeconds(waitTime);
        targetIdx += 1;
        if (targetIdx > ends.Count-1)
        {
            targetIdx = 0;
        }

        target = ends[targetIdx];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
            if (platformType == PlatformType.Trigger)
            {
                platformType = PlatformType.Moving;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
