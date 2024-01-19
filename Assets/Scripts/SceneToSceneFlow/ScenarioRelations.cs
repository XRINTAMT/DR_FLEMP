using UnityEngine;

[System.Serializable]
public struct FollowingScenario
{
    public string Name;
    public string Next;
}

[CreateAssetMenu(fileName = "ScenarioRelations", menuName = "Create Scenario Relations")]
public class ScenarioRelations : ScriptableObject
{
    public FollowingScenario[] followingScenarios;
    // Add any other variables you need

    public string GetNext(string targetName)
    {
        foreach (var scenario in followingScenarios)
        {
            if (scenario.Name == targetName)
            {
                return scenario.Next;
            }
        }

        // If the targetName is not found, you can return null or an appropriate default value
        return null;
    }
}
