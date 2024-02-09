using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FuzzyController : MonoBehaviour
{
    public string emotion;
    public SpriteRenderer emotionFeedback;
    private bool _stop;

    // Linguistic Variables
    private float _distanceToPlayer;
    private float _playerHealth;
    private float _enemyHealth;
    private float _enemyCount;

    // Distance intervals
    private readonly float _nearDistance = 5f;
    private readonly float _mediumDistance = 10f;
    private readonly float _farDistance = 15f;

    // Health thresholds
    private float _lowHealthThreshold = 2f;
    private float _mediumHealthThreshold = 5f;
    private float _highHealthThreshold = 10f;

    // Enemy count thresholds
    private float _lowEnemyCountThreshold = 3f;
    private float _mediumEnemyCountThreshold = 6f;
    private float _highEnemyCountThreshold = 9f;

    private float TriangularMembership(float x, float a, float b, float c)
    {
        return Mathf.Max(0, Mathf.Min(Mathf.Min((x - a) / (b - a), (c - x) / (c - b)), 1));
    }

    private float CalculateCalmMembership()
    {
        var distance = TriangularMembership(_distanceToPlayer, _mediumDistance, _farDistance, float.MaxValue);

        // var playerH = TriangularMembership(playerHealth, mediumHealthThreshold, highHealthThreshold, highHealthThreshold);
        var playerH = TriangularMembership(_playerHealth, 3f, 5f, 7f);

        // var enemyH = TriangularMembership(enemyHealth, mediumHealthThreshold, highHealthThreshold, float.MaxValue);
        var enemyH = TriangularMembership(_enemyHealth, 4f, 5f, float.MaxValue);

        // var enemyC = TriangularMembership(enemyCount, mediumHealthThreshold, highHealthThreshold, float.MaxValue);
        var enemyC = TriangularMembership(_enemyCount, 2f, 3f, 4f);

        return (distance + playerH + enemyH + enemyC) / 4;
    }

    private float CalculateFearMembership()
    {
        var distance = TriangularMembership(_distanceToPlayer, 0f, _nearDistance, _nearDistance + 2);

        // var playerH = TriangularMembership(playerHealth, 0f, mediumHealthThreshold, highHealthThreshold);
        var playerH = TriangularMembership(_playerHealth, 4f, 6f, float.MaxValue);

        // var enemyH = TriangularMembership(enemyHealth, 0f, lowHealthThreshold, mediumHealthThreshold);
        var enemyH = TriangularMembership(_enemyHealth, 0f, 2f, 3f);

        // var enemyC = TriangularMembership(enemyCount, 0f, lowEnemyCountThreshold, mediumEnemyCountThreshold);
        var enemyC = TriangularMembership(_enemyCount, 0f, 1f, 2f);

        return (distance + playerH + enemyH + enemyC) / 4;
    }

    private float CalculateBraveMembership()
    {
        var distance = TriangularMembership(_distanceToPlayer, _nearDistance, _mediumDistance, _mediumDistance + 2);

        // var playerH = TriangularMembership(playerHealth, 0f, lowHealthThreshold, lowHealthThreshold + 2);
        var playerH = TriangularMembership(_playerHealth, 0f, 3f, 5f);

        // var enemyH = TriangularMembership(enemyHealth, mediumHealthThreshold, mediumHealthThreshold + 2, highHealthThreshold);
        var enemyH = TriangularMembership(_enemyHealth, 2f, 3f, 4f);

        // var enemyC = TriangularMembership(enemyCount, mediumEnemyCountThreshold, highEnemyCountThreshold, float.MaxValue);
        var enemyC = TriangularMembership(_enemyCount, 3f, 4f, float.MaxValue);

        return (distance + playerH + enemyH + enemyC) / 4;
    }

    private float CalculateAngryMembership()
    {
        var distance = TriangularMembership(_distanceToPlayer, _nearDistance, _mediumDistance, _mediumDistance + 2);

        // var playerH = TriangularMembership(playerHealth, 0f, lowHealthThreshold, mediumHealthThreshold);
        var playerH = TriangularMembership(_playerHealth, 0f, 2f, 4f);

        // var enemyH = TriangularMembership(enemyHealth, 0f, lowHealthThreshold, mediumHealthThreshold);
        var enemyH = TriangularMembership(_enemyHealth, 3f, 4f, 5f);

        // var enemyC = TriangularMembership(enemyCount, mediumEnemyCountThreshold, highEnemyCountThreshold, float.MaxValue);
        var enemyC = TriangularMembership(_enemyCount, 1f, 2f, 3f);

        return (distance + playerH + enemyH + enemyC) / 4;
    }

    private string FuzzyEmotionCalculation()
    {
        float calmMembership = CalculateCalmMembership();
        float fearMembership = CalculateFearMembership();
        float braveMembership = CalculateBraveMembership();
        float angryMembership = CalculateAngryMembership();

        float totalMembership = calmMembership + fearMembership + braveMembership + angryMembership;

        if (totalMembership > 0)
        {
            float calmEmotion = calmMembership / totalMembership;
            float fearEmotion = fearMembership / totalMembership;
            float braveEmotion = braveMembership / totalMembership;
            float angryEmotion = angryMembership / totalMembership;

            string emotionalState = DetermineEmotionalState(calmEmotion, fearEmotion, braveEmotion, angryEmotion);

            // Debug.Log($"Calm: {calmEmotion}, Fear: {fearEmotion}, Brave: {braveEmotion}, Angry: {angryEmotion}");

            return emotionalState;
        }

        return "Calm";
    }

    private string DetermineEmotionalState(float calm, float fear, float brave, float angry)
    {
        float maxEmotion = Mathf.Max(calm, fear, brave, angry);

        if (maxEmotion == angry)
            return "Angry";
        else if (maxEmotion == fear)
            return "Fear";
        else if (maxEmotion == brave)
            return "Brave";
        else
            return "Calm";
    }

    private void UpdateDistanceToPlayer()
    {
        _distanceToPlayer =
            Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position);
    }

    private void UpdatePlayerHealth()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>().currentHealth;
    }

    private void UpdateEnemyHealth()
    {
        _enemyHealth = GetComponent<EnemyHealth>().currentHealth;
    }

    private void UpdateEnemyCount()
    {
        _enemyCount = FindObjectsOfType<EnemyHealth>().Length;
    }

    private void Start()
    {
        emotionFeedback.color = Color.green;
    }

    private void Update()
    {
        if (_stop) return;

        UpdateDistanceToPlayer();
        UpdatePlayerHealth();
        UpdateEnemyHealth();
        UpdateEnemyCount();

        // Debug.Log($"Distance: {_distanceToPlayer}, PlayerHealth: {_playerHealth}, EnemyHealth: {_enemyHealth}, Enemies: {_enemyCount}");

        emotion = FuzzyEmotionCalculation();

        switch (emotion)
        {
            case "Calm":
                emotionFeedback.color = Color.green;
                break;
            case "Fear":
                emotionFeedback.color = Color.black;
                break;
            case "Brave":
                emotionFeedback.color = Color.blue;
                break;
            case "Angry":
                emotionFeedback.color = Color.red;
                break;
        }
    }

    public IEnumerator StopEmotion()
    {
        _stop = true;
        yield return new WaitForSeconds(5f);
        _stop = false;
    }
}