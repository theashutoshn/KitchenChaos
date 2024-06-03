using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputAction _playerInputAction;

    private void Awake()
    {
       _playerInputAction = new PlayerInputAction();
        _playerInputAction.Player.Enable();
    }
    public Vector2 GetMovementVectorNormalized()
    {
        //Vector2 inputVector = new Vector2(0, 0);
        
        /* if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }
        */

        Vector2 inputVector = _playerInputAction.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
