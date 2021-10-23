using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
   [Header("Stats of attack")]
   [SerializeField]private float damage=0.0f;
   //One of these will be used to increase the fly energy of the attacker if the hit connects and one will be used to decrease the energy of the recipient again if the hit connects.
   [SerializeField]private float flyEnergyToAdd=0.0f;
   [SerializeField]private float flyEnergyToDecrease=0.0f; 
   [Header("Audio")]
   [SerializeField]private AudioSource effectPlayer;
   [SerializeField]private AudioClip startAnimationAudio;
   [SerializeField]private AudioClip hitAnimationAudio;
   [Header("Particle effects")]
   [SerializeField]private ParticleSystem startingEffect;
   [SerializeField]private ParticleSystem hitEffect;
   [Header("Misc")]
   [SerializeField]private BoxCollider hitBox;


    private void OnEnable()
    {
        PlayStartingEffects();
    }
    private void PlayStartingEffects()
    {
        //TODO use AudioSource.PlayClipAtPoint rather then creating an audiosource
        //This will be called when the animation starts playing the audio and effects
        effectPlayer.clip=startAnimationAudio;
        effectPlayer.loop=false;
        effectPlayer.clip=startAnimationAudio;
        effectPlayer.Play();
        startingEffect.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Set the audio and effects to play
        effectPlayer.clip=hitAnimationAudio;
        effectPlayer.Play();
        hitEffect.Play();

        //Gives fly energy to the attacker
        if(GetComponent<FlySystem>())
        {
            GetComponent<FlySystem>().IncreaseFlyAmount(flyEnergyToAdd);
        }
        //This one will be used to decrease the fly energy of the recipient.
        if(other.gameObject.GetComponent<FlySystem>())
        {
            other.gameObject.GetComponent<FlySystem>().DecreaseFlyAmount(flyEnergyToDecrease);
        }
    }    
}