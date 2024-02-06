using UnityEngine;

public class FuzzyController : MonoBehaviour
{
    public string emotion;

    private float distanceToPlayer;
    private float nearDistance = 5f;
    private float mediumDistance = 10f;
    private float farDistance = 15f;

    private float TriangularMembership(float x, float a, float b, float c)
    {
        return Mathf.Max(0, Mathf.Min(Mathf.Min((x - a) / (b - a), (c - x) / (c - b)), 1));
    }

    private float CalculateCalmMembership()
    {
        return TriangularMembership(distanceToPlayer, 0f, nearDistance, mediumDistance);
    }

    private float CalculateFearMembership()
    {
        return TriangularMembership(distanceToPlayer, nearDistance, mediumDistance, farDistance);
    }

    private float CalculateBraveMembership()
    {
        return TriangularMembership(distanceToPlayer, mediumDistance - 2f, farDistance - 2f, farDistance + 2f);
    }

    private float CalculateAngryMembership()
    {
        return TriangularMembership(distanceToPlayer, farDistance - 2f, farDistance + 2f, float.MaxValue);
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

            // Debug.Log($"Calm: {calmEmotion}, Fear: {fearEmotion}, Brave: {braveEmotion}, Angry: {angryEmotion}");

            string emotionalState = DetermineEmotionalState(calmEmotion, fearEmotion, braveEmotion, angryEmotion);

            // Debug.Log($"Emotional State: {emotionalState}");

            return emotionalState;
        }

        return "";
    }

    private string DetermineEmotionalState(float calm, float fear, float brave, float angry)
    {
        float maxEmotion = Mathf.Max(calm, fear, brave, angry);

        if (maxEmotion == calm)
            return "Calm";
        else if (maxEmotion == fear)
            return "Fear";
        else if (maxEmotion == brave)
            return "Brave";
        else
            return "Angry";
    }

    private void UpdateDistanceToPlayer()
    {
        distanceToPlayer =
            Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position);
    }

    private void Update()
    {
        UpdateDistanceToPlayer();
        emotion = FuzzyEmotionCalculation();
    }

    public string ReturnEmotion()
    {
        return emotion;
    }
}