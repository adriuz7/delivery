using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car;
        private CarInputActions input;

        private float steer;
        private float accel;
        private float brake;
        private float handbrake;

        private void Awake()
        {
            m_Car = GetComponent<CarController>();
            input = new CarInputActions();
        }

        private void OnEnable()
        {
            input.Player.Enable();

            input.Player.Steer.performed += ctx => steer = ctx.ReadValue<float>();
            input.Player.Steer.canceled  += _ => steer = 0f;

            input.Player.Accelerate.performed += ctx => accel = ctx.ReadValue<float>();
            input.Player.Accelerate.canceled  += _ => accel = 0f;

            input.Player.Brake.performed += ctx => brake = ctx.ReadValue<float>();
            input.Player.Brake.canceled  += _ => brake = 0f;

            input.Player.Handbrake.performed += _ => handbrake = 1f;
            input.Player.Handbrake.canceled  += _ => handbrake = 0f;
        }

        private void OnDisable()
        {
            input.Player.Disable();
        }

        private void FixedUpdate()
        {
            m_Car.Move(steer, accel, brake, handbrake);
        }
    }
}
