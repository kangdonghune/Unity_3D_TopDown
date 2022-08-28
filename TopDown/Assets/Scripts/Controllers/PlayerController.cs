using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variable

    public float speed = 5.0f;
    public float jumpHeight = 2.0f;
    public float dashDistance = 5.0f;
    public float gravity = -9.81f;
    public Vector3 drags;
    public LayerMask groundLayerMask;
    public float groundCheckDistance = 0.3f;

    private CharacterController _characterController;
    private Vector3 inputDirection = Vector3.zero;
    private bool _isGrounded = false;
    private Vector3 calcVelocity;



    #endregion

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        _isGrounded = _characterController.isGrounded;
        if (_isGrounded && calcVelocity.y < 0)
            calcVelocity.y = 0.0f;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _characterController.Move(move * Time.deltaTime * speed); //move는 중력계산 x SimpleMove는 중력 계산 및 델타타임도 자동으로 곱해줌

        if(move != Vector3.zero)
        {
            transform.forward = move;
        }

        if(Input.GetButton("Jump") && _isGrounded)
        {
            calcVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        if(Input.GetButtonDown("Dash"))
        {
            Debug.Log("Dash");
            calcVelocity += Vector3.Scale(transform.forward, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drags.x + 1)) / -Time.deltaTime),
                0,
                (Mathf.Log(1f / (Time.deltaTime * drags.z + 1)) / -Time.deltaTime))
                );
        }

        //
        calcVelocity.y += gravity * Time.deltaTime;
        calcVelocity.x /= 1 + drags.x * Time.deltaTime;
        calcVelocity.y /= 1 + drags.y * Time.deltaTime;
        calcVelocity.z /= 1 + drags.z * Time.deltaTime;

        _characterController.Move(calcVelocity * Time.deltaTime);

    }
}
