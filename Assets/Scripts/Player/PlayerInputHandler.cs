using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2Int movementInput;
    public bool jumpInput;
    public bool castInput;

    [SerializeField]
    private float inputHoldTime = 0.1f;

    private float jumpInputStartTime;

    private void Update()
    {
        #region Horizontal Input
        if (Input.GetKey(KeyCode.D)) {
            movementInput.x = 1;
        } 
        else if (Input.GetKey(KeyCode.A)) {
            movementInput.x = -1;
        }
        else {
            movementInput.x = 0;
        }
        #endregion

        #region Vertical Input
        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpInput = true;
            jumpInputStartTime = Time.time;
        }
        else
        {
            CheckJumpInputHoldTime();
        }
        #endregion

        #region Cast Input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            castInput = true;
        }
        else
        {
            castInput = false;
        }
        #endregion

    }

    #region Misc Functions
    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            jumpInput = false;
        }
    }
    #endregion
}
