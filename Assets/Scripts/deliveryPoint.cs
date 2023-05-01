using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deliveryPoint : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Player"){
            GameManager.instance.RemoveBoxFromInventory();
        }
    }
}
