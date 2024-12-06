using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")] // Label for Clarity
    [SerializeField] AudioSource musicSource;
    
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Source ----------")] // Label for Clarity

    public AudioClip Menu;
    public AudioClip Buttons;

    public AudioClip PopUp;

    public AudioClip Shoot;



    private void Start() { //We want the Background Music to immediately play when the scene loads in
        musicSource.clip = Menu;
        musicSource.loop = true; // Make the BG Music Loop
        musicSource.Play(); // Play the BG Music
    }


    public void SFXPlay(AudioClip clip) { //Functionality for SFX

        SFXSource.PlayOneShot(clip);

    }

    public bool isSFXPlay(AudioClip audio) {

        return SFXSource.isPlaying && SFXSource.clip == audio;

    }



}