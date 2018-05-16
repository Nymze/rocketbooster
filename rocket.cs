
       using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {


    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float boost = 100f;
    [SerializeField] float levelLoadDelay = 1f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip win;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem winParticles;

    Rigidbody rigidBody;
    AudioSource rocketSound;
    enum State {Alive, Dying, Transcending}
    State state = State.Alive;

    bool CollisionsDisabled = false;

   

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();
        
    }
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive) { 
        RespondToThrustInput();
        RespondToRotateInput();
        }

        if (Debug.isDebugBuild){

            respondToDebugKey();

        }

       


        }

    private void respondToDebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartSuccessSequence();

        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            // disable collisions
            CollisionsDisabled = !CollisionsDisabled;
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (state != State.Alive || CollisionsDisabled) {return;}

        switch (collision.gameObject.tag)
        {
            case "friendly":
                break;
            case "Finish":
                StartSuccessSequence();

                break;

            default:
                StartDeathSequence();

                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        rocketSound.Stop();
        rocketSound.PlayOneShot(explosion);
        explosionParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        rocketSound.Stop();
        rocketSound.PlayOneShot(win);
        winParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);

        
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {

    

        if (Input.GetKey(KeyCode.Space)) // if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();

        }
        else
        {
            rocketSound.Stop();
            mainEngineParticles.Stop();
        }
    }
    // private
    public void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * boost);
        if (!rocketSound.isPlaying)
        {

            rocketSound.PlayOneShot(mainEngine);

        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of the rotation

        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation

    }

    



}

