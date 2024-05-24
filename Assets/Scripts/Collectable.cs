using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    GameObject healthPickUpPrefab;

    public void PickUp() 
    {
        GameManager.instance.IncreaseHealth(1);
        Destroy(gameObject);
    }
}
