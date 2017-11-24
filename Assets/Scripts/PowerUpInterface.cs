using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpInterface : MonoBehaviour
{
    public float timeBeforeVanish = 3.0f;

    new private Light light;

    void OnTriggerEnter(Collider other)
    {
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();

        if (pc)
            OnPickUp(pc);
    }

    protected virtual void OnPickUp(PlayerController playerController)
    {
        Destroy(gameObject, 0.01f);
    }

    void Start()
    {
        light = GetComponent<Light>();
    }

    void Update()
    {
        if (timeBeforeVanish > 0.0f)
            timeBeforeVanish -= Time.deltaTime;
        else
            Destroy(gameObject);

        light.color = new Color(Random.value, Random.value, Random.value, Random.value);
    }
}