using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // tracks
    [SerializeField] private Transform[] tracks;
    private int activeTrack = 1;

    // player body
    [SerializeField] private GameObject playerBody;
    private Rigidbody playerRB;

    // player movements
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpHeight = 5f;
    private bool canJump = true;

    [SerializeField] private SwipeManager swipeManager;

    // animation
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject torchFire;

    // sound
    private AudioSource audio;
    [SerializeField] AudioClip addFireEffect;
    [SerializeField] AudioClip waterEffect;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool gameOver = LevelController.instance.GameOver;
        bool gameStarted = LevelController.GameStarted;
        if (!gameOver && gameStarted)
        {
            CheckUserInput();
            ApplyMovement();
            
        }
        CheckGameOver();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Obstacle")
        {
            //animator.SetTrigger("isDead");
            //GroundGenerator.instance.gameOver = true;
            LevelController.instance.RemoveFire();
            audio.PlayOneShot(waterEffect);
        }
        if(other.tag == "Fire")
        {
            LevelController.instance.AddFire();
            audio.PlayOneShot(addFireEffect);
            other.gameObject.SetActive(false);
        }
        
    }
    private void CheckGameOver()
    {
        if (LevelController.instance.GameOver)
        {
            animator.SetTrigger("isDead");
            torchFire.SetActive(false);
        }
    }
    private void ApplyMovement()
    {
        bool isPlayerOnTrack = tracks[activeTrack].position.x != playerBody.transform.position.x;
        if (isPlayerOnTrack)
        {
            var newPosition = new Vector3(tracks[activeTrack].position.x, playerBody.transform.position.y, playerBody.transform.position.z);
            playerBody.transform.position = Vector3.MoveTowards(playerBody.transform.position, newPosition, speed * Time.deltaTime);
        }
    }

    private void CheckUserInput()
    {
        // left
        if (Input.GetKeyDown(KeyCode.D) || swipeManager.SwipeRight)
        {
            activeTrack += 1;
            if (activeTrack > tracks.Length - 1) activeTrack = tracks.Length - 1;
        }
        // right
        if (Input.GetKeyDown(KeyCode.A) || swipeManager.SwipeLeft)
        {
            activeTrack -= 1;
            if (activeTrack < 0) activeTrack = 0;
        }
        // jump
        if (canJump && Input.GetKeyDown(KeyCode.W) || swipeManager.SwipeUp)
        {
            animator.SetTrigger("isJump");
            playerRB.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

}
