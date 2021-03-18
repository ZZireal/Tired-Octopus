using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControls : MonoBehaviour
{

    private void Start()
    {
        //Debug.Log("Size of lightning: ");
        //Debug.Log(gameObject.GetComponent<Renderer>().bounds.size);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = GameObject.Find("Player");
        PlayerControls playerControls = go.GetComponent<PlayerControls>();

        if (other.CompareTag("Player"))
        {
            Debug.Log(playerControls.rigidBody.velocity.z);
            playerControls.rigidBody.AddForce(Vector3.forward * playerControls.frontForce * 30 * Time.fixedDeltaTime);
            Debug.Log(playerControls.rigidBody.velocity.z);
            Destroy(gameObject);
        }
    }
}
