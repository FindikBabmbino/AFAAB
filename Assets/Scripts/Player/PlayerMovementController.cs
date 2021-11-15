using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMovementController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]private float walkSpeed=5f;
    [SerializeField]private float runSpeed=2.0f;

    [Header("Raycast stats")]
    [SerializeField]private float rayCastReach=10.0f;

    [Header("Camera")]
    //This weill not be referenced to the camera anymore it will be referenced to a gameobject that will follow the cameras yaw rotation
    [SerializeField]private Transform cameraTransform;
    private Vector3 moveDirection;
    //this will be used to calculate players slope normal
    private RaycastHit slopeHit;
    //This will be used to store players slope movement
    private Vector3 slopeMovementDirection;
    //If this returns true we will change the players movement direction to this
    private bool isOnSlope;
    //Get the playermovement input system
    private PlayerInput playerInput;
    private Rigidbody rigidBody;
    //This gets the last direction the player was heading this is used to decided where the player is going to dash.
    private Vector3 DashDirection; 

    private void Awake()
    {
        //This sets the playerinput 
        playerInput = GetComponent<PlayerInput>();
        rigidBody=GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        //Subscribe to events when starting the script
    }

    private void OnDisable()
    {
        //Unsub to avoid memory leaks
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //calculates the slope normal so that the player does not stick to it
        slopeMovementDirection=Vector3.ProjectOnPlane(moveDirection,slopeHit.normal);
    }

    void FixedUpdate()
    {
        //Does the calculation for the players movement
        PlayerMovementCalculation();
    }

    void PlayerMovementCalculation()
    {
        //For some reason the calculation of the player movement direction and adding the force has to be on the same function if not it does not work.
        //If this returns true we wont let the player move.
        //We also don't want the player to move if they are in a dialogue.
        if(GetComponent<DashSystem>().GetIsDashing()||ChatterBoxManager.instance.ReturnPlayerIsInDialogue())
        {
            Debug.Log("called");
            return;
        }
        //we get the cameras right and forward vector to set where player is going head.
          if(cameraTransform!=null)
        {
            Vector2 movementAxis=playerInput.actions["Movement"].ReadValue<Vector2>();
            moveDirection=new Vector3(movementAxis.x,0,movementAxis.y);
            moveDirection=moveDirection.x*cameraTransform.transform.right+moveDirection.z*cameraTransform.transform.forward;
            //We do a check if it is not 0 this is done so it does not reset the players rotation when the character does not move
            //This check used to be moveDirection.x!=0&&moveDirection.y!=0&&moveDirection.z!=0 this is because it was the cinemachibe camera before
            if(moveDirection.x!=0&&moveDirection.z!=0)
            {
                Debug.Log("in movedirection check");
                //This makes the character look at the where they are going
                //TODO-We clamp the x and z so the player does not rotate to weird angles
                transform.localRotation=Quaternion.LookRotation(moveDirection);
            }
            //Set the y to be zero to be sure 
            moveDirection.y=0;
        }
        else
        {
            Debug.Log("warning the camera is missing");
        }
        // If the player is not on a slope use moveDirection.
        if(!OnSlope())
        {
            //Multiply it by delta time here because we can't call this from fixed update
            rigidBody.MovePosition(transform.position+(moveDirection.normalized*walkSpeed*Time.deltaTime));
        }
        else
        {
            rigidBody.MovePosition(transform.position+(slopeMovementDirection.normalized*walkSpeed*Time.deltaTime));
        }
        //We do this check to avoid no directions.
        if(moveDirection.x!=0&&moveDirection.z!=0)
        {
           DashDirection=moveDirection;
        }
    }

    //Returns true if the player is on a slope
    bool OnSlope()
    {
        if(Physics.Raycast(transform.position,Vector3.down,out slopeHit,1))
        {
            //If the player is on a slope the normal will not return a vector up since it is at an incline.
            if(slopeHit.normal!=Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void CalculateGroundHit()
    {
    }
    //This will be used to call it and use it from other script
    public Vector3 GetDashDirection()
    {
        return DashDirection;
    }
}