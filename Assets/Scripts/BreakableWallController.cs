using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWallController : MonoBehaviour
{
    public GameObject[] powerUps;

    public void Break()
    {
        int powerUpIndex = Random.Range(0, powerUps.Length);
        Debug.Log(powerUpIndex);
        Instantiate(powerUps[0], gameObject.transform.position, gameObject.transform.rotation);

        Destroy(gameObject);
    }
}