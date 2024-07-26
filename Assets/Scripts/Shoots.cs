using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoots : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.DestruirAoSairDaTela(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        DestruirAoColidir(other.gameObject);
    }

    void DestruirAoColidir(GameObject other)
    {
        if (!other.CompareTag("Bullet")) Destroy(gameObject);
    }

}
