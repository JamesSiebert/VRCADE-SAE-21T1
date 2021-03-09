using UnityEngine.XR.Interaction.Toolkit;

public class Bow : XRGrabInteractable
{
    
    /*
     * making the notch as ready once it has been picked and not when it has been dropped
     * this stops the arrow from automatically socketing itself in the bow. or once the bow has been dropped having an arrow still sit within the socket.
     * 
     */
    private Notch notch = null;

    protected override void Awake()
    {
        base.Awake();
        notch = GetComponentInChildren<Notch>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // Only notch an arrow if the bow is held
        selectEntered.AddListener(notch.SetReady);
        selectExited.AddListener(notch.SetReady);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(notch.SetReady);
        selectExited.RemoveListener(notch.SetReady);
    }
}
