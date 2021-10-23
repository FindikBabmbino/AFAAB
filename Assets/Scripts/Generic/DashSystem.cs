using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashSystem : MonoBehaviour
{
    [Header("Dash stats")]
    [SerializeField]private float dashPower=10.0f;
    [SerializeField]private int dashLimit=1;
    [Header("Effects")]
    [SerializeField]private AudioClip dashSfx;
    [SerializeField]private ParticleSystem dashVFX;
    [Header("Misc")]
    [SerializeField]private PlayerInput playerInput;
    [SerializeField] private Transform cameraTransform;
    private Rigidbody rigidBody;
    private bool isDashing;

     private void Start()
     {
        playerInput=GetComponent<PlayerInput>();
        rigidBody=GetComponent<Rigidbody>();
        cameraTransform=Camera.main.transform;
     }


    private void OnEnable()
    {
        playerInput.actions["Dash"].performed+=DashCoroutineStarter;
    }
    private void OnDisable()
    {
      playerInput.actions["Dash"].performed-=DashCoroutineStarter;
    }

     private void DashCoroutineStarter(InputAction.CallbackContext context)
    {
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
         //DONE--TODO also use isdashing to make the character unable to be controlled by WASD--DONE
         //TODO decrease dashlimit after use and make a function that will regenerate it.

        //We don't want the player to be able to dash if they are not in combat
        if(!CombatEventSystemManager.instance.GetPlayerIsInBattle())
        {
            yield break;
        }
        isDashing=true;  
         //If the limit is maxed out just return out of it 
        if(dashLimit<=0)
        {
            yield break;
        }
        //Check if these are null so we dont crash the game
        if(dashVFX!=null)
        {
            dashVFX.Play();
        }
        if(dashSfx!=null)
        {
            //We don't need a source we can just create it and destroy it here.
            AudioSource.PlayClipAtPoint(dashSfx,transform.position);
        }
        //Get it in a variable to check if the x and z are 0
        Vector3 DashDirection=GetComponent<PlayerMovementController>().GetDashDirection();
        //We check if the x and z are 0
        if(DashDirection.x==0&&DashDirection.z==0)
        {
            //If it is we set the players forward as the direction 
            DashDirection=transform.forward;
        }
        rigidBody.AddForce(DashDirection*dashPower,ForceMode.VelocityChange);
        rigidBody.velocity=Vector3.zero;
        yield return new WaitForEndOfFrame();
        yield return null;
        isDashing=false;
    }
    //This will be called from the player controller to check if the player is using dash
    public bool GetIsDashing()
    {
        return isDashing;
    }
}