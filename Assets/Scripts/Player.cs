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

    private Vector3 lastInteractDir;
    void Start()
    {
        
    }

   
    void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleInteraction()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if(moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir; // creating lastInteractDir because once the player is near the counter, it should continously be there. Which means the ray should be contiusely 
                                       // hittign the object
        }
        float interactDistance = 2f; // distance between player and counter
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, lastInteractDir, out raycastHit ,interactDistance))
        {
            Debug.Log(raycastHit.transform);
        }
        
    }

    private void HandleMovement()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        //checking for the collision based on Raycasting
        float moveDistance = _playerSpeed * Time.deltaTime; // Speed = Distance/time
        float playerHeight = 2f;
        float playerRadius = 0.7f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //cannnot move towards moveDir

            //attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // can move only on X
                moveDir = moveDirX;
            }
            else
            {
                //cannot move only on X

                //Attempt only Z movement

                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    //can move only on the z
                    moveDir = moveDirZ;
                }
                else
                {
                    // we cannot move in any direcrtion
                }
            }


        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }



        // the below line basically translates to isWalking = true;
        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10.0f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
}
