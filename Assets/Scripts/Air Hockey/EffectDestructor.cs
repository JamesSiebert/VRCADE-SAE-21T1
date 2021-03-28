using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestructor : MonoBehaviour
{
    public  float destroySeconds;

    void Start()
    {
        Destroy(this.gameObject, destroySeconds);
    }
}
