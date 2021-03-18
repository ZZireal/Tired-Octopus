using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopControls : MonoBehaviour
{
    private void Start()
    {
        //Debug.Log("Size of wall: ");
        //Debug.Log(gameObject.GetComponent<Renderer>().bounds.size);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = GameObject.Find("Player");
        PlayerControls playerControls = go.GetComponent<PlayerControls>();

        if (other.CompareTag("Player"))
        {
            Debug.Log(playerControls.rigidBody.velocity.z);
            playerControls.rigidBody.AddForce(Vector3.back * playerControls.rigidBody.velocity.z * 5000 * Time.fixedDeltaTime);
            Debug.Log(playerControls.rigidBody.velocity.z);
        }
    }
}
