using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public static MusicManager instance;

    //We will probably just do AudioSource.PlayClipAtPoint rather then use this.
    //Using this might be a better idea but still we might want to convert it
    //We seperate them so we can switch between states.
    [SerializeField] private AudioSource jukeBoxIntro;
    [SerializeField] private AudioSource jukeBoxLoop;
    [SerializeField] private AudioSource jukeBoxOutro;
    //These will be the overworld themes we will add more to the list when we want more
    //Since we won't probably add any more to the array while we are in the game I think we can keep it an array rather then a list.
    //Since we want the music to loop we need to chop it into pieces.
    [SerializeField] private AudioClip[] overWorldIntro;
    [SerializeField] private AudioClip[] overWorldLoop;
    [SerializeField] private AudioClip[] overWorldOutro;
    //These will be pulled from a game object so there is no need to make them SerializeField.
    private AudioClip missionIntro;
    private AudioClip missionLoop;
    private AudioClip missionOutro;
    //This will store every music clips for the side stories.
    [SerializeField] private AudioClip[] sideStoryClips;

    //This will keep hold of the random music.
    private int randMusicToSelect;
    //This is so that the game does not constantly decide what it is going to play.
    private bool ableToDecideMusic;
    //This bool controls the music to be pulled to mission musics.
    private bool pullMusic;

    private void Awake()
    {
        if(instance==null)
        {
            instance=this;
        }
        else if(instance!=this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }


   private void Update()
    {
        //Constantly check if the player is in battle.
        if(CombatEventSystemManager.instance.GetPlayerIsInBattle())
        {
            if(!CombatEventSystemManager.instance.GetPlayerIsInMission())
            {
                if(jukeBoxIntro.clip==null&&jukeBoxLoop.clip==null)
                {
                    ableToDecideMusic=true;
                }
                DecideMusicToPlay();
                CombatMusicState();
            }
        }
    }

    //This will only be used when Sarp is in the overworld if he is in a mission a mission specific music will be pulled but since we want multiple music variations in the overworld we need this.
    private void DecideMusicToPlay()
    {
        if(ableToDecideMusic)
        {
            ableToDecideMusic=false;
            if(overWorldIntro.Length!=0)
            {
                randMusicToSelect=Random.Range(0,overWorldIntro.Length);
                jukeBoxIntro.clip=overWorldIntro[randMusicToSelect];
                jukeBoxIntro.Play();
            }
            //If this is null just pull from the loop
            else if(overWorldLoop.Length!=0)
            {
                randMusicToSelect=Random.Range(0,overWorldLoop.Length);
                jukeBoxLoop.clip=overWorldLoop[randMusicToSelect]; 
            }
        }
    }

    //This will controll the states of the music from intro to loop to outro
    private void CombatMusicState()
    {
        //If the intro has stopped playing we have to switch to the loop
        //If the intro does not exist also go into the loop
        //Also check if the loop is not playing if not it will constantly reset the music
        if(!jukeBoxIntro.isPlaying||jukeBoxIntro.clip==null)
        {
            //If the player is not in a mission that means he is in the overworld so just change the overworld
            if(!CombatEventSystemManager.instance.GetPlayerIsInMission()&&jukeBoxLoop.clip==null)
            {
                jukeBoxLoop.clip=overWorldLoop[randMusicToSelect];
            }
            else if(jukeBoxLoop.clip==null)
            {
                jukeBoxLoop.clip=missionLoop;
            }
            if(jukeBoxLoop.clip!=null&&!jukeBoxLoop.isPlaying)
            {
                jukeBoxLoop.Play();
            }
        }

        //If the player defeats all the enemies or clears the mission that means he is not in a battle if so we stop the loop and null its clip
        if(!CombatEventSystemManager.instance.GetPlayerIsInBattle())
        {
            jukeBoxLoop.Stop();
            jukeBoxLoop.clip=null;
            //If the player is in the overworld play the outro appropriate to the theme that has been chosen.
            //If the player clears a mission though we should play the outro that is appropriate to that mission.
            if(!CombatEventSystemManager.instance.GetPlayerIsInMission())
            {
                jukeBoxOutro.clip=overWorldOutro[randMusicToSelect];
            }
            else
            {
                jukeBoxOutro.clip=missionOutro;
            }
            if(jukeBoxOutro.clip!=null&&!jukeBoxOutro.isPlaying)
            {
                jukeBoxOutro.Play();
            }
        }
    }
}