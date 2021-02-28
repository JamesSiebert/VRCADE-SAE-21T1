using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : XRGrabInteractable
{

    public float speed = 2000;
    public Transform tip = null;
    
    private bool inAir = false;
    private Vector3 lastPosition = Vector3.zero;
    private Rigidbody rigidBody = null;
    
    protected override void Awake()
    {
        base.Awake();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // stops weird behaviour if brush up against things
        if (inAir)
        {
            CheckForCollision();
            lastPosition = tip.position; // every frame, line cast and see if there was hit.
        }
    }

    private void CheckForCollision()
    {
        if(Physics.Linecast(lastPosition, tip.position))
            Stop();
    }

    private void Stop()
    {
        inAir = false;
        SetPhysics(false);
    }

    public void Release(float pullValue)
    {
        inAir = true;
        SetPhysics(true);
        
        MaskAndFire(pullValue);
        StartCoroutine(RotateWithVelocity());

        lastPosition = tip.position;
    }

    private void SetPhysics(bool usePhysics)
    {
        rigidBody.isKinematic = !usePhysics;
        rigidBody.useGravity = usePhysics;
    }

    private void MaskAndFire(float power)
    {
        colliders[0].enabled = false; // disable grab collider
        interactionLayerMask = 1 << LayerMask.NameToLayer("Ignore"); // Bitwise operator - sets arrow to ignore layer

        Vector3 force = transform.forward * (power * speed);
        rigidBody.AddForce(force);
    }

    private IEnumerator RotateWithVelocity() //coroutine - wait for fixed update
    {
        yield return new WaitForFixedUpdate();

        while (inAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(rigidBody.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }

    public new void OnSelectEnter(XRBaseInteractor interactor)
    {
        Debug.Log("Arrow - OnSelectEnter");
        base.OnSelectEnter(interactor);
    }

    public new void OnSelectExit(XRBaseInteractor interactor)
    {
        Debug.Log("Arrow - OnSelectExit");
        base.OnSelectExit(interactor);
    }
}
