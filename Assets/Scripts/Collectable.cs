using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    GameObject healthPickUpPrefab;

    [SerializeField]
    int healthAmount; 

    public void PickUp() 
    {
        GameManager.instance.IncreaseHealth(healthAmount);
        Destroy(gameObject);
    }
}
