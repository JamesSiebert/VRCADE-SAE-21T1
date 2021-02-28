using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Notch : XRSocketInteractor
{

    private Puller puller = null;
    private Arrow currentArrow = null;

    
    protected override void Awake()
    {
        base.Awake();
        puller = GetComponent<Puller>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        puller.onSelectExit.AddListener(TryToReleaseArrow);
        
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        puller.onSelectExit.RemoveListener(TryToReleaseArrow);
    }

    protected override void OnSelectEnter(XRBaseInteractable interactable) // when you initiate a new grab on the sphere
    {
        Debug.Log("OnSelectEnter");
        base.OnSelectEnter(interactable);
        StoreArrow(interactable);
    }
    

    private void StoreArrow(XRBaseInteractable interactable)
    {
        Debug.Log("StoreArrow");
        if (interactable is Arrow arrow) // cast & if cast is valid
        {
            currentArrow = arrow;
            Debug.Log("Current arrow set - cast ok");
        }
            
    }

    private void TryToReleaseArrow(XRBaseInteractor interactor)
    {
        Debug.Log("TryToReleaseArrow");
        if (currentArrow)
        {
            // fighting XR toolkit
            // force disconnection of current arrow
            ForceDeselect();
            
            // tell arrow to release itself
            ReleaseArrow();
            
        }
    }

    private void ForceDeselect()
    {
        Debug.Log("Force Deselect");
        base.OnSelectExit(currentArrow);
        currentArrow.OnSelectExit(this);
    }

    private void ReleaseArrow()
    {
        Debug.Log("Release Arrow");
        currentArrow.Release(puller.PullAmount);
        currentArrow = null;
    }

    public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride
    {
        get { return XRBaseInteractable.MovementType.Instantaneous; }
    }
}
