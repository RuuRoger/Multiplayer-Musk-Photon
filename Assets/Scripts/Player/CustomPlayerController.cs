using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]

    public class CustomPlayerController : MonoBehaviour
    {
#region Members        
        [SerializeField] private float m_rotationSpeed = 1f;
        private InputSystem_Actions m_inputSystemAction;
        private Vector2 m_moveInput = new Vector2(0f, 0f);
        private float m_speed = 5f;
        private CharacterController m_characterController;
#endregion

#region Unity LifeCycle Methods
        private void Awake()
        {
            m_inputSystemAction = new InputSystem_Actions();
            m_characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            ReadInput();
            Move();
        }
#endregion        

#region Events
        private void OnEnable()
        {
            m_inputSystemAction.Player.Move.Enable();
        }

        private void OnDisable()
        {
            m_inputSystemAction.Player.Move.Disable();
        }
#endregion

# region Horizontal
        private void ReadInput()
        {
            m_moveInput = m_inputSystemAction.Player.Move.ReadValue<Vector2>();
        }

        private void Move()
        {
            var camForward = UnityEngine.Camera.main.transform.forward;
            var camRight = UnityEngine.Camera.main.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            var direction = camForward * m_moveInput.y + camRight * m_moveInput.x;
            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);
            }
            m_characterController.Move(direction * m_speed * Time.deltaTime);
        }
#endregion
    }
}