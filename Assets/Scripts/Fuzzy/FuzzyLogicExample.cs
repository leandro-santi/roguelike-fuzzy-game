using System;
using System.Collections.Generic;
using UnityEngine;
using Fuzzy;

public class FuzzyLogicExample : MonoBehaviour
{
    public void Start()
    {
        // Create the linguistic variables
        FuzzyLogic.LinguisticVariable distance = new FuzzyLogic.LinguisticVariable("Distance");
        FuzzyLogic.LinguisticVariable speed = new FuzzyLogic.LinguisticVariable("Speed");
        FuzzyLogic.LinguisticVariable acceleration = new FuzzyLogic.LinguisticVariable("Acceleration");

        // Create the fuzzy sets for each linguistic variable
        FuzzyLogic.FuzzySet near = new FuzzyLogic.TriangularFuzzySet("Near", 0, 0, 10);
        FuzzyLogic.FuzzySet far = new FuzzyLogic.TriangularFuzzySet("Far", 5, 10, 15);
        FuzzyLogic.FuzzySet slow = new FuzzyLogic.TriangularFuzzySet("Slow", 0, 0, 5);
        FuzzyLogic.FuzzySet fast = new FuzzyLogic.TriangularFuzzySet("Fast", 5, 10, 15);
        FuzzyLogic.FuzzySet negative = new FuzzyLogic.TriangularFuzzySet("Negative", -15, -10, -5);
        FuzzyLogic.FuzzySet positive = new FuzzyLogic.TriangularFuzzySet("Positive", 5, 10, 15);

        // Add the fuzzy sets to their corresponding linguistic variables
        distance.AddFuzzySet(near);
        distance.AddFuzzySet(far);
        speed.AddFuzzySet(slow);
        speed.AddFuzzySet(fast);
        acceleration.AddFuzzySet(negative);
        acceleration.AddFuzzySet(positive);

        // Create the fuzzy rules
        FuzzyLogic.FuzzyRule rule1 = new FuzzyLogic.FuzzyRule();
        rule1.Antecedents.Add(distance, near);
        rule1.Antecedents.Add(speed, slow);
        rule1.Consequents.Add(acceleration, negative);

        FuzzyLogic.FuzzyRule rule2 = new FuzzyLogic.FuzzyRule();
        rule2.Antecedents.Add(distance, far);
        rule2.Antecedents.Add(speed, fast);
        rule2.Consequents.Add(acceleration, positive);

        // Add the fuzzy rules to a rule set
        FuzzyLogic.FuzzyRuleSet ruleSet = new FuzzyLogic.FuzzyRuleSet();
        ruleSet.AddRule(rule1);
        ruleSet.AddRule(rule2);

        // Create the fuzzy inference system
        FuzzyLogic.FuzzyInferenceSystem fis = new FuzzyLogic.FuzzyInferenceSystem();

        // Define the input values
        Dictionary<FuzzyLogic.LinguisticVariable, float> inputs = new Dictionary<FuzzyLogic.LinguisticVariable, float>();
        inputs.Add(distance, 6);
        inputs.Add(speed, 7);

        // Infer the output value
        float output = fis.Infer(inputs, acceleration, ruleSet);

        // Display the output in console
        print(output);
    }
    
    // public Transform playerTransform; // Reference to the player's transform
    //
    // // Define the linguistic variables and their associated fuzzy sets
    // private FuzzyLogic.LinguisticVariable distanceVariable;
    // private FuzzyLogic.LinguisticVariable actionVariable;
    //
    // // Define the fuzzy rules
    // private FuzzyLogic.FuzzyRule rule1;
    // private FuzzyLogic.FuzzyRule rule2;
    // private FuzzyLogic.FuzzyRule rule3;
    //
    // // Define the fuzzy rule set
    // private FuzzyLogic.FuzzyRuleSet ruleSet;
    //
    // // Define the fuzzy inference system
    // private FuzzyLogic.FuzzyInferenceSystem fis;
    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //     // Define the "distance" linguistic variable
    //     distanceVariable = new FuzzyLogic.LinguisticVariable("distance");
    //     distanceVariable.AddFuzzySet(new FuzzyLogic.TriangularFuzzySet("close", 0f, 20f, 40f));
    //     distanceVariable.AddFuzzySet(new FuzzyLogic.TriangularFuzzySet("medium", 20f, 40f, 60f));
    //     distanceVariable.AddFuzzySet(new FuzzyLogic.TriangularFuzzySet("far", 40f, 60f, 80f));
    //
    //     // Define the "action" linguistic variable
    //     actionVariable = new FuzzyLogic.LinguisticVariable("action");
    //     actionVariable.AddFuzzySet(new FuzzyLogic.TriangularFuzzySet("attack", 0f, 0f, 0.5f));
    //     actionVariable.AddFuzzySet(new FuzzyLogic.TriangularFuzzySet("retreat", 0.5f, 1f, 1f));
    //
    //     // Define the fuzzy rules
    //     rule1 = new FuzzyLogic.FuzzyRule();
    //     rule1.Antecedents[distanceVariable] = distanceVariable.FuzzySets[0];
    //     rule1.Consequents[actionVariable] = actionVariable.FuzzySets[0];
    //
    //     rule2 = new FuzzyLogic.FuzzyRule();
    //     rule2.Antecedents[distanceVariable] = distanceVariable.FuzzySets[1];
    //     rule2.Consequents[actionVariable] = actionVariable.FuzzySets[0];
    //
    //     rule3 = new FuzzyLogic.FuzzyRule();
    //     rule3.Antecedents[distanceVariable] = distanceVariable.FuzzySets[2];
    //     rule3.Consequents[actionVariable] = actionVariable.FuzzySets[1];
    //
    //     // Define the fuzzy rule set
    //     ruleSet = new FuzzyLogic.FuzzyRuleSet();
    //     ruleSet.AddRule(rule1);
    //     ruleSet.AddRule(rule2);
    //     ruleSet.AddRule(rule3);
    //
    //     // Define the fuzzy inference system
    //     fis = new FuzzyLogic.FuzzyInferenceSystem();
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     // Calculate the distance between the enemy and the player
    //     float distance = Vector3.Distance(transform.position, playerTransform.position);
    //     
    //     Debug.Log("Distance to the player:" + distance);
    //
    //     // Set the inputs for the fuzzy inference system
    //     var inputs = new Dictionary<FuzzyLogic.LinguisticVariable, float>();
    //     inputs[distanceVariable] = distance;
    //
    //     // Infer the action using the fuzzy inference system
    //     float actionValue = fis.Infer(inputs, actionVariable, ruleSet);
    //     
    //     Debug.Log(actionValue);
    //
    //     // Take action based on the fuzzy inference result
    //     if (actionValue < 0.5f)
    //     {
    //         // Attack the player
    //         Debug.Log("Attacking player");
    //     }
    //     else
    //     {
    //         // Retreat
    //         Debug.Log("Retreating");
    //     }
    // }
}