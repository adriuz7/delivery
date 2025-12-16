using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    public enum ControlMode { Keyboard, Buttons }
    public enum Axel { Front, Rear }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public float maxAcceleration = 30f;
    public float brakeAcceleration = 50f;
    public float turnSensitivity = 1f;
    public float maxSteerAngle = 30f;

    public Vector3 centerOfMass;
    public List<Wheel> wheels;

    private float moveInput;
    private float steerInput;
    private bool isBraking;

    private Rigidbody carRb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction brakeAction;

    private void Awake()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMass;

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        brakeAction = playerInput.actions["Brake"];
    }

    private void Update()
    {
        ReadInput();
        AnimateWheels();
        WheelEffects();
    }

    private void FixedUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    private void ReadInput()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        steerInput = move.x;
        moveInput = move.y;

        isBraking = brakeAction.IsPressed();
    }

    private void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque =
                moveInput * maxAcceleration * 1000f * Time.fixedDeltaTime;
        }
    }

    private void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                float targetAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle =
                    Mathf.Lerp(wheel.wheelCollider.steerAngle, targetAngle, 0.6f);
            }
        }
    }

    private void Brake()
    {
        if (isBraking || Mathf.Abs(moveInput) < 0.1f)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque =
                    brakeAcceleration * 1000f * Time.fixedDeltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0f;
            }
        }
    }

    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            wheel.wheelModel.transform.SetPositionAndRotation(pos, rot);
        }
    }

    private void WheelEffects()
    {
        foreach (var wheel in wheels)
        {
            bool emit =
                isBraking &&
                wheel.axel == Axel.Rear &&
                wheel.wheelCollider.isGrounded &&
                carRb.linearVelocity.magnitude > 10f;

            wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = emit;

            if (emit)
                wheel.smokeParticle.Emit(1);
        }
    }
}
