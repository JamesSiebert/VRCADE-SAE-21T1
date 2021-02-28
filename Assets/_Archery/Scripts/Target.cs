using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private float speed = 2f;
    Renderer rend;
    int colorPicker = 0;

    public bool move;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }
    
    void Update()
    {
        if (move)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        
    }

    //When the Primitive collides, it will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        speed = speed * -1;
        colorPicker = Random.Range(0, 10);
        
        // switch (colorPicker)
        // {
        //     case 0: rend.material.color = Color.white; break;
        //     case 1: rend.material.color = Color.cyan; break;
        //     case 2: rend.material.color = Color.blue; break;
        //     case 3: rend.material.color = Color.black; break;
        //     case 4: rend.material.color = Color.red; break;
        //     case 5: rend.material.color = Color.green; break;
        //     case 6: rend.material.color = Color.grey; break;
        //     case 7: rend.material.color = Color.magenta; break;
        //     case 8: rend.material.color = Color.yellow; break;
        //     case 9: rend.material.color = Color.gray; break;
        // }
    }
    
    private void OnTriggerExit(Collider other)
    {
    }
}

