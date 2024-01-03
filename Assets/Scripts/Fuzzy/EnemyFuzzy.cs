using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFuzzy : MonoBehaviour
{
    public FuzzyLogicExample fuzzyLogicController;
    public Transform player;

    private void Start()
    {
        fuzzyLogicController = GetComponent<FuzzyLogicExample>();
    }

    void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, player.position);
        Emotion currentEmotion = fuzzyLogicController.CalculateEmotion(playerDistance);

        // Faça algo com base na emoção atual (por exemplo, ajustar o comportamento do inimigo)
        Debug.Log(playerDistance);
        HandleEmotion(currentEmotion);
    }

    private void HandleEmotion(Emotion emotion)
    {
        // Implemente o comportamento do inimigo com base na emoção atual
        // ...
        Debug.Log(emotion.ToString());
    }
}