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
    [SerializeField]private Transform cameraTransform;
    [SerializeField]private bool isEnemy;
    private Rigidbody rigidBody;
    [SerializeField]private bool isDashing;

    [SerializeField]private float dashCoolDownAmount;
    //This will hold the dashlimit so we can then set the dashlimit again to replenish it
    private float waitForDashReplenish;

    private int defaultDashAmount;

     private void Start()
     {
        playerInput=GetComponent<PlayerInput>();
        rigidBody=GetComponent<Rigidbody>();
        cameraTransform=Camera.main.transform;
        defaultDashAmount=dashLimit;
     }


    private void OnEnable()
    {
        //Only bind if it is the player if we don't do this check we get errors let's try to avoid them as much as we can
        if(!isEnemy)
        {
            playerInput.actions["Dash"].performed+=DashCoroutineStarter;
        }
    }
    private void OnDisable()
    {
        if(!isEnemy)
        {
            playerInput.actions["Dash"].performed-=DashCoroutineStarter;
        }
    }

    private void Update()
    {   //If it is zero or is dashing is false we start the replenish process
        if(dashLimit<=0||!isDashing)
        {
            if(waitForDashReplenish<dashCoolDownAmount)
            {
                //We start the countdown
                waitForDashReplenish+=Time.deltaTime;
                if(waitForDashReplenish>=dashCoolDownAmount)
                {
                    dashLimit=defaultDashAmount;
                    //Set it to zero so that it stops counting
                    waitForDashReplenish=0;
                }
                else if (isDashing)
                {
                    //If player dashes again cancel the process by setting it to zero
                    waitForDashReplenish=0;
                }
            }
        }
    }
    private void DashCoroutineStarter(InputAction.CallbackContext context)
    {
        StartCoroutine(DashCoroutine());
    }

    //Investigate transform.translate more it could be better then rb.addforce
    private IEnumerator DashCoroutine()
    {
         //DONE--TODO also use isdashing to make the character unable to be controlled by WASD--DONE
         //TODO decrease dashlimit after use and make a function that will regenerate it-DONE.

        //We don't want the player to be able to dash if they are not in combat
        //We also don't want the player to be able to dash if they are in a dialogue if they are in a battle they most likely won't get into one but just to be sure we should still check.
        if(!CombatEventSystemManager.instance.GetPlayerIsInBattle()||ChatterBoxManager.instance.ReturnPlayerIsInDialogue())
        {
            yield break;
        }
        //If the limit is maxed out just return out of it 
        if(dashLimit<=0)
        {
            yield break;
        }
        isDashing=true;  
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
        Vector3 dashDirection=GetComponent<PlayerMovementController>().GetDashDirection();
        //We check if the x and z are 0
        if(dashDirection.x==0&&dashDirection.z==0)
        {
            //If it is we set the players forward as the direction 
            dashDirection=transform.forward;
        }
        //Rigidbody.moveposition seems to be better then addforce but i will comment this out if we ever want to go back.
        //rigidBody.AddForce(dashDirection*dashPower*Time.deltaTime,ForceMode.VelocityChange);
        rigidBody.MovePosition(transform.position+(dashDirection.normalized*dashPower*Time.deltaTime));
        //Since we are now using moveposition we don't need this too
        //rigidBody.velocity=Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        yield return null;
        isDashing=false;
        dashLimit--;
    }
    //This will be called from the player controller to check if the player is using dash
    public bool GetIsDashing()
    {
        return isDashing;
    }

    //This will be called from the enemy combat ai to make the ai dash but the problem is that the ai can only dash to right we have to make him be able to dash to other directions as well
    //Or we can seperate all of them and call them depending on the state (e.g AIDashrigh AIDashLeft) so on and so forth
    public IEnumerator AIDash()
    {
        if(dashLimit<=0)
        {
            yield break;
        }
        isDashing=true;
        Vector3 dashDirection=transform.right;
        rigidBody.MovePosition(transform.position+(dashDirection.normalized*dashPower*Time.deltaTime));
        yield return null;
        isDashing=false;
        dashLimit--;
    }
}