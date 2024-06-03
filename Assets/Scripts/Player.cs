using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    private float _playerSpeed = 7f;
    [SerializeField]
    private GameInput _gameInput;

    [SerializeField]
    private bool isWalking;
    void Start()
    {
        
    }

   
    void Update()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * _playerSpeed * Time.deltaTime;

        // the below line basically translates to isWalking = true;
        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10.0f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
