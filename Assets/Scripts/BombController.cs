using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float explosionDelay = 3.0f;
    public int explosionRadius = 2;     // Measured in blocks of 1x1x1 Unity units.
    public GameObject explosionClass;
    public AudioSource explosionSound;
    public LayerMask layerMask;

    private bool hasExploded;
    private int explosionDirectionsCompleted;
    private GameObject gameRoundManager;

    // Use this for initialization
    void Start()
    {
        hasExploded = false;

        gameRoundManager = GameObject.FindWithTag("GameRoundManager");

        Invoke("Explode", explosionDelay);
    }
    
    public void Explode()
    {
        if (! hasExploded)
        {
            hasExploded = true;
            
            Destroy(GetComponent<MeshRenderer>(), 0.2f);
            Destroy(GetComponent<Collider>(), 0.2f);
            Destroy(GetComponent<Light>());

            Instantiate(explosionSound, gameObject.transform, true);

            // Loop in every direction to see how many explosion blocks we can spawn.

            // First explode in this bomb's position.
            StartCoroutine(SpawnExplosion(Vector3.zero, 1));

            //for (int i = 1; i <= explosionRadius; i ++)
            {
                // Check forward.
                StartCoroutine(SpawnExplosion(new Vector3(0, 0, 1), explosionRadius));

                // Check right.
                StartCoroutine(SpawnExplosion(new Vector3(1, 0, 0), explosionRadius));

                // Check backward.
                StartCoroutine(SpawnExplosion(new Vector3(0, 0, -1), explosionRadius));

                // Check left.
                StartCoroutine(SpawnExplosion(new Vector3(-1, 0, 0), explosionRadius));
            }
        }
    }

    IEnumerator SpawnExplosion(Vector3 direction, int radius)
    {
        for (int i = 1; i <= radius; i ++)
        {
            RaycastHit hitInfo;

            Physics.Raycast(gameObject.transform.position, direction * i, out hitInfo, i, layerMask);

            if (! hitInfo.collider)
            {
                GameObject boom = Instantiate(explosionClass, gameObject.transform.position + direction * i, Quaternion.identity);
                boom.gameObject.GetComponent<ExplosionController>().SetGameRoundManager(gameRoundManager);
            }
            else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("BreakableWall"))
            {
                GameObject boom = Instantiate(explosionClass, gameObject.transform.position + direction * i, Quaternion.identity);
                boom.gameObject.GetComponent<ExplosionController>().SetGameRoundManager(gameRoundManager);

                break;  // break a breakable object, then stop further explosions.
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(0.05f);
        }

        explosionDirectionsCompleted += 1;

        // If all 4 directions + center have been completed, destroy the bomb.
        if (explosionDirectionsCompleted >= 5)
        {
            Destroy(gameObject, explosionSound.clip.length);
        }
    }
}