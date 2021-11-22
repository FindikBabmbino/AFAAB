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
    [SerializeField]private PlayerInput playerInput;
    private Rigidbody rigidBody;
    //This gets the last direction the player was heading this is used to decided where the player is going to dash.
    private Vector3 DashDirection; 
    //This will hold the movement commands Vector2
    private Vector2 movementVector2;

    private void Awake()
    {
  
        rigidBody=GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        //Subscribe to events when starting the script
        //This sets the playerinput 
        playerInput = GetComponent<PlayerInput>();
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
        //For some reason with the new update we have to get the input from the update and store it here before this we could just call the value of the vector 2 in the movement part 
        movementVector2=playerInput.actions["Movement"].ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        //Does the calculation for the players movement
        PlayerMovementCalculation(movementVector2);
    }

    void PlayerMovementCalculation(Vector2 var)
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
            Vector2 movementAxis=var;
            moveDirection=new Vector3(movementAxis.x,0,movementAxis.y);
            moveDirection=moveDirection.x*cameraTransform.transform.right+moveDirection.z*cameraTransform.transform.forward;
            //We do a check if it is not 0 this is done so it does not reset the players rotation when the character does not move
            //This check used to be moveDirection.x!=0&&moveDirection.y!=0&&moveDirection.z!=0 this is because it was the cinemachibe camera before
            if(moveDirection.x!=0&&moveDirection.z!=0)
            {
                Debug.Log("in movedirection check");
                //This makes the character look at the where they are going
                //TODO-We clamp the x and z so the player does not rotate to weird angles--DONE
                //First create a quaternion that holds the lookrotation
                Quaternion lookRotation=Quaternion.LookRotation(moveDirection);
                //Then create a new quaternion taking the players normal eulerangles of x and z but take the euler angle of the quaternion that holds the lookrotation
                Quaternion lookAtYAxis=Quaternion.Euler(transform.eulerAngles.x,lookRotation.eulerAngles.y,transform.eulerAngles.z);
                //Then put it into the players rotation
                transform.rotation=lookAtYAxis;
            }
        }
        else
        {
            Debug.Log("warning the camera is missing returning out of this function");
            return;
        }
        //Just to be sure set the movedirection.y to zero
        moveDirection.y=0;
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