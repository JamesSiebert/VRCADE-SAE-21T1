using System;
using UnityEngine;
using UnityEngine.Events;

public class ShooterTarget : MonoBehaviour
{
    // create a new class that inherits from UnityEvent, this lets us pass a value with event. Serializable makes Visible in editor
    [Serializable] public class HitEvent : UnityEvent<int> { }

    // custom event
    public HitEvent OnHit = new HitEvent();

    // When projectile hits this target
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Figure out the score using the collision position.
            FigureOutScore(collision.transform.position);
        }
    }

    private void FigureOutScore(Vector3 hitPosition)
    {
        // distance the projectile hit from the center
        float distanceFromCenter = Vector3.Distance(transform.position, hitPosition);
        
        int score = 0;

        // Target radius = 0.5
        
        // if inside inner 50%
        if (distanceFromCenter < 0.25f)
            score = 15;
        // If inside outer circle
        else if (distanceFromCenter < 0.5)
            score = 5;
        
        // Calls event and passes the int with it.
        OnHit.Invoke(score);
    }
}
