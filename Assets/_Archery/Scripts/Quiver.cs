using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Quiver : XRSocketInteractor
{
    public GameObject arrowPrefab = null;
    private Vector3 attachOffset = Vector3.zero;
    
    protected override void Awake()
    {
        base.Awake();
        CreateAndSelectArrow();
        SetAttachOffset();
    }

    protected override void OnSelectExit(XRBaseInteractable interactable)
    {
        Debug.Log("Quiver - OnSelectExit");
        base.OnSelectExit(interactable);
        CreateAndSelectArrow();
    }

    private void CreateAndSelectArrow()
    {
        // Debug.Log("Quiver - CreateAndSelectArrow");
        Arrow arrow = CreateArrow();
        SelectArrow(arrow);
    }

    private Arrow CreateArrow()
    {
        // Debug.Log("Quiver - CreateArrow");
        GameObject arrowObject = Instantiate(arrowPrefab, transform.position - attachOffset, transform.rotation);
        return arrowObject.GetComponent<Arrow>();
    }

    private void SelectArrow(Arrow arrow)
    {
        // Debug.Log("Quiver - SelectArrow");
        OnSelectEnter(arrow);
        arrow.OnSelectEnter(this);
    }

    private void SetAttachOffset()
    {
        Debug.Log("Quiver - SetAttachOffset");
        if (selectTarget is XRGrabInteractable interactable)
            attachOffset = interactable.attachTransform.localPosition;
    }
}
