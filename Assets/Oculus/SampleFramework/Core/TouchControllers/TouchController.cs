/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided �AS IS� WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace OVRTouchSample
{
    // Animating controller that updates with the tracked controller.
    public class TouchController : MonoBehaviour
    {
        
        //[SerializeField]
        //private OVRInput.Controller m_controller = OVRInput.Controller.None;
        
        [SerializeField]
        private Animator m_animator = null;

        private bool m_restoreOnInputAcquired = false;

        public bool isLeftHand = true;
        public List<InputDevice> devices = new List<InputDevice>();
        public InputDevice device;

        public bool debug;

        
        void GetDevice()
        {
            if (isLeftHand)
            {
                // Get all left hand devices
                InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
            }
            else
            {
                // Get all right hand devices
                InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
            }
            // Get first device in list
            device = devices.FirstOrDefault();
        }
        
        private void OnEnable()
        {
            if (!device.isValid)
            {
                // If device is not valid get device
                GetDevice();
            }
        }
        
        
        private void Update()
        {
            // If device is not valid get device
            if (!device.isValid)
                GetDevice();
            

            // Get primary button Bool
            device.TryGetFeatureValue(CommonUsages.primaryButton, out bool valuePrimary);
            m_animator.SetFloat("Button 1", valuePrimary ? 1.0f : 0.0f);
            
            // Get secondary button Bool
            device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool valueSecondary);
            m_animator.SetFloat("Button 2", valueSecondary ? 1.0f : 0.0f);
            
            
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 valueJoyX);
            m_animator.SetFloat("Joy X", valueJoyX.x);
            
            
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 valueJoyY);
            m_animator.SetFloat("Joy Y", valueJoyY.y);
            
            
            device.TryGetFeatureValue(CommonUsages.grip, out float valueGrip);
            m_animator.SetFloat("Grip", valueGrip);
            
            
            device.TryGetFeatureValue(CommonUsages.trigger, out float valueTrigger);
            m_animator.SetFloat("Trigger", valueTrigger);

            if (debug)
            {
                Debug.Log(
                    $"valuePrimary: {valuePrimary} | " +
                          $"valueSecondary: {valueSecondary} | " +
                          $"valueJoyX.x: {valueJoyX.x} | " +
                          $"valueJoyY.y:  {valueJoyY.y} |" +
                          $"valueGrip: {valueGrip} | " +
                          $"valueTrigger: {valueTrigger}");
            }
                
            
            
            //
            // OVRManager.InputFocusAcquired += OnInputFocusAcquired;
            // OVRManager.InputFocusLost += OnInputFocusLost;
            
            
            // Original
            // m_animator.SetFloat("Button 1", OVRInput.Get(OVRInput.Button.One, m_controller) ? 1.0f : 0.0f);
            // m_animator.SetFloat("Button 2", OVRInput.Get(OVRInput.Button.Two, m_controller) ? 1.0f : 0.0f);
            // m_animator.SetFloat("Joy X", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, m_controller).x);
            // m_animator.SetFloat("Joy Y", OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, m_controller).y);
            // m_animator.SetFloat("Grip", OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller));
            // m_animator.SetFloat("Trigger", OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller));
            //
            // OVRManager.InputFocusAcquired += OnInputFocusAcquired;
            // OVRManager.InputFocusLost += OnInputFocusLost;
        }

        // private void OnInputFocusLost()
        // {
        //     if (gameObject.activeInHierarchy)
        //     {
        //         gameObject.SetActive(false);
        //         m_restoreOnInputAcquired = true;
        //     }
        // }
        //
        // private void OnInputFocusAcquired()
        // {
        //     if (m_restoreOnInputAcquired)
        //     {
        //         gameObject.SetActive(true);
        //         m_restoreOnInputAcquired = false;
        //     }
        // }

    }
}
