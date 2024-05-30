using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behavior of a door in the game.
/// </summary>
public class DoorController : MonoBehaviour
{ 
    public Animator animator;

    public string keyRequired = string.Empty;
    public GameObject doorBlock;
    public AudioClip creak;
    public AudioClip creak2;
    public AudioClip doorShut;
    
    private AudioSource audioSource;
    private BoxCollider doorBlockCollider;

    private void Awake()
    {
        doorBlockCollider = doorBlock.GetComponent<BoxCollider>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Called when a collider enters the trigger zone of the door.
    /// </summary>
    /// <param name="other">The collider that entered the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bool canOpen = false;
            if(keyRequired == string.Empty)
            {
                canOpen = true;
            }
            else
            {
                canOpen = GameController.Instance.keysHeld.Contains(keyRequired);
            }
            if (canOpen)
            {
                animator.SetTrigger("open");
            }
        }
    }

    /// <summary>
    /// Called when a collider exits the trigger zone of the door.
    /// </summary>
    /// <param name="other">The collider that exited the trigger zone.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("close");
        }
    }

    /// <summary>
    /// Plays the sound of the door shutting.
    /// </summary>
    public void PlayDoorShut()
    {
        audioSource.clip = doorShut;
        audioSource.Play();
    }

    /// <summary>
    /// Plays the sound of the door opening.
    /// </summary>
    public void PlayDoorOpen()
    {
        audioSource.clip = creak;
        audioSource.Play();
    }

    /// <summary>
    /// Removes the door block collider, allowing the player to pass through the door.
    /// </summary>
    private void RemoveDoorBlock()
    {
        doorBlockCollider.enabled = false;
    }

    /// <summary>
    /// Adds the door block collider, blocking the player from passing through the door.
    /// </summary>
    private void AddDoorBlock()
    {
        doorBlockCollider.enabled = true;
    }
}
