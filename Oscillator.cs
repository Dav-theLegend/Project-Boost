using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [Range(0.1f, 20f)] [SerializeField] float period = 2.5f; // time taken for an oscillation

    // todo remove from inspector later
    [Range(0, 1)] [SerializeField] float movementFactor;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = 0;
        if (period != 0)
        {
            cycles = Time.time / period;
        }
        
        const float tau = Mathf.PI * 2; // full circle AKA an oscillation is 6.28
        float rawSinWave = Mathf.Sin(cycles * tau); // cycles * tau defines no of oscillations done
        // Mathf.Sin spits out a value -1 to 1

        movementFactor = (rawSinWave / 2f) + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
