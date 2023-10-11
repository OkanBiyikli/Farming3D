using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField] private int boostAmount = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            BagController.instance.maxBagCapacity += boostAmount;
            Destroy(gameObject);
            BagController.instance.SaveBagCapacity();
        }
    }
}
