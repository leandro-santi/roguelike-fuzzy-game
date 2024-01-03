using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogic2 : MonoBehaviour
{
    
    public float distanceThreshold = 10f;
    public Transform player;

    // Parâmetros da lógica fuzzy
    public float calmPeakDistance = 5f;
    public float fearPeakDistance = 15f;
    public float angerPeakDistance = 25f;
    public float couragePeakDistance = 35f;

    // Parâmetros das funções de pertinência
    public float fuzzyMembershipPeak = 1f;

    // Parâmetros para transição suave entre emoções
    public float transitionSpeed = 2f;
    private Emotion currentEmotion;
    private float currentEmotionMembership;

    void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, player.position);

        // Calcular a nova emoção com base na distância
        Emotion newEmotion = CalculateEmotion(playerDistance);

        // Transição suave entre emoções
        TransitionEmotion(newEmotion);

        // Use a emoção atual (pode ser usada no comportamento do inimigo)
        HandleEmotion(currentEmotion);
    }

    private void TransitionEmotion(Emotion newEmotion)
    {
        // Se a nova emoção for diferente da atual, faça uma transição suave
        if (newEmotion != currentEmotion)
        {
            currentEmotionMembership = Mathf.Lerp(currentEmotionMembership, 0f, Time.deltaTime * transitionSpeed);

            // Se a transição estiver quase completa, atualize para a nova emoção
            if (currentEmotionMembership < 0.01f)
            {
                currentEmotion = newEmotion;
                currentEmotionMembership = 1f;
            }
        }
        else
        {
            // Se a emoção não mudou, aumente a pertinência gradualmente
            currentEmotionMembership = Mathf.Lerp(currentEmotionMembership, fuzzyMembershipPeak, Time.deltaTime * transitionSpeed);
        }
    }

    private void HandleEmotion(Emotion emotion)
    {
        // Implemente o comportamento do inimigo com base na emoção atual
        switch (emotion)
        {
            case Emotion.Calm:
                // Comportamento para a emoção calma
                break;
            case Emotion.Fear:
                // Comportamento para a emoção de medo
                break;
            case Emotion.Anger:
                // Comportamento para a emoção de ira
                break;
            case Emotion.Courage:
                // Comportamento para a emoção de coragem
                break;
            default:
                break;
        }
    }

    private Emotion CalculateEmotion(float distance)
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

    // ... (restante do código permanece inalterado)
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
