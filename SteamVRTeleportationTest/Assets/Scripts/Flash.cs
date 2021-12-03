using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    public bool toggle;

    private Light myLight;
    private bool done = true;

    // Start is called before the first frame update
    void Start()
    {
        myLight = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (done)
        {
            done = false;
            StartCoroutine(Blink());
        }
    }

    private IEnumerator Blink()
    {
        if(toggle)
        {
            myLight.intensity = 5;
        }
        else
        {
            myLight.intensity = 0;
        }
        yield return new WaitForSeconds(0.5f);
        if (toggle)
        {
            myLight.intensity = 0;
        }
        else
        {
            myLight.intensity = 5;
        }
        yield return new WaitForSeconds(0.5f);
        done = true;
    }
}