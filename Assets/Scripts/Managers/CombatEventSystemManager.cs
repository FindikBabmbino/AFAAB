using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEventSystemManager : MonoBehaviour
{
    public static CombatEventSystemManager instance;

    //This bool will be turned true when Sarp enters combat thanks to this bool we will be able to make Sarp fight set up events and music
    [SerializeField]private bool playerIsInBattle;

    //List of enemies will be kept to determine when the playerIsInBattle should be set to false when an enemy is defeated they are subtracted from the list.
    [SerializeField] private List<GameObject> enemies=new List<GameObject>();
    //This bool will be used to start the GetEnemiesInScene function
    private bool startGettingEnemies;
    //This will be set to true after the list is filled to stop the list being filled with duplicates
    private bool gotEnemies;

    private void Awake()
    {
        if(instance==null)
        {
            instance=this;
            //We want this to persist through the game
            DontDestroyOnLoad(gameObject);
        }
        else if(instance!=this)
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        //If this is true meaning the player is in combat we get all the enemies.
        if(playerIsInBattle)
        {
            //If got enemies is false that means our list is empty
            if(!gotEnemies)
            {
                startGettingEnemies=true;
            }
            //else our list is full and we don't need to run the list anymore if we run it more the list will be filled with duplicates
            else
            {
                startGettingEnemies=false;
            }
            if(startGettingEnemies)
            {
                GetEnemiesInScene();
            }
            //If the enemy count reaches 0 we get out of the battle state.
            if(enemies.Count<=0)
            {
                SetIsInBattle(false);
                //After the battle is over the list is cleared up we set this to false again so in the next loop we can get the enemies.
                gotEnemies=false;
            }
        }
    }
    //This will be called from other classes to check if it returns true or false
    public bool GetPlayerIsInBattle()
    {
        return playerIsInBattle;
    }
    //This will be used to set the combat state from other scripts
    public void SetIsInBattle(bool var)
    {
        playerIsInBattle=var;
    }
    //When this is initiated it will get all the enemies in the scene
    private void GetEnemiesInScene()
    {
        //In here we get all the enemies and after that we set gotenemies to true so we can set the startgettingenemies to false, this way we can stop getting the enemies.
       enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
       gotEnemies=true;
    }
    //This will be used to remove  defeated enemies from the list this will most likely be called from a different script (most likely health)
    public void RemoveEnemiesFromTheList(GameObject enemyToRemove)
    {
        //We return out if the list is null
        if(enemies.Count<=0)
        {
            return;
        }
        //If it does not contain it don't try to remove it
        if(!enemies.Contains(enemyToRemove))
        {
            return;
        }
        for(int i=0;i<=enemies.Count;i++)
        {
            //For some reason saying not equels to removes it.
            if(enemies[i]!=enemyToRemove)
            {
                enemies.Remove(enemyToRemove);
            }
            else
            {
                return;
            }
        }
    }
}