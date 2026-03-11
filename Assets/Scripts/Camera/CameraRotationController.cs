using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class CameraRotationController : MonoBehaviour
    {
#region MEMBERS
        [SerializeField] private float m_sensitivity = 0.1f;
        private InputSystem_Actions m_inputSystemAction;
        private Vector2 m_lookInput = new Vector2(0f, 0f);
        private float m_yaw = 0f;
        private float m_pitch = 0f;
#endregion

#region UNITY LIFECYCLE METHODS
        private void Awake()
        {
            m_inputSystemAction = new InputSystem_Actions();
            m_yaw = transform.parent != null ? transform.parent.eulerAngles.y : transform.eulerAngles.y;
            m_pitch = NormalizeAngle(transform.localEulerAngles.x);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            ReadInput();
            RotationCamera();
        }
#endregion

#region EVENTS
        private void OnEnable()
        {
            m_inputSystemAction.Player.Look.Enable();
        }

        private void OnDisable()
        {
            m_inputSystemAction.Player.Look.Disable();
        }
#endregion

#region ROTATION
        private void ReadInput()
        {
            m_lookInput = m_inputSystemAction.Player.Look.ReadValue<Vector2>();
        }
        private void RotationCamera()
        {
            m_yaw += m_lookInput.x * m_sensitivity;
            m_pitch -= m_lookInput.y * m_sensitivity;
            m_pitch = Mathf.Clamp(m_pitch, -40f, 40f);

            if (transform.parent != null)
            {
                transform.parent.rotation = Quaternion.Euler(0f, m_yaw, 0f);
            }

            transform.localRotation = Quaternion.Euler(m_pitch, 0f, 0f);
        }

        private static float NormalizeAngle(float angle)
        {
            if (angle > 180f)
            {
                angle -= 360f;
            }

            return angle;
        }
#endregion
    }
}