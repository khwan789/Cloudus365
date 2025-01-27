using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PlanetObjectDistributor : MonoBehaviour
{
    public GameObject objectToPlace; // The prefab to place around the planet
    public float radius = 10f; // The radius of the planet
    public int numberOfObjects = 10; // Number of objects to place evenly

    // Function to place objects evenly
    public void PlaceObjects()
    {
        if (objectToPlace == null)
        {
            Debug.LogError("No object selected to place!");
            return;
        }

        // Clear existing child objects
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        // Calculate and place objects
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Calculate position on a sphere
            float angle = i * Mathf.PI * 2 / numberOfObjects; // Evenly spaced angle
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 position = new Vector3(x, y, 0);

            // Instantiate the object
            GameObject obj = PrefabUtility.InstantiatePrefab(objectToPlace) as GameObject;
            obj.transform.position = transform.position + position;
            obj.transform.parent = transform;
        }
    }
}

[CustomEditor(typeof(PlanetObjectDistributor))]
public class PlanetObjectDistributorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Reference to the script
        PlanetObjectDistributor distributor = (PlanetObjectDistributor)target;

        // Draw the default inspector
        DrawDefaultInspector();

        // Add a button to the inspector
        if (GUILayout.Button("Place Objects"))
        {
            distributor.PlaceObjects();
        }
    }
}
