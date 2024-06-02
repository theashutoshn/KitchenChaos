using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string Is_Walking = "IsWalking";
    private Animator _animator;
    
    [SerializeField]
    private Player _player;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.Log("_animator is not null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // below line can also be written as 
        // _animator.SetBool("IsWalking", true/false)
        // bascially, we have converted the "IsWalking" to string Is_Walking and the _player.IsWalking(), this method gives use the value of true or false from the player script
        _animator.SetBool(Is_Walking, _player.IsWalking());
    }

    
}
