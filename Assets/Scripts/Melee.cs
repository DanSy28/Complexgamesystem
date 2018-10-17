using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public int damage = 50;
    public float hitDuration = 1f;
    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        
        yield return new WaitForSeconds(hitDuration);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        
       
    }

}
