using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Hit : MonoBehaviour
{
    public bool move;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Archery Target"))
        {
            Debug.Log("Archery target hit");
            other.GetComponent<Target>().ArrowHit();
            Destroy(this.gameObject);
        }
        Debug.Log("not target hit");
    }
}
