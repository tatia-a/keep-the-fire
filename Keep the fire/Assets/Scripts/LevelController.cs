using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private Camera mainCamera;

    [Header("Level Settings")]
    [SerializeField] private Transform startPoint; //точка, в которой начинается путь
    [SerializeField] private Platform tilePrefab;
    [SerializeField] private float movingSpeed = 12; // скорость уровня
    [SerializeField] private int tilesToPreSpawn = 15; //как много тайлов справнится за раз
    [SerializeField] private int tilesWithoutObstacles = 3; //как много пропустить в начале (я не использую в данном случае)
    [SerializeField] private static int maxFireAmount = 6;

    List<Platform> spawnedTiles = new List<Platform>();

    // game parametrs
    public bool GameOver { get; private set; } = false;
    public static bool GameStarted { get; private set; } = false;
    public float Score { get; private set; } = 0;
    public int FireAmount { get; private set; } = maxFireAmount;

    public static LevelController instance;

    // for fire decrease
    [SerializeField] private float fireDecreaseInterval = 1f;
    private float timer;

    void Start()
    {
        instance = this;

        GameOver = GameStarted = false;
        Score = 0;
        FireAmount = maxFireAmount;

        timer = fireDecreaseInterval;

        mainCamera = Camera.main;
        GenerateGround();
    }


    void Update()
    {
        if (!GameOver && GameStarted)
        {
            MoveTiles();
            DeleteInvisibleTile();

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                instance.RemoveFire();
                timer = fireDecreaseInterval;
            }
        }

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (FireAmount <= 0)
        {
            GameOver = true;
        }
        if (GameOver)
        {
            print("GameOver");
        }
    }

    public void StartLevel()
    {
        GameStarted = true;
    }
    public void AddFire()
    {
        if (FireAmount < maxFireAmount) 
            FireAmount++;
        else 
            FireAmount = maxFireAmount;
        UI.instance.UpdateFireAmountUI();
    }
    public void RemoveFire()
    {
        if (FireAmount > 0)
            FireAmount--;
        else
            FireAmount = 0;
        UI.instance.UpdateFireAmountUI();
    }
    private void DeleteInvisibleTile()
    {
        if (mainCamera.WorldToViewportPoint(spawnedTiles[0].endPoint.position).z < 0)
        {
            //Move the tile to the front if it's behind the Camera
            Platform tileTmp = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            tileTmp.transform.position = spawnedTiles[spawnedTiles.Count - 1].endPoint.position - tileTmp.startPoint.localPosition;
            tileTmp.ActivateRandomObstacle();
            spawnedTiles.Add(tileTmp);
        }
    }
    private void MoveTiles()
    {
        
         transform.Translate(-spawnedTiles[0].transform.forward * Time.deltaTime * (movingSpeed + (Score / 500)), Space.World);
         Score += Time.deltaTime * movingSpeed;
        
    }
    private void GenerateGround()
    {
        Vector3 spawnPosition = startPoint.position;
        int tilesWithNoObstaclesTmp = tilesWithoutObstacles;
        for (int i = 0; i < tilesToPreSpawn; i++)
        {
            spawnPosition -= tilePrefab.startPoint.localPosition;
            Platform spawnedTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity) as Platform;

            //пропуск тайлов (для старта)
            if (tilesWithNoObstaclesTmp > 0)
            {
                spawnedTile.DeactivateAllObstacles();
                tilesWithNoObstaclesTmp--;
            }
            else
            {
                spawnedTile.ActivateRandomObstacle();
            }

            spawnPosition = spawnedTile.endPoint.position;
            spawnedTile.transform.SetParent(transform);
            spawnedTiles.Add(spawnedTile);
        }
    }
}
