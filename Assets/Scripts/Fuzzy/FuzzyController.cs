using UnityEngine;

public class FuzzyController : MonoBehaviour
{
    public string emotion;
    public SpriteRenderer emotionFeedback;

    // Linguistic Variables
    private float distanceToPlayer;
    private float playerHealth;
    private float enemyHealth;
    private float enemyCount;

    // Distance intervals
    private float nearDistance = 5f;
    private float mediumDistance = 10f;
    private float farDistance = 15f;

    // Health thresholds
    private float lowHealthThreshold = 2f;
    private float mediumHealthThreshold = 5f;
    private float highHealthThreshold = 10f;

    // Enemy count thresholds
    private float lowEnemyCountThreshold = 3f;
    private float mediumEnemyCountThreshold = 6f;
    private float highEnemyCountThreshold = 9f;

    private float TriangularMembership(float x, float a, float b, float c)
    {
        return Mathf.Max(0, Mathf.Min(Mathf.Min((x - a) / (b - a), (c - x) / (c - b)), 1));
    }

    private float CalculateCalmMembership()
    {
        var distance = TriangularMembership(distanceToPlayer, 0f, nearDistance, mediumDistance);

        // var playerH = TriangularMembership(playerHealth, mediumHealthThreshold, highHealthThreshold, highHealthThreshold);
        var playerH = TriangularMembership(playerHealth, 3f, 5f, 7f);

        // var enemyH = TriangularMembership(enemyHealth, mediumHealthThreshold, highHealthThreshold, float.MaxValue);
        var enemyH = TriangularMembership(enemyHealth, 4f, 5f, float.MaxValue);

        // var enemyC = TriangularMembership(enemyCount, mediumHealthThreshold, highHealthThreshold, float.MaxValue);
        var enemyC = TriangularMembership(enemyCount, 2f, 3f, 4f);

        return (distance + playerH + enemyH + enemyC) / 4;
    }

    private float CalculateFearMembership()
    {
        var distance = TriangularMembership(distanceToPlayer, 0f, nearDistance, mediumDistance);

        // var playerH = TriangularMembership(playerHealth, 0f, mediumHealthThreshold, highHealthThreshold);
        var playerH = TriangularMembership(playerHealth, 4f, 6f, float.MaxValue);

        // var enemyH = TriangularMembership(enemyHealth, 0f, lowHealthThreshold, mediumHealthThreshold);
        var enemyH = TriangularMembership(enemyHealth, 0f, 2f, 3f);

        // var enemyC = TriangularMembership(enemyCount, 0f, lowEnemyCountThreshold, mediumEnemyCountThreshold);
        var enemyC = TriangularMembership(enemyCount, 0f, 1f, 2f);

        return (distance + playerH + enemyH + enemyC) / 4;
    }

    private float CalculateBraveMembership()
    {
        var distance = TriangularMembership(distanceToPlayer, 0f, nearDistance, mediumDistance);

        // var playerH = TriangularMembership(playerHealth, 0f, lowHealthThreshold, lowHealthThreshold + 2);
        var playerH = TriangularMembership(playerHealth, 0f, 3f, 5f);

        // var enemyH = TriangularMembership(enemyHealth, mediumHealthThreshold, mediumHealthThreshold + 2, highHealthThreshold);
        var enemyH = TriangularMembership(enemyHealth, 2f, 3f, 4f);

        // var enemyC = TriangularMembership(enemyCount, mediumEnemyCountThreshold, highEnemyCountThreshold, float.MaxValue);
        var enemyC = TriangularMembership(enemyCount, 3f, 4f, float.MaxValue);

        return (distance + playerH + enemyH + enemyC) / 4;
    }

    private float CalculateAngryMembership()
    {
        var distance = TriangularMembership(distanceToPlayer, 0f, nearDistance, mediumDistance);

        // var playerH = TriangularMembership(playerHealth, 0f, lowHealthThreshold, mediumHealthThreshold);
        var playerH = TriangularMembership(playerHealth, 0f, 2f, 4f);

        // var enemyH = TriangularMembership(enemyHealth, 0f, lowHealthThreshold, mediumHealthThreshold);
        var enemyH = TriangularMembership(enemyHealth, 3f, 4f, 5f);

        // var enemyC = TriangularMembership(enemyCount, mediumEnemyCountThreshold, highEnemyCountThreshold, float.MaxValue);
        var enemyC = TriangularMembership(enemyCount, 1f, 2f, 3f);

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

            Debug.Log($"Calm: {calmEmotion}, Fear: {fearEmotion}, Brave: {braveEmotion}, Angry: {angryEmotion}");

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
        distanceToPlayer =
            Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position);
    }

    private void UpdatePlayerHealth()
    {
        playerHealth = FindObjectOfType<PlayerHealth>().currentHealth;
    }

    private void UpdateEnemyHealth()
    {
        enemyHealth = GetComponent<EnemyHealth>().currentHealth;
    }

    private void UpdateEnemyCount()
    {
        enemyCount = FindObjectsOfType<EnemyHealth>().Length;
    }

    private void Update()
    {
        UpdateDistanceToPlayer();
        UpdatePlayerHealth();
        UpdateEnemyHealth();
        UpdateEnemyCount();

        // Debug.Log($"Distance: {distanceToPlayer}, PlayerHealth: {playerHealth}, EnemyHealth: {enemyHealth}, Enemies: {enemyCount}");

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
}