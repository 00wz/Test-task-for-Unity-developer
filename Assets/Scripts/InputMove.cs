using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class InputMove : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 2f;

    private CharacterController _controller;
    readonly private float DIAGONAL_COEFFICIENT = 1 / Mathf.Sqrt(2f);
    private Vector3 CAMERA_RIGHT;
    private Vector3 CAMERA_FORWARD;

    void Start()
    {
        InitMoveDirections();
        _controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Defines the projections of the camera axes on the XZ plane so
    /// that the movement is relative to the camera
    /// </summary>
    private void InitMoveDirections()
    {
        Quaternion cameraRotateY = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        CAMERA_RIGHT = cameraRotateY * Vector3.right;
        CAMERA_FORWARD = cameraRotateY * Vector3.forward;
    }

    void Update()
    {
        Vector3 direction = Input.GetAxis("Horizontal") * CAMERA_RIGHT +
                Input.GetAxis("Vertical") * CAMERA_FORWARD;

        float directionSqrLength = direction.sqrMagnitude;

        //input check
        if (directionSqrLength > 0.001f)
        {
            //correction of diagonally accelerated movement
            //more efficient than Vector3.normalized
            if (directionSqrLength > 1.5f)
            {
                direction *= DIAGONAL_COEFFICIENT;
            }
            _controller.Move(direction * Time.deltaTime * _moveSpeed);
        }
    }
}
