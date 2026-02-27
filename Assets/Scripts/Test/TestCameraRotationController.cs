using System;
using UnityEngine;

namespace Assets.Scripts.Test
{
    public class TestCameraRotationController : MonoBehaviour
    {
        [SerializeField] private float m_sensitivity = 0f;
        [SerializeField] private float m_minValueCamera;
        [SerializeField] private float m_maxValueCamera;

        private InputSystem_Actions m_inputSystemAction;
        private Vector2 m_lookInput;
        private float m_yaw = 0f;
        private float m_pitch = 0f;

        private void Awake()
        {
            m_inputSystemAction = new InputSystem_Actions();
        }

        private void LateUpdate()
        {
            ReadInput();
            RotationCameraHandler();
        }

        private void OnEnable()
        {
            m_inputSystemAction.Player.Look.Enable();
        }

        private void OnDisable()
        {
            m_inputSystemAction.Player.Look.Disable();
        }

        private void ReadInput()
        {
            m_lookInput = m_inputSystemAction.Player.Look.ReadValue<Vector2>();
        }

        private void RotationCameraHandler()
        {
            m_yaw += m_lookInput.x * m_sensitivity;
            m_pitch -= m_lookInput.y * m_sensitivity;
            m_pitch = Mathf.Clamp(m_pitch, -0.4f, 0.4f);
            transform.rotation = Quaternion.Euler(m_pitch, m_yaw, 0f);
        }
    }
}