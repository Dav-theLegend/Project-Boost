using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 150f;
    [SerializeField] float rcsThrust_Main = 1000f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    Rigidbody rigidBody;
    AudioSource audiosource;
    ParticleSystem particles;

    enum State {Alive, Dying, Transcending};
    State state = State.Alive;

    [SerializeField] bool toggleCollision = true;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (state == State.Alive)
        {
           
            RespondtoThrustInput();
            RespondtoRotateInput();
            //This is for the users input
        }
        if (Debug.isDebugBuild)
        {
            RespondtoDebugKeys();
        }
    }

    private void RespondtoDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            int finalSceneIndex = SceneManager.sceneCountInBuildSettings;
            if (finalSceneIndex == nextSceneIndex)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            
        }
        else if (Input.GetKey(KeyCode.C))
        {
            toggleCollision = !toggleCollision;  
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || toggleCollision == false)
        {
            return; // ignore colisiions
        }

        if (toggleCollision == true)
        {
            print("Collision Enabled");
            ToggleCollision(collision);
        }
      
    }

    private void ToggleCollision(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Ök"); //do nothing
                break;
            case "Finish":
                state = State.Transcending;
                audiosource.PlayOneShot(success);
                successParticles.Play();
                Invoke("LoadNextScene", 1.7f);
                print("Finshed Level 1");
                break;
            case "Finish (1)":
                state = State.Transcending;
                audiosource.PlayOneShot(success);
                successParticles.Play();
                Invoke("LoadNextScene1", 1.7f);
                print("Finished Level 2");
                break;
            default:
                state = State.Dying;
                if (state == State.Dying)
                {
                    deathParticles.Play();
                    audiosource.PlayOneShot(death);
                    Invoke("LoadInitialScene", 1.2f);
                    print("Dead");
                }
                break;

        }
    }

    private void LoadInitialScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadNextScene1()
    {
        SceneManager.LoadScene(2);
    }

    private void RespondtoThrustInput()
    {
        

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audiosource.Stop();
            mainEngineParticles.Stop();
        }
        
       

    }

    private void ApplyThrust()
    {
        float mainThrustThisFrame = rcsThrust_Main * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * mainThrustThisFrame);
        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
        if (!audiosource.isPlaying)
        {
            audiosource.PlayOneShot(mainEngine);
        }
    }

    private void RespondtoRotateInput()
    {
       
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.right * rotationThisFrame);
            rigidBody.freezeRotation = true; //take manual of control of rotation
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.right * rotationThisFrame);
            rigidBody.freezeRotation = true; //take manual of control of rotation
        }
            rigidBody.freezeRotation = false; //resume physics control of rotation
        
    }


}
    

