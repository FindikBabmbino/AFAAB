using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonFollow : MonoBehaviour
{
    //This will be the gameplay camera that Sarp will use 
    [SerializeField]private GameObject gimbel;
    [SerializeField]private GameObject followCamera;
    //This will hold the mouse sensitivity but we will probably move this to somewhere else in the future.
    [SerializeField]private float mouseSensitivity=1.0f;
    //This will hold the minumum and the maximum values the pitch of the camera will have
    [SerializeField]private Vector2 pitchMinMax=new Vector2(-90,90);
    //This will give smooth rotation
    [SerializeField]private float rotationSmoothTime = 0.12f;
    //This will be the distance of the camera from the target
    [SerializeField]private float distanceOfTheCamera=10f;
    //These will hold the reference smooth velocity and current rotation
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;
    private PlayerInput playerInput;


    private void Start()
    {
        if(gimbel==null)
        {
            Debug.LogError("Missing the gimbel!");
            return;
        }
        followCamera=this.gameObject;
        //Get the gimbel 
        gimbel=GameObject.Find("/Player/Gimbel");
        playerInput=gimbel.GetComponentInParent<PlayerInput>();
    }
    private void Update()
    {
       CalculateCameraMovement();
    }

    private void CalculateCameraMovement()
    {
        //These variables will hold the 
        float yaw=0;
        float pitch=0;
        yaw+=playerInput.actions["LookAround"].ReadValue<Vector2>().x;
        pitch-=playerInput.actions["LookAround"].ReadValue<Vector2>().y;
        //Clamp the pitch so the player can't do stupid angles 
        pitch=Mathf.Clamp(pitch,pitchMinMax.x,pitchMinMax.y);
        //I dont really know how this works this is from the old red cinder block project younger saynen was smarter
        //I mean when I look at it it is simple but how the hell was I able to think about this months ago>
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        followCamera.transform.eulerAngles+=currentRotation;
        followCamera.transform.position=gimbel.transform.position-followCamera.transform.forward*distanceOfTheCamera;
    }
}