using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public static Player Instance { get; private set; }
    

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs //additional event data for storing which counter was selected
    {
        public ClearCounter selectedCounter; // this selectedCounter is different then the below private ClearCounter selectedCounter;
    }
    
    private float _playerSpeed = 7f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private bool isWalking;
    [SerializeField] private LayerMask countersLayerMask;
    
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;


    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than One Player Instance");
        }
        Instance = this;
    }
    private void Start()
    {
        _gameInput.OnInteractionAction += GameInput_OnInteractionAction;
    }

    private void GameInput_OnInteractionAction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact();
        }
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
                                       // hittign the object. We used lastInteractDir variable just so that we can get the hit data continually. This will ensure that the interaction with respective objects will be carry on until the player is near the object
        }
        float interactDistance = 2f; // distance between player and counter


        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, lastInteractDir, out raycastHit ,interactDistance, countersLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) // TryGetCompnentis better than GetCOmponent as it handles the null check for itself. this same can be written as
                                                                                    // ClearCounter clearCounter = (raycastHit.transform.GetComponent<ClearCounter>();  
                                                                                    // if(clearCounter != null)..................
            {
                if(clearCounter != selectedCounter)
                {
                    //selectedCounter = clearCounter;
                    SetSelectedCounter(clearCounter);
                }
                
            }
            else // Case 1: If Raycast hit something and that something is not the counter
            {
                //selectedCounter = null;
                SetSelectedCounter(null);
            }
        }
        else // Case 2: if Raycast doesn't hit anything, meaing there is nothing in front of player
        {
            //selectedCounter = null;
            SetSelectedCounter(null);
        }

        Debug.Log(selectedCounter);
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

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        
        this.selectedCounter = selectedCounter; // This line assigns the value of the parameter selectedCounter to the private field selectedCounter basically, private ClearCounter selectedCounter = clearCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
}
