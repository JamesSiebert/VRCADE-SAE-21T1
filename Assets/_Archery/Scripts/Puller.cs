using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Puller : XRBaseInteractable
{

    public float PullAmount { get; private set; } = 0.0f;

    public Transform start = null;
    public Transform end = null;

    private XRBaseInteractor pullingInteractor = null;
    
    
    protected override void OnSelectEnter(XRBaseInteractor interactor)
    {
        Debug.Log("puller - OnSelectEnter");
        base.OnSelectEnter(interactor);
        pullingInteractor = interactor;
    }

    protected override void OnSelectExit(XRBaseInteractor interactor)
    {
        Debug.Log("puller - OnSelectExit");
        base.OnSelectExit(interactor);
        pullingInteractor = null;
        PullAmount = 0.0f;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (isSelected)
        {
            Vector3 pullPosition = pullingInteractor.transform.position;
            PullAmount = CalculatePull(pullPosition);
        }
    }

    private float CalculatePull(Vector3 pullPosition)
    {
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;
        float maxLength = targetDirection.magnitude;
        
        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength; // will let us know if we've reached max pull

        return Mathf.Clamp(pullValue, 0, 1);
    }
}
