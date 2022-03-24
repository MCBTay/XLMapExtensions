using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnpoint : MonoBehaviour
{
    // Vector 3 position
    public Gameobject[] objects;

    void Start()
    {
        objects = (GameObject.FindGameObjectsWithTag("SpawnPoint"));

        int randomNumber = Random.Range(0, (objects.Length - 1));
        Debug.Log(objects[randomNumber].transform.position);



        
    }
}
