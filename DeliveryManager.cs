using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    // Dependancies
    public GameObject passengerSpawnPoint;
    public GameObject passengerDestination;
    public GameObject passengerPrefab;

    // Play space
    private float _heightBounds;
    private float _widthBounds;

    // Game logic
    private GameObject _passenger;
    private float _passengerPositionOffset = 1f;

    void Awake()
    {
        Camera camera = Camera.main;
        _heightBounds = camera.orthographicSize - 10f;
        _widthBounds = camera.orthographicSize * 1.15f;
        OnPassengerDroppedOff(false); // Initial Setup

        DeliveryPointController.OnPassengerPickUp += OnPassengerPickedUp;
        DeliveryPointController.OnPassengerDropOff += OnPassengerDroppedOff;
    }

    private void OnPassengerPickedUp(bool havePassenger)
    {
        passengerDestination.SetActive(havePassenger);
        passengerSpawnPoint.SetActive(!havePassenger);
        Destroy(_passenger);
    }

    private void OnPassengerDroppedOff(bool havePassenger)
    {
        passengerDestination.SetActive(havePassenger);
        passengerSpawnPoint.SetActive(!havePassenger);
        SetPositionsAndPassenger();
    }
 
    private void SetPositionsAndPassenger()
    {
        SetPositions();
        SpawnPassenger();
    }

    private void SetPositions()
    {
        passengerSpawnPoint.transform.position = new Vector3(Random.Range(-_widthBounds, _widthBounds), Random.Range(-_heightBounds, _heightBounds), 0f);
        SetDestinationPosition();
    }

    // Quadrant 1 is top left assuming play space has a center at (0, 0)
    // Rotate clockwise for the rest of the quadrants
    private void SetDestinationPosition()
    {
        Vector3 spawn = passengerSpawnPoint.transform.position;

        float x = Random.Range(-_widthBounds, _widthBounds);

        bool quadrant1 = spawn.x <= 0f && spawn.y > 0f;
        bool quadrant2 = spawn.x > 0f && spawn.y > 0f;
        bool quadrant3 = spawn.x > 0f && spawn.y <= 0f;
        bool quadrant4 = spawn.x <= 0f && spawn.y <= 0f;

        if (quadrant1)
        {
            if (x <= 0)
            {
                passengerDestination.transform.position = new Vector3(x, Random.Range(-_heightBounds, 0f), 0f);
            }

            else
            {
                passengerDestination.transform.position = new Vector3(x, Random.Range(-_heightBounds, _heightBounds), 0f);
            }
        }

        else if (quadrant2) 
        {
            if (x >= 0)
            {
                passengerDestination.transform.position = new Vector3(x, Random.Range(-_heightBounds, 0f), 0f);
            }
            else
            {
                passengerDestination.transform.position = new Vector3(x, Random.Range(-_heightBounds, _heightBounds), 0f);
            }
        }

        else if (quadrant3)
        {
            if (x >= 0)
            {
                passengerDestination.transform.position = new Vector3(x, Random.Range(0f, _heightBounds), 0f);
            }
            else
            {
                passengerDestination.transform.position = new Vector3(x, Random.Range(-_heightBounds, _heightBounds), 0f);
            }
        }

        else if (quadrant4)
        {
            if (x <= 0)
            {
                passengerDestination.transform.position = new Vector3(x, Random.Range(0f, _heightBounds), 0f);
            }
            else
            {
                passengerDestination.transform.position = new Vector3(x, Random.Range(-_heightBounds, _heightBounds), 0f);
            }
        }

    }

    private void SpawnPassenger()
    {
        _passenger = Instantiate<GameObject>(passengerPrefab);
        _passenger.transform.position = passengerSpawnPoint.transform.position + (transform.up * _passengerPositionOffset);
    }
    
}
