using UnityEngine;

public class CarController : MonoBehaviour {
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBreakForce;

    // Settings
    [SerializeField] private float motorForce = 1000f;
    //[SerializeField] private float breakForce = 2000f;
    [SerializeField] private float maxSteerAngle = 45f;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    //private void Update()
    //{
    //    float forwardInput = Input.GetAxis("Vertical"); // Передний ввод
    //    float turnInput = Input.GetAxis("Horizontal"); // Ввод поворота

    //    // Вызываем SetInputs с полученными значениями
    //    SetInputs(forwardInput, turnInput);
    //    HandleMotor();
    //    HandleSteering();
    //    UpdateWheels();
    //}

    public void SetInputs(float forwardAmount, float turnAmount) {
        /*Debug.Log("INPUTS :" + forwardAmount + " " + turnAmount);*/
        verticalInput = forwardAmount;
        horizontalInput = turnAmount;

        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void HandleMotor() {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        // ????????: ???? ?????? W ?? ?????? ? ???????? ?????? ?????????????, ????????? ??????????? ????????? ??????
        /*if (!Input.GetKey(KeyCode.W) && verticalInput >= 0 && GetComponent<Rigidbody>().velocity.magnitude > 0.1f) {
            currentBreakForce = breakForce * 0.2f; // ????????, ????????? ?????? ????? 20% ?? ?????????????
        }
        else {
            currentBreakForce = 0f;
        }*/

        ApplyBraking();
    }

    private void ApplyBraking() {
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    public void StopCompletely() {
        SetInputs(0f, 0f);
        ApplyBraking();
    }
   
    private void HandleSteering() {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels() {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
