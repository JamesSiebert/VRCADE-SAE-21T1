using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class XR_InputLogger : MonoBehaviour
{

    [SerializeField] 
    private XRNode xrNode = XRNode.LeftHand;
    
    private List<InputDevice> devices = new List<InputDevice>();

    private InputDevice device;

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
        device = devices.FirstOrDefault();
    }

    private void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }

    void Update()
    {
        if (!device.isValid)
        {
            GetDevice();
        }

        
        // Capture Trigger
        bool triggerButtonAction = false;
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonAction) && triggerButtonAction)
        {
            Debug.Log("Trigger Button Active");
        }
        
        
        // Capture Primary Button
        bool primaryButtonValue = false;
        InputFeatureUsage<bool> primaryButtonUsage = CommonUsages.primaryButton;
        if (device.TryGetFeatureValue(primaryButtonUsage, out primaryButtonValue) && primaryButtonValue)
        {
            Debug.Log("Primary Button Active");
        }
        
        // Capture Primary 2D Axis - thumbstick
        Vector2 primary2DAxisValue = Vector2.zero;
        InputFeatureUsage<Vector2> primary2DAxisUsage = CommonUsages.primary2DAxis;
        if (device.TryGetFeatureValue(primary2DAxisUsage, out primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
        {
            Debug.Log($"Primary2DAxis (thumb) value {primary2DAxisValue}");
        }
        
        // Capture Grip float value
        float gripValue = 0;
        InputFeatureUsage<float> gripUsage = CommonUsages.grip;
        if (device.TryGetFeatureValue(gripUsage, out gripValue))
        {
            if (gripValue > 0.01)
            {
                Debug.Log($"Grip Value {gripValue}");
            }
            
        }
    }
}
