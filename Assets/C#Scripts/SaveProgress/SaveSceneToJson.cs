using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class SaveSceneToJson : MonoBehaviour
{
    [System.Serializable]
    public class ObjectData
    {
        public string prefabName;
        public Vector3 position;
        // Add any other parameters you want to include in the JSON
    }

    [System.Serializable]
    public class ObjectDataListWrapper
    {
        public List<ObjectData> objectDataList;
    }

    public GameObject[] prefabList;

    private List<GameObject> instantiatedObjects;

    public void SaveSceneObjectsToJson()
    {
        // Get all GameObjects in the scene
        GameObject[] sceneObjects = FindObjectsOfType<GameObject>();

        // Collect object data
        List<ObjectData> objectDataList = new List<ObjectData>();
        foreach (GameObject obj in sceneObjects)
        {
            if (obj != gameObject) // Exclude the script's GameObject itself
            {
                ObjectData objectData = new ObjectData();
                objectData.prefabName = obj.name; // Store the prefab name
                objectData.position = obj.transform.position;
                // Add any other parameters you want to include in the JSON

                objectDataList.Add(objectData);
            }
        }

        // Wrap the object data list
        ObjectDataListWrapper wrapper = new ObjectDataListWrapper();
        wrapper.objectDataList = objectDataList;

        // Convert object data to JSON
        string json = JsonUtility.ToJson(wrapper, true);

        // Prompt user to select the save location and filename
        string savePath = EditorUtility.SaveFilePanel("Save JSON", "", "scene_objects", "json");
        if (!string.IsNullOrEmpty(savePath))
        {
            // Save JSON to the selected file
            File.WriteAllText(savePath, json);
            Debug.Log("Scene objects saved to JSON file: " + savePath);
        }
        else
        {
            Debug.Log("Save operation canceled by the user.");
        }
    }

    public void LoadSceneObjectsFromJson()
    {
        // Prompt user to select the JSON file
        string loadPath = EditorUtility.OpenFilePanel("Load JSON", "", "json");
        if (!string.IsNullOrEmpty(loadPath))
        {
            // Read the JSON file
            string json = File.ReadAllText(loadPath);

            // Convert JSON to object data
            ObjectDataListWrapper wrapper = JsonUtility.FromJson<ObjectDataListWrapper>(json);

            // Create objects from object data and keep track of instantiated objects
            instantiatedObjects = new List<GameObject>();
            foreach (ObjectData objectData in wrapper.objectDataList)
            {
                GameObject prefab = GetPrefabByName(objectData.prefabName);
                if (prefab != null)
                {
                    GameObject newObject = Instantiate(prefab, objectData.position, Quaternion.identity);
                    instantiatedObjects.Add(newObject);
                    // Set any other parameters based on the object data
                }
            }

            // Delete any remaining objects in the scene that were not loaded from the JSON
            DeleteExtraObjects();

            Debug.Log("Scene objects loaded from JSON file: " + loadPath);
        }
        else
        {
            Debug.Log("Load operation canceled by the user.");
        }
    }

    private GameObject GetPrefabByName(string prefabName)
    {
        foreach (GameObject prefab in prefabList)
        {
            if (prefab.name == prefabName)
            {
                return prefab;
            }
        }
        Debug.LogWarning("Prefab not found: " + prefabName);
        return null;
    }

    private void DeleteExtraObjects()
    {
        GameObject[] sceneObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in sceneObjects)
        {
            if (obj != gameObject && !instantiatedObjects.Contains(obj))
            {
                if (obj.name != "Main Camera" && obj.name != "Canvas" && obj.name != "EventSystem")
                {
                    Destroy(obj);
                }
            }
        }
    }
}