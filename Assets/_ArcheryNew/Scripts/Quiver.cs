﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Quiver : XRBaseInteractable
{
    public GameObject arrowPrefab = null;
    
    /*
     * when hand grabs on quiver
     * we create the arrow
     * Interaction manager gives it to the hand
     */

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(CreateAndSelectArrow);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(CreateAndSelectArrow);
    }

    private void CreateAndSelectArrow(SelectEnterEventArgs args)
    {
        // Create arrow, force into interacting hand
        Arrow arrow = CreateArrow(args.interactor.transform);
        interactionManager.ForceSelect(args.interactor, arrow);
    }

    private Arrow CreateArrow(Transform orientation)
    {
        // Create arrow, and get arrow component
        GameObject arrowObject = PhotonNetwork.Instantiate(arrowPrefab.name, orientation.position, orientation.rotation);
        //GameObject arrowObject = Instantiate(arrowPrefab, orientation.position, orientation.rotation);

        return arrowObject.GetComponent<Arrow>();
    }
    

}
