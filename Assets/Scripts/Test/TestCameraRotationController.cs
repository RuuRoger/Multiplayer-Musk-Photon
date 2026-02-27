using System;
using UnityEngine;

namespace Assets.Scripts.Test
{
    public class TestCameraRotationController : MonoBehaviour
    {
        private InputSystem_Actions m_inputSystemAction;

        private void Awake()
        {
            m_inputSystemAction = new InputSystem_Actions();
        }
    }
}