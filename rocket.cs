
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
