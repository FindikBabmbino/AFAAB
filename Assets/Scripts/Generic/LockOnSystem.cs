using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class LockOnSystem : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform targetTransform;

    [Header("Variables")]
    [SerializeField] private bool lockedOn;
    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private PlayerInput playerInput;


    private void Start()
    {
        player=GameObject.FindObjectOfType<PlayerMovementController>().gameObject;
    }

    private void OnEnable()
    {
        //In here we subscribe to the lock on key so we can lock on to targets
        playerInput=GetComponent<PlayerInput>();
        playerInput.actions["LockOn"].performed+=SetLockOnTrue;
        playerInput.actions["LockOn"].canceled+=SetLockOnFalse;
    }
    private void OnDisable()
    {
        //Unsubscribe to avoid memory leaks
        playerInput.actions["LockOn"].performed-=SetLockOnTrue;
        playerInput.actions["LockOn"].canceled-=SetLockOnFalse;
    }

    private void Update()
    {
        LockOn();
        //Since we don't want to retarget the player while they are locked on we will only find the target when the player is not locked on.
        if(!lockedOn)
        {
            FindTargetToLockOnTo();
        }
    }
    private void LockOn()
    {
        //This makes it return out of the function if the player is not in combat
        if(!CombatEventSystemManager.instance.GetPlayerIsInBattle())
        {
            return;
        }
        if(lockedOn)
        {
            //Thıs for clamping the rotation of the x and z rotation
            Vector3 lockOnRotation=new Vector3(targetTransform.position.x,transform.position.y,targetTransform.position.z);
            //When we do look at the player starts orbiting the target
            player.transform.LookAt(lockOnRotation);
            //We switch to this mode so that the camera is locked to the players back
            freeLookCamera.m_BindingMode=CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;
            //We close this script so that the player can't move the camera
            //freeLookCamera.GetComponent<CinemachineInputProvider>().enabled=false;
        }
        else
        {
            player.transform.LookAt(null);
            freeLookCamera.m_BindingMode=CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
            freeLookCamera.GetComponent<CinemachineInputProvider>().enabled=true;
        }
    }
    //Set true will be called when it is performed while set false will be called when it is cancelled 
    private void SetLockOnTrue(InputAction.CallbackContext context)
    {
        lockedOn=true;
    }
    private void SetLockOnFalse(InputAction.CallbackContext context)
    {
        lockedOn=false;
    }

    private void FindTargetToLockOnTo()
    {
        GameObject[] Enemies=GameObject.FindGameObjectsWithTag("Enemy");
        //Since we want to get something that is closes to the player and since we don't have a set distance we use infinity.
        float minDistance=Mathf.Infinity;
        foreach(var enemy in Enemies)
        {
            Vector3 lockOnTarget=enemy.transform.position-player.transform.position;
            //Squaring a value makes the code more optimised since the values are not linear and are multiplied.
            float distanceSqrd=lockOnTarget.sqrMagnitude;
            if(distanceSqrd<minDistance)
            {
                //We change the mindistance to distancesqrd when we find the target we do this so we can compare the last closest enemy to the new ones distance(?) 
                minDistance=distanceSqrd;
                targetTransform=enemy.transform;
            }
        }
    }
}