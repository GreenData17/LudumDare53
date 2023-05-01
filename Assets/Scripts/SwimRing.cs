using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimRing : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player"){
            var magnitude = 2500;
 
            var force = transform.position - col.transform.position;

            force.Normalize();
            col.gameObject.GetComponent<Rigidbody2D>().AddForce (-force * magnitude);
            Debug.Log("Forced Back!");
        }
    }
}
