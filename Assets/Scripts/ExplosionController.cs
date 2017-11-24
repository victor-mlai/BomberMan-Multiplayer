using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public GameObject explosionParticles;

    private GameObject gameRoundManager;

    void Start()
    {
        GameObject particlesObj = Instantiate(explosionParticles, gameObject.transform.position, gameObject.transform.rotation);
        ParticleSystem particlesObjPartSystem = particlesObj.GetComponent<ParticleSystem>();

        Destroy(GetComponent<Collider>().gameObject, 0.5f);
        Destroy(particlesObj.gameObject, particlesObjPartSystem.main.duration);
        Destroy(gameObject, particlesObjPartSystem.main.duration);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && ! other.GetComponent<PlayerController>().isDead)
        {
            other.GetComponent<PlayerController>().isDead = true;
            Destroy(other.gameObject);

            gameRoundManager.gameObject.GetComponent<GameRoundManagerController>().AnnouncePlayerDeath(other.gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("BreakableWall"))
        {
            other.gameObject.GetComponent<BreakableWallController>().Break();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.GetComponent<BombController>().Invoke("Explode", 0.05f);
        }
    }

    public void SetGameRoundManager(GameObject manager)
    {
        gameRoundManager = manager;
    }
}