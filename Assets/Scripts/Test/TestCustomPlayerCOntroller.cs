using System;
using UnityEngine;

namespace Assets.Scripts.Test
{
    [RequireComponent(typeof(CharacterController))]
    public class TestCustomPlayerCOntroller : MonoBehaviour
    {
        [SerializeField]
        private float m_speed = 5f;
        private InputSystem_Actions m_inputSystemAction;
        private CharacterController m_characterController;
        private Vector2 m_inputMove = new Vector2(0f, 0f);

        private void Awake()
        {
            m_inputSystemAction = new InputSystem_Actions();
            m_characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            m_inputSystemAction.Player.Move.Enable();
        }

        private void OnDisable()
        {
            m_inputSystemAction.Player.Move.Disable();
        }

        private void Update()
        {
            ReadInput();
            Move();           
        }

        private void ReadInput()
        {
            m_inputMove = m_inputSystemAction.Player.Move.ReadValue<Vector2>();
        }

        private void Move()
        {
            var direction = new Vector3(m_inputMove.x, 0f, m_inputMove.y);
            m_characterController.Move(m_speed * Time.deltaTime * direction);
        }
    }
}