using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    private float speed = 2f;
    public bool move;

    void Update()
    {
        if (move)
        {
            // Cube movement
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            if (transform.position.x >= 15 || transform.position.x <= -15 || 
                transform.position.y >= 15 || transform.position.y <= 0 || 
                transform.position.z >= 15 || transform.position.z <= 5)
            {
                speed = speed * -1;
            }
        }
    }
}