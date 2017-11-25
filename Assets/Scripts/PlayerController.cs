using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementControlType
{
    Letters,
    Arrows
}

public enum DropBombControlType
{
    SpaceBar,
    ReturnKey
}

public enum PlayerTags
{
    Player1,
    Player2
}

public class PlayerController : MonoBehaviour
{
    public string playerName = "No name";
	public float walkSpeed = 10.0f;
	public MovementControlType movementControlType = MovementControlType.Letters;
	public DropBombControlType dropBombControlType = DropBombControlType.SpaceBar;
    public GameObject bombClass;
    public GameObject deathEffect;
    new public ParticleSystem particleSystem;

    private Vector3 veloc;
    private ParticleSystem myParticles;

    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public int extraBombRange = 0;

    private void Start()
    {
        veloc = new Vector3();

        myParticles = Instantiate(particleSystem, gameObject.transform.position, gameObject.transform.rotation, this.gameObject.transform);
    }

    // Update is called once per frame
    void Update ()
	{
		if (
			(Input.GetKeyDown(KeyCode.Space) && dropBombControlType == DropBombControlType.SpaceBar) ||
			(Input.GetKeyDown(KeyCode.Return) && dropBombControlType == DropBombControlType.ReturnKey)
		   )
			DropBomb ();

        //myParticles.transform.position = gameObject.transform.position;
	}

	void FixedUpdate()
	{
		float horiz = 0.0f;
		float vert = 0.0f;

		if (movementControlType == MovementControlType.Letters)
		{
			horiz = Input.GetAxis ("HorizAxis Letter") * Time.fixedDeltaTime * walkSpeed;
			vert = Input.GetAxis ("VertAxis Letter") * Time.fixedDeltaTime * walkSpeed;
		}
		else
		{
			horiz = Input.GetAxis ("HorizAxis Arrow") * Time.fixedDeltaTime * walkSpeed;
			vert = Input.GetAxis ("VertAxis Arrow") * Time.fixedDeltaTime * walkSpeed;
		}

        veloc.x = -1f * vert;
        veloc.z = horiz;

        GetComponent<Rigidbody>().velocity = veloc * 20f;
        //transform.Translate(-1 * vert, 0, horiz);
	}

    void OnDestroy()
    {
        GameObject boomEffect = Instantiate(deathEffect, gameObject.transform.position, gameObject.transform.rotation);

        Destroy(boomEffect.gameObject, 1.0f);
    }

	void DropBomb()
	{
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        Vector3 position = new Vector3(
            Mathf.RoundToInt(gameObject.transform.position.x),
            capsuleCollider.height / 2,
            Mathf.RoundToInt(gameObject.transform.position.z)
            );


        GameObject newBomb = Instantiate(bombClass, position , Quaternion.identity);
        newBomb.GetComponent<BombController>().explosionRadius += extraBombRange;
	}

    public void SetLightAfterMaterial()
    {
        Color materialColor = GetComponent<Renderer>().material.color;
        Vector4 colorVector = new Vector4(materialColor.r, materialColor.g, materialColor.b, materialColor.a);

        colorVector.Normalize();

        GetComponent<Light>().color = new Color(colorVector.x, colorVector.y, colorVector.z, colorVector.w);
    }
}