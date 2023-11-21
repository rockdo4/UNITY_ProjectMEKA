using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineBlink : MonoBehaviour
{
    public Material[] materials;
    public float delay = 1;
    public float randomDelay = 1;
    Renderer renderer;
    int counter;
    float switchTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (switchTime < Time.time)
        {
            counter++;
            renderer.material = materials[counter % materials.Length];
            switchTime = Time.time + delay + Random.value * randomDelay;
        }
    }
}
