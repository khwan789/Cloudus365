using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WeightedGameObject))]
public class WeightedGameObjectDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Begin property
        EditorGUI.BeginProperty(position, label, property);

        // Get the SerializedProperties for `gameObject` and `weight`
        SerializedProperty gameObjectProp = property.FindPropertyRelative("gameObject");
        SerializedProperty weightProp = property.FindPropertyRelative("weight");

        // Calculate widths
        float gameObjectWidth = position.width * 0.7f; // 70% for GameObject
        float weightWidth = position.width * 0.3f;    // 30% for weight

        // Draw fields side by side
        Rect gameObjectRect = new Rect(position.x, position.y, gameObjectWidth, position.height);
        Rect weightRect = new Rect(position.x + gameObjectWidth + 5, position.y, weightWidth - 5, position.height);

        EditorGUI.PropertyField(gameObjectRect, gameObjectProp, GUIContent.none);
        EditorGUI.PropertyField(weightRect, weightProp, GUIContent.none);

        // End property
        EditorGUI.EndProperty();
    }
}

[System.Serializable]
public class WeightedGameObject
{
    public GameObject gameObject; // The GameObject to instantiate
    public float weight;          // Weight for this object
}
public class WeightedObjectSpawner : MonoBehaviour
{
    public List<WeightedGameObject> weightedObjects = new List<WeightedGameObject>(); // List of objects with weights
    public float noSpawnChance = 10f; // Chance of not spawning any object (as a weight)
    private Transform player; // Reference to the player
    public float spawnDistance = 10f; // Distance to check for spawning or destroying objects

    private GameObject currentObject; // Reference to the currently spawned object
    private bool playerInRange = false; // Tracks if the player is within the distance

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Determine initial state based on player distance
        if (IsPlayerWithinDistance())
        {
            playerInRange = true;
        }
        else
        {
            SpawnRandomObject();
            playerInRange = false;
        }
    }

    void Update()
    {
        bool isInRange = IsPlayerWithinDistance();

        if (isInRange && !playerInRange)
        {
            // Player has come into range; do nothing
            playerInRange = true;
        }
        else if (!isInRange && playerInRange)
        {
            // Player has left the range; destroy and respawn
            DestroyCurrentObject();
            SpawnRandomObject();
            playerInRange = false;
        }
    }

    private bool IsPlayerWithinDistance()
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) <= spawnDistance;
    }

    public void SpawnRandomObject()
    {
        // Calculate total weight including the noSpawnChance
        float totalWeight = noSpawnChance;
        foreach (var obj in weightedObjects)
        {
            totalWeight += obj.weight;
        }

        // Generate a random number between 0 and totalWeight
        float randomValue = Random.Range(0f, totalWeight);

        // Determine which object (if any) to instantiate
        float cumulativeWeight = noSpawnChance; // Start with noSpawnChance to account for it first

        // Check against the noSpawnChance
        if (randomValue < cumulativeWeight)
        {
            Debug.Log("No object spawned.");
            return;
        }

        // Iterate through the weighted objects and find the one to instantiate
        foreach (var obj in weightedObjects)
        {
            cumulativeWeight += obj.weight;
            if (randomValue < cumulativeWeight)
            {
                // Instantiate the new object and store a reference to it
                currentObject = Instantiate(obj.gameObject, transform.position, Quaternion.identity);
                Debug.Log($"Spawned: {obj.gameObject.name}");
                return;
            }
        }

        // Fallback (should never reach here unless weights are misconfigured)
        Debug.LogWarning("Random value did not match any object. No object spawned.");
    }

    private void DestroyCurrentObject()
    {
        if (currentObject != null)
        {
            if (Application.isPlaying)
            {
                Destroy(currentObject); // Safely destroy the object
            }
            else
            {
                DestroyImmediate(currentObject); // Handle if not in play mode
            }
            currentObject = null; // Clear the reference
        }
    }
}