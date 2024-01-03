using System;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{
    Calm,
    Fear,
    Anger,
    Courage
}

public class FuzzyLogicExample : MonoBehaviour
{
    public float distanceThreshold = 10f;

    // Defina os parâmetros da lógica fuzzy
    public float calmPeakDistance = 5f;
    public float fearPeakDistance = 15f;
    public float angerPeakDistance = 25f;
    public float couragePeakDistance = 35f;

    // Defina os parâmetros das funções de pertinência
    public float fuzzyMembershipPeak = 1f;

    public Emotion CalculateEmotion(float distance)
    {
        float calmMembership = CalculateCalmMembership(distance);
        float fearMembership = CalculateFearMembership(distance);
        float angerMembership = CalculateAngerMembership(distance);
        float courageMembership = CalculateCourageMembership(distance);

        float maxMembership = Mathf.Max(calmMembership, fearMembership, angerMembership, courageMembership);

        if (maxMembership == calmMembership)
        {
            return Emotion.Calm;
        }
        else if (maxMembership == fearMembership)
        {
            return Emotion.Fear;
        }
        else if (maxMembership == angerMembership)
        {
            return Emotion.Anger;
        }
        else
        {
            return Emotion.Courage;
        }
    }

    private float CalculateCalmMembership(float distance)
    {
        return CalculateFuzzyMembership(distance, calmPeakDistance);
    }

    private float CalculateFearMembership(float distance)
    {
        return CalculateFuzzyMembership(distance, fearPeakDistance);
    }

    private float CalculateAngerMembership(float distance)
    {
        return CalculateFuzzyMembership(distance, angerPeakDistance);
    }

    private float CalculateCourageMembership(float distance)
    {
        return CalculateFuzzyMembership(distance, couragePeakDistance);
    }

    private float CalculateFuzzyMembership(float inputValue, float peakValue)
    {
        // Função de pertinência triangular
        float halfWidth = distanceThreshold / 2f;
        float start = peakValue - halfWidth;
        float end = peakValue + halfWidth;

        if (inputValue < start || inputValue > end)
        {
            return 0f; // Fora do intervalo
        }
        else if (inputValue < peakValue)
        {
            return (inputValue - start) / (peakValue - start);
        }
        else
        {
            return 1f - (inputValue - peakValue) / (end - peakValue);
        }
    }
}