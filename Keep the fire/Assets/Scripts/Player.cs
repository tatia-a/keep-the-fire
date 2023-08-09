using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform[] tracks;
    private int activeTrack = 1;

    [SerializeField] private GameObject playerBody;
    [SerializeField] private float speed = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            activeTrack += 1;
            if (activeTrack > tracks.Length - 1) activeTrack = tracks.Length - 1;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            activeTrack -= 1;
            if (activeTrack < 0) activeTrack = 0;
        }

        var newPosition = new Vector3(tracks[activeTrack].position.x, playerBody.transform.position.y, playerBody.transform.position.z);

        playerBody.transform.position = Vector3.MoveTowards(playerBody.transform.position, newPosition, speed * Time.deltaTime);
    }
}
