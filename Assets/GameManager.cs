using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine.UI;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    public int roks = 10;
    public float respawnTime;
    public int scoreIncrement = 100;

    private int score;
    private Text scoreText;
    private Unity.Mathematics.Random random;

    EntityManager manager;

    private void Awake()
    {
        if (main != null && main != this)
        {
            Destroy(gameObject);
            return;
        }

        main = this;

        score = 0;
        random = new Unity.Mathematics.Random(10);
        scoreText = GameObject.Find("Score").GetComponent<Text>();

        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
    }

    public void PlayerScore()
    {
        score += scoreIncrement;
        scoreText.text = score.ToString();
    }

}
