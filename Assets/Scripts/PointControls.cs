using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointControls : MonoBehaviour
{
    private void Start()
    {
        //Debug.Log("Size of point: ");
        //Debug.Log(gameObject.GetComponent<Renderer>().bounds.size);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = GameObject.Find("Player");
        PlayerControls playerControls = go.GetComponent<PlayerControls>();

        if (other.CompareTag("Player")) 
        {
            playerControls.playerPoints++;
            playerControls.playerPointsText.text = playerControls.playerPoints.ToString();
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
