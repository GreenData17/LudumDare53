using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    [Header("Data")]
    public float delay = 10f;
    public float _delay;

    [Header("Setup?")]
    public Vector2 spawnArea = Vector2.one;
    public GameObject[] Boxes;

    void Start(){
        _delay = delay;
    }

    void Update(){
        if(_delay >= 0f){
            _delay -= Time.deltaTime;
        }else{
            Spawn();
            _delay = delay;
        }
    }

    void Spawn(){
        float X = Random.Range(transform.position.x + -(spawnArea.x / 2), transform.position.x + spawnArea.x / 2);
        float Y = Random.Range(transform.position.y + -(spawnArea.y / 2), transform.position.y + spawnArea.y / 2);

        for (int i = 0; i < Boxes.Length; i++)
        {
            if(!Boxes[i].activeInHierarchy){
                Boxes[i].GetComponent<Box>().RandomPosition(spawnArea);
                Boxes[i].GetComponent<Box>().RandomSprite();
                Boxes[i].SetActive(true);
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(spawnArea.x, spawnArea.y, 0));
    }
}
