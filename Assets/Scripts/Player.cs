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

        //checking for the collision based on Raycasting
        float moveDistance = _playerSpeed * Time.deltaTime; // Speed = Distance/time
        float playerHeight = 2f;
        float playerRadius = 0.7f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        
        if(!canMove)
        {
            //cannnot move towards moveDir

            //attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
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

                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if(canMove)
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

    public bool IsWalking()
    {
        return isWalking;
    }
}
