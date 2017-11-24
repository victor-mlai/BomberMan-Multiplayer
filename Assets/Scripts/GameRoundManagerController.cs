using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoundManagerController : MonoBehaviour
{
    public float gameRoundTimer = 60.0f;
    public float playerSpawnDelay = 3.0f;
    public GameObject playerClass;

    private PlayerTags player1Tag = PlayerTags.Player1;
    public string player1Name;
    public GameObject player1Spawn;
    public Material player1Material;
    public MovementControlType player1Movement = MovementControlType.Letters;
    public DropBombControlType player1DropType = DropBombControlType.SpaceBar;
    public GameObject player1Bomb;
    public ParticleSystem player1Particles;

    private PlayerTags player2Tag = PlayerTags.Player2;
    public string player2Name;
    public GameObject player2Spawn;
    public Material player2Material;
    public MovementControlType player2Movement = MovementControlType.Arrows;
    public DropBombControlType player2DropType = DropBombControlType.ReturnKey;
    public GameObject player2Bomb;
    public ParticleSystem player2Particles;

    private int player1Score = 0;
    private int player2Score = 0;

    private GameObject player1, player2;
    private float timer;
    private bool isGameOver;

    private GameUIBehaviour gameUIBehaviour;
    
    void Awake()
    {
        MainMenuUIBehaviour mainMenuUIBehaviour = FindObjectOfType<MainMenuUIBehaviour>();

        if (mainMenuUIBehaviour.player1Name.Length > 0)
        {
            player1Name = mainMenuUIBehaviour.player1Name;            
        }

        if (mainMenuUIBehaviour.player2Name.Length > 0)
        {
            player2Name = mainMenuUIBehaviour.player2Name;
        }
    }

    // Use this for initialization
    void Start()
    {
        InitGameElementsAndUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (! isGameOver)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;

                gameUIBehaviour.UpdateUITimer((int) timer);
            }
            else
            {
                AnnounceGameOver();
            }
        }
    }

    public void InitGameElementsAndUI()
    {
        CleanUpArena();

        player1 = SpawnPlayer(player1Name, player1Tag, player1Spawn, player1Material, player1Movement, player1DropType, player1Bomb, player1Particles);
        player2 = SpawnPlayer(player2Name, player2Tag, player2Spawn, player2Material, player2Movement, player2DropType, player2Bomb, player2Particles);

        player1Score = player2Score = 0;

        timer = gameRoundTimer;

        gameUIBehaviour = GetComponent<GameUIBehaviour>();

        gameUIBehaviour.gameOverPanel.SetActive(false);
        gameUIBehaviour.UpdateUITimer((int)timer);
        gameUIBehaviour.UpdateUIPlayer1Score(player1Name, 0);
        gameUIBehaviour.UpdateUIPlayer2Score(player2Name, 0);

        isGameOver = false;
    }

    private GameObject SpawnPlayer(string playerName, PlayerTags playerTag, GameObject playerSpawn, Material playerMaterial, MovementControlType playerMovement, DropBombControlType playerDropType, GameObject playerBomb, ParticleSystem playerParticles)
    {
        GameObject player = Instantiate(playerClass, playerSpawn.gameObject.transform.position, playerSpawn.gameObject.transform.rotation);

        player.gameObject.GetComponent<PlayerController>().playerName = playerName;
        player.gameObject.tag = playerTag.ToString();
        player.gameObject.GetComponent<PlayerController>().movementControlType = playerMovement;
        player.gameObject.GetComponent<PlayerController>().dropBombControlType = playerDropType;
        player.gameObject.GetComponent<PlayerController>().bombClass = playerBomb;
        player.gameObject.GetComponent<Renderer>().material.color = playerMaterial.color;
        player.gameObject.GetComponent<PlayerController>().particleSystem = playerParticles;

        player.gameObject.GetComponent<PlayerController>().SetLightAfterMaterial();

        return player;
    }

    public void AnnouncePlayerDeath(GameObject player)
    {
        if (player.tag == PlayerTags.Player1.ToString())
        {
            player2Score += 1;
            gameUIBehaviour.UpdateUIPlayer2Score(player2Name, player2Score);

            Invoke("SpawnPlayer1", playerSpawnDelay);
        }
        else
        {
            player1Score += 1;
            gameUIBehaviour.UpdateUIPlayer1Score(player1Name, player1Score);

            Invoke("SpawnPlayer2", playerSpawnDelay);
        }

        StartCoroutine(SlowMotionEffect());
    }

    private void AnnounceGameOver()
    {
        isGameOver = true;

        // Clear the arena.
        if (player1 || player2)
            Invoke("CleanUpArena", 2.0f);

        if (player1Score > player2Score)
            gameUIBehaviour.UpdateUIGameOver(player1Name);
        else if (player2Score > player1Score)
            gameUIBehaviour.UpdateUIGameOver(player2Name);
        else
            gameUIBehaviour.UpdateUIGameOverDraw();
    }

    private void CleanUpArena()
    {
        CancelInvoke("CleanUpArena");

        if (player1)
        {
            Destroy(player1.gameObject);
            player1 = null;
        }

        if (player2)
        {
            Destroy(player2.gameObject);
            player2 = null;
        }
    }

    private void SpawnPlayer1()
    {
        if (! player1 && timer > playerSpawnDelay)
            player1 = SpawnPlayer(player1Name, player1Tag, player1Spawn, player1Material, player1Movement, player1DropType, player1Bomb, player1Particles);
    }

    private void SpawnPlayer2()
    {
        if (! player2 && timer > playerSpawnDelay)
            player2 = SpawnPlayer(player2Name, player2Tag, player2Spawn, player2Material, player2Movement, player2DropType, player2Bomb, player2Particles);
    }

    public IEnumerator SlowMotionEffect()
    {
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        while (Time.timeScale < 1.0f)
        {
            Time.timeScale += Time.deltaTime;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return null;
        }
    }
}
