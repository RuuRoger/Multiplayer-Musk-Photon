using System;
using UnityEngine;

namespace Assets.Scripts.Test
{
    public class TestCustomPlayerCOntroller : MonoBehaviour
    {
        private InputSystem_Actions _inputSystemAction;

        private void Awake()
        {
            _inputSystemAction = new InputSystem_Actions();
        }
    }
}