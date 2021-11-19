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
    [SerializeField]private Vector2 pitchMinMax=new Vector2(-5,35);
    private Vector2 test=new Vector2(-3,35);
    //This will give smooth rotation
    [SerializeField]private float rotationSmoothTime = 0.12f;
    //This will be the distance of the camera from the target
    //This will be set in the start it will be set to minmax y
    [SerializeField]private float distanceOfTheCamera;
    [Header("Collision")]
    [SerializeField]private float radiousOfSphereCast=10f;
    //This will change the distance of the camera based on if the camera is colliding with something
    [SerializeField]private Vector2 minMaxOfCamera=new Vector2(0.5f,10f);
    private Vector3 cameraDirection;
    //These will hold the reference smooth velocity and current rotation
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;
    //While this will follow the camera and stop when it hits a wall keeping the original distance
    [SerializeField]private GameObject dummyCamera;
    //This controls the lerping of the new camera distance values
    [SerializeField]private float cameraDistanceLerpAmount=0.5f;
    private PlayerInput playerInput;


    private void Start()
    {
        if(gimbel==null)
        {
            Debug.LogError("Missing the gimbel!");
            return;
        }
        dummyCamera=GameObject.Find("CameraDummy");
        followCamera=this.gameObject;
        gimbel=GameObject.Find("/Player/Gimbel");
        playerInput=gimbel.GetComponentInParent<PlayerInput>();
        distanceOfTheCamera=minMaxOfCamera.y;
    }

    private void LateUpdate()
    {
        CalculateCameraMovement();
    }

    private void Update()
    {
        //Keep updating the position between the camera and the gimbel 
        cameraDirection=gimbel.transform.position;
        //Distance of the dummy should always be the max 
        CalculateCameraCollision();
    }

    private void CalculateCameraMovement()
    {
        //These variables will hold the x and y values of the mouse 
        float pitch=0;
        float yaw=0;
        //Clamp the pitch so the player can't do stupid angles 
        yaw+=playerInput.actions["LookAround"].ReadValue<Vector2>().x*mouseSensitivity;
        pitch-=playerInput.actions["LookAround"].ReadValue<Vector2>().y*mouseSensitivity;
        pitch=Mathf.Clamp(pitch,pitchMinMax.x,pitchMinMax.y);
     
        //I dont really know how this works this is from the old red cinder block project younger saynen was smarter
        //I mean when I look at it it is simple but how the hell was I able to think about this months ago>
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch,yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        followCamera.transform.eulerAngles+=currentRotation;
        followCamera.transform.position=gimbel.transform.position-followCamera.transform.forward*distanceOfTheCamera; 
        //This will match the follow of the camera with the dummy camera
        dummyCamera.transform.eulerAngles+=currentRotation;
        dummyCamera.transform.localPosition=gimbel.transform.position-dummyCamera.transform.forward*minMaxOfCamera.y;               
    }
    

    //This function will do the the spherecast calculations
    //There needs to be an external dummy camera on the scene that will keep track of the original distance of the camera, if not the camera changes it's pos therefore changing it's distance so we get a terrible effect
    //In the future we might find a better effect 
    private void CalculateCameraCollision()
    {
        RaycastHit hit;
        Debug.DrawLine(dummyCamera.transform.position,cameraDirection,Color.red);
        if(Physics.Linecast(dummyCamera.transform.position,cameraDirection,out hit))
        {
            //Change the min max of distance of camera
            //We clamp it to the hit distance of the ray
            float newDistance=Mathf.Clamp(hit.distance,minMaxOfCamera.x,minMaxOfCamera.y);
            distanceOfTheCamera=Mathf.Lerp(distanceOfTheCamera,newDistance,cameraDistanceLerpAmount*Time.deltaTime);
            Debug.Log(cameraDirection.magnitude);
        }
        else
        {
            distanceOfTheCamera=Mathf.Lerp(distanceOfTheCamera,minMaxOfCamera.y,cameraDistanceLerpAmount*Time.deltaTime);
        }
    }
}