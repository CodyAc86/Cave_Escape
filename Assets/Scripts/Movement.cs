using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip thrusterSound;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    [SerializeField] float fuelMainDepleteRate = 0.8f;
    [SerializeField] float fuelRotationDepeleRate = 0.1f;
    [SerializeField] float refuelPoints = 25f;
    public float maxFuel = 100f;
    public static float currentFuel;
    public Fuel fuelBar;

    Rigidbody rigidBody;
    AudioSource audioSource;

    

    void Start()
    {
        currentFuel = maxFuel;
        fuelBar.SetFuel(maxFuel);
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    
    void FixedUpdate()
    {
        Thrust();
        Rotate();
        CheckFuel();
    }

    void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }
    
    

    void Rotate()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation

        float rotationThisFrame = rotationThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            ThrusterEngineSound();
            RotateLeft(rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ThrusterEngineSound();
            RotateRight(rotationThisFrame);
        }
        else
        {

            StopThrusterParticles();
        }
        rigidBody.freezeRotation = false;
    }
    


    void StartThrusting()
    {
        ConsumeFuel(fuelMainDepleteRate);
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        MainEngineSound();
        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }

    

    void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    


    void RotateLeft(float rotationThisFrame)
    {
        ConsumeFuel(fuelRotationDepeleRate);
        transform.Rotate(Vector3.forward * rotationThisFrame);
        if (!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
        }
    }
    void RotateRight(float rotationThisFrame)
    {
        ConsumeFuel(fuelRotationDepeleRate);
        transform.Rotate(-Vector3.forward * rotationThisFrame);
        if (!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fuel")
        {
            Refuel(refuelPoints);
            other.gameObject.SetActive(false);
        }

    }
    void Refuel(float refuel)
    {
        currentFuel += refuel;
        fuelBar.SetFuel(currentFuel);
    }
    private void MainEngineSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }
    private void ThrusterEngineSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrusterSound);
        }
    }
    void StopThrusterParticles()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }
    void ConsumeFuel(float consumeRate )
    {
        currentFuel -= consumeRate;
        fuelBar.SetFuel(currentFuel);
    }
    void CheckFuel()
    {
        if (currentFuel <= 0)
        {
            StopEngines();
        }        
    }
    void StopEngines()
    {
        this.enabled = false;        
    }
}
