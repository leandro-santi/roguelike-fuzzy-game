using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fuzzy
{
    public class FuzzyLogic : MonoBehaviour
    {
        // Representa um conjunto fuzzy triangular com propriedades para inicio - pico - fim.
        public class TriangularFuzzySet : FuzzySet // Conjunto Fuzzy Triangular
        {
            public float Start { get; set; }
            public float Peak { get; set; }
            public float End { get; set; }
            
            public TriangularFuzzySet(string name, float start, float peak, float end)
            {
                Name = name;
                Start = start;
                Peak = peak;
                End = end;
            }

            public override float GetMembership(float value)
            {
                if (value <= Start || value >= End)
                {
                    return 0f;
                }
                else if (value == Peak)
                {
                    return 1f;
                }
                else if (value < Peak)
                {
                    return (value - Start) / (Peak - Start);
                }
                else // value > Peak
                {
                    return (End - value) / (End - Peak);
                }
            }

            public override FuzzySet Union(FuzzySet other)
            {
                if (other is TriangularFuzzySet triangularFuzzySet)
                {
                    var result = new TriangularFuzzySet(Name + " or " + triangularFuzzySet.Name,
                        Math.Min(Start, triangularFuzzySet.Start),
                        (Peak + triangularFuzzySet.Peak) / 2f,
                        Math.Max(End, triangularFuzzySet.End));
                    return result;
                }

                throw new ArgumentException($"Cannot compute the union between {GetType()} and {other.GetType()}");
            }

            public override float Centroid(float membership)
            {
                if (membership == 0f)
                {
                    return float.NaN;
                }
                else if (membership == 1f)
                {
                    return Peak;
                }
                else
                {
                    float area = membership * (End - Start) / 2f;
                    float leftTriangle = (Peak - Start) * membership / 2f;
                    float rightTriangle = (End - Peak) * membership / 2f;
                    float centroid = Start + 2f * area / (leftTriangle + rightTriangle);
                    return centroid;
                }
            }
        }

        public class LinguisticVariable // Variável Linguística
        {
            public string Name { get; set; }
            public List<FuzzySet> FuzzySets { get; set; }

            public LinguisticVariable(string name)
            {
                Name = name;
                FuzzySets = new List<FuzzySet>();
            }

            public void AddFuzzySet(FuzzySet fuzzySet)
            {
                FuzzySets.Add(fuzzySet);
            }
        }

        public class FuzzyRule // Regra Fuzzy
        {
            public Dictionary<LinguisticVariable, FuzzySet> Antecedents { get; set; }
            public Dictionary<LinguisticVariable, FuzzySet> Consequents { get; set; }

            public FuzzyRule()
            {
                Antecedents = new Dictionary<LinguisticVariable, FuzzySet>();
                Consequents = new Dictionary<LinguisticVariable, FuzzySet>();
            }
        }

        public class FuzzyRuleSet // Conjunto de Regras Fuzzy
        {
            public List<FuzzyRule> Rules { get; set; }

            public FuzzyRuleSet()
            {
                Rules = new List<FuzzyRule>();
            }

            public void AddRule(FuzzyRule rule)
            {
                Rules.Add(rule);
            }
        }

        public class FuzzyInferenceSystem // Sistema de Inferência Fuzzy
        {
            public float Infer(Dictionary<LinguisticVariable, float> inputs, LinguisticVariable outputVariable,
                FuzzyRuleSet ruleSet)
            {
                // Step 1: Fuzzification
                Dictionary<FuzzySet, float> inputMemberships = new Dictionary<FuzzySet, float>();
                foreach (var input in inputs)
                {
                    if (!ruleSet.Rules.Any(rule => rule.Antecedents.ContainsKey(input.Key)))
                    {
                        throw new ArgumentException(
                            $"Linguistic variable {input.Key.Name} is not used in any rule antecedent.");
                    }

                    foreach (var fuzzySet in input.Key.FuzzySets)
                    {
                        float membership = fuzzySet.GetMembership(input.Value);
                        inputMemberships.Add(fuzzySet, membership);
                    }
                }

                // Step 2: Rule evaluation
                Dictionary<FuzzySet, float> ruleOutputs = new Dictionary<FuzzySet, float>();
                foreach (var rule in ruleSet.Rules)
                {
                    if (!rule.Antecedents.Keys.All(input => inputs.ContainsKey(input)))
                    {
                        throw new ArgumentException(
                            $"Not all antecedent linguistic variables of rule {rule} are present in the inputs.");
                    }

                    float ruleOutput = float.MaxValue;
                    foreach (var antecedent in rule.Antecedents)
                    {
                        float antecedentMembership = inputMemberships[antecedent.Value];
                        if (antecedentMembership < ruleOutput)
                        {
                            ruleOutput = antecedentMembership;
                        }
                    }

                    if (ruleOutput < float.MaxValue)
                    {
                        if (!rule.Consequents.ContainsKey(outputVariable))
                        {
                            throw new ArgumentException(
                                $"Output variable {outputVariable.Name} is not present in the consequents of rule {rule}.");
                        }

                        ruleOutputs[rule.Consequents[outputVariable]] = ruleOutput;
                    }
                }

                if (ruleOutputs.Count == 0)
                {
                    throw new InvalidOperationException("No rules were fired.");
                }

                // Step 3: Aggregation
                FuzzySet aggregatedFuzzySet = null;
                float aggregatedMembership = 0f;
                foreach (var outputMembership in ruleOutputs)
                {
                    if (aggregatedFuzzySet == null)
                    {
                        aggregatedFuzzySet = outputMembership.Key;
                    }
                    else
                    {
                        aggregatedFuzzySet = aggregatedFuzzySet.Union(outputMembership.Key);
                    }

                    aggregatedMembership = Math.Max(aggregatedMembership, outputMembership.Value);
                }

                // Step 4: Defuzzification
                if (aggregatedFuzzySet == null)
                {
                    throw new InvalidOperationException("No rule output fuzzy sets were generated.");
                }

                return aggregatedFuzzySet.Centroid(aggregatedMembership);
            }
        }

        public class FuzzySet // Classe base para conjuntos fuzzy
        {
            public string Name { get; set; }

            public virtual float GetMembership(float value)
            {
                throw new NotImplementedException();
            }

            public virtual FuzzySet Union(FuzzySet other)
            {
                throw new NotImplementedException();
            }

            public virtual float Centroid(float membership)
            {
                throw new NotImplementedException();
            }
        }
    }
}