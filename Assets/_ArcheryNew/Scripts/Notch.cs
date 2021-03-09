using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PullMeasurer))]
public class Notch : XRSocketInteractor
{
    // Settings - 
    // between 0-1 how far does the arrow need to be pulled back to be released.
    [Range(0, 1)] public float releaseThreshold = 0.25f;

    // Necessary stuff
    public PullMeasurer PullMeasurer { get; private set; } = null;
    public bool IsReady { get; private set; } = false; // set by bow when picked up or dropped

    // Need to cast to custom for Force Deselect
    // get reference to interaction and cast as a custom interaction manager - so we can easily use force deselect function
    private CustomInteractionManager CustomManager => interactionManager as CustomInteractionManager;

    protected override void Awake()
    {
        base.Awake();
        PullMeasurer = GetComponent<PullMeasurer>(); // get pull measurer component
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // Arrow is released once the puller is released
        // subscribe to pull measurer events - when pull measurer is released, release the arrow
        PullMeasurer.selectExited.AddListener(ReleaseArrow);

        // Move the point where the arrow is attached
        // when pull measurer is being pulled, move attach point
        PullMeasurer.Pulled.AddListener(MoveAttach);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        // unsubscribe from events
        PullMeasurer.selectExited.RemoveListener(ReleaseArrow);
        PullMeasurer.Pulled.RemoveListener(MoveAttach);
    }

    public void ReleaseArrow(SelectExitEventArgs args)
    {
        // Only release if the target is an arrow using custom deselect
        // make sure target notch has selected is an arrow
        // if yes then use custom deselect
        if (selectTarget is Arrow && PullMeasurer.PullAmount > releaseThreshold)
            CustomManager.ForceDeselect(this); //this is an interactor, does this object 
    }

    public void MoveAttach(Vector3 pullPosition, float pullAmount)
    {
        // Move attach when bow is pulled, this updates the renderer as well
        attachTransform.position = pullPosition;
    }

    public void SetReady(BaseInteractionEventArgs args)
    {
        // Set the notch ready if bow is selected
        IsReady = args.interactable.isSelected;
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        // We check for the hover here too, since it factors in the recycle time of the socket
        // We also check that notch is ready, which is set once the bow is picked up
        //return base.CanSelect(interactable) && CanHover(interactable) && IsArrow(interactable);
        return base.CanSelect(interactable) && CanHover(interactable) && IsArrow(interactable) && IsReady;
    }

    private bool IsArrow(XRBaseInteractable interactable)
    {
        // Simple arrow check, can be tag or interaction layer as well
        return interactable is Arrow;
    }

    public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride
    {
        // Use instantaneous so it follows smoothly
        get { return XRBaseInteractable.MovementType.Instantaneous; }
    }

    // This enables the socket to grab the arrow immediately
    public override bool requireSelectExclusive => false;
}
