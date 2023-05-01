using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Vector2 currentSpawnArea;

    [Header("references")]
    public GameObject smallBox;
    public GameObject bigBox;


    public void RandomSprite(){
        float ran = Random.Range(0,100);

        if(ran <= 60){
            smallBox.SetActive(true);
            bigBox.SetActive(false);
        }else{
            smallBox.SetActive(false);
            bigBox.SetActive(true);
        }
    }

    public void RandomPosition(Vector2 spawnArea){
        currentSpawnArea = spawnArea;

        float X = Random.Range(transform.position.x + -(spawnArea.x / 2), transform.position.x + (spawnArea.x / 2));
        float Y = Random.Range(transform.position.y + -(spawnArea.y / 2), transform.position.y + (spawnArea.y / 2));

        transform.position = new Vector3(X, Y, 0);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Island"){
            RandomPosition(currentSpawnArea);
        }else if(col.tag == "Player"){
            if(GameManager.instance.AddBoxToInventory(bigBox.activeInHierarchy ? true : false))
                gameObject.SetActive(false);
        }
    }
}
