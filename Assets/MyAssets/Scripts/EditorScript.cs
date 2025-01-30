#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using UnityEngine.SceneManagement;

//[CustomEditor(typeof(MonoBehaviour))] // Replace MonoBehaviour with the actual script type you want to use
public class EditorScript : EditorWindow
{
    [MenuItem("Tools/Modify Prefab Components")]
    static void Init()
    {
        EditorScript window = (EditorScript)EditorWindow.GetWindow(typeof(EditorScript));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Modify Prefab Components", EditorStyles.boldLabel);

        if (GUILayout.Button("Remove Rigidbodies"))
        {
            RemoveRigidbodies();
        }

        if (GUILayout.Button("Turn Off Receive Shadows"))
        {
            TurnOffShadows();
        }

        if (GUILayout.Button("Turn Off Cast Shadows"))
        {
            TurnOffCastShadows();
        }


        if (GUILayout.Button("Remove All Colliders and Add MeshColliders"))
        {
            RemoveAndAddMeshColliders();
        }

        if (GUILayout.Button("Create Empty Parents"))
        {
            CreateEmptyParents();
        }

        if (GUILayout.Button("Turn Off Contribute Global Illumination"))
        {
            TurnOffContributeGlobalIllumination();
        }
    }

    private void RemoveRigidbodies()
    {
        GameObject prefab = Selection.activeGameObject;

        if (prefab != null)
        {
            GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            if (prefabInstance != null)
            {
                Rigidbody[] rigidbodies = prefabInstance.GetComponentsInChildren<Rigidbody>(true);

                foreach (Rigidbody rb in rigidbodies)
                {
                    DestroyImmediate(rb);
                }

                PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.AutomatedAction);
                DestroyImmediate(prefabInstance);
                Debug.Log("Rigidbodies removed from the prefab.");
            }
            else
            {
                Debug.LogError("Failed to instantiate the prefab.");
            }
        }
        else
        {
            Debug.LogError("Please select a prefab in the Project window.");
        }
    }

    private void TurnOffShadows()
    {
        GameObject prefab = Selection.activeGameObject;

        if (prefab != null)
        {
            GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            if (prefabInstance != null)
            {
                TurnOffShadowsRecursive(prefabInstance.transform);
                PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.AutomatedAction);
                DestroyImmediate(prefabInstance);
                Debug.Log("Receive Shadows turned off for all MeshRenderers/SkinnedMeshRenderers in the prefab.");
            }
            else
            {
                Debug.LogError("Failed to instantiate the prefab.");
            }
        }
        else
        {
            Debug.LogError("Please select a prefab in the Project window.");
        }
    }

    private void TurnOffCastShadows()
    {
        GameObject prefab = Selection.activeGameObject;

        if (prefab != null)
        {
            GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            if (prefabInstance != null)
            {
                TurnOffCastShadowsRecursive(prefabInstance.transform);
                PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.AutomatedAction);
                DestroyImmediate(prefabInstance);
                Debug.Log("Cast Shadows turned off for all MeshRenderers/SkinnedMeshRenderers in the prefab.");
            }
            else
            {
                Debug.LogError("Failed to instantiate the prefab.");
            }
        }
        else
        {
            Debug.LogError("Please select a prefab in the Project window.");
        }
    }

    private void TurnOffShadowsRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.receiveShadows = false;
            }

            SkinnedMeshRenderer skinnedMeshRenderer = child.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.receiveShadows = false;
            }

            // Recursively call for child GameObjects
            TurnOffShadowsRecursive(child);
        }
    }

    private void TurnOffCastShadowsRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            SkinnedMeshRenderer skinnedMeshRenderer = child.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            // Recursively call for child GameObjects
            TurnOffCastShadowsRecursive(child);
        }
    }

    private void RemoveAndAddMeshColliders()
    {
        GameObject prefab = Selection.activeGameObject;

        if (prefab != null)
        {
            GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            if (prefabInstance != null)
            {
                RemoveAndAddMeshCollidersRecursive(prefabInstance.transform);
                PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.AutomatedAction);
                DestroyImmediate(prefabInstance);
                Debug.Log("Removed all colliders and added MeshColliders to all MeshRenderers and SkinnedMeshRenderers in the prefab.");
            }
            else
            {
                Debug.LogError("Failed to instantiate the prefab.");
            }
        }
        else
        {
            Debug.LogError("Please select a prefab in the Project window.");
        }
    }

    private void RemoveAndAddMeshCollidersRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Remove all existing colliders
            Collider[] existingColliders = child.GetComponents<Collider>();
            foreach (Collider collider in existingColliders)
            {
                DestroyImmediate(collider);
            }

            // Add MeshCollider to GameObjects with MeshRenderer or SkinnedMeshRenderer
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            SkinnedMeshRenderer skinnedMeshRenderer = child.GetComponent<SkinnedMeshRenderer>();

            if (meshRenderer != null)
            {
                MeshCollider meshCollider = child.gameObject.GetComponent<MeshCollider>();
                if (meshCollider == null)
                {
                    meshCollider = child.gameObject.AddComponent<MeshCollider>();
                }
            }
            else if (skinnedMeshRenderer != null)
            {
                MeshCollider meshCollider = child.gameObject.GetComponent<MeshCollider>();
                if (meshCollider == null)
                {
                    meshCollider = child.gameObject.AddComponent<MeshCollider>();
                    meshCollider.sharedMesh = skinnedMeshRenderer.sharedMesh;
                }
            }

            // Recursively call for child GameObjects
            RemoveAndAddMeshCollidersRecursive(child);
        }
    }

    private void CreateEmptyParents()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject selectedObject in selectedObjects)
        {
            GameObject parentObject = new GameObject("EmptyParent");
            parentObject.transform.SetParent(selectedObject.transform.parent, false);
            selectedObject.transform.SetParent(parentObject.transform);
        }

        Debug.Log("Created empty parents for selected game objects.");
    }


    private void TurnOffContributeGlobalIllumination()
    {
        GameObject prefab = Selection.activeGameObject;

        if (prefab != null)
        {
            GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            if (prefabInstance != null)
            {
                TurnOffContributeGlobalIlluminationRecursive(prefabInstance.transform);
                PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.AutomatedAction);
                DestroyImmediate(prefabInstance);
                Debug.Log("Turned off Contribute Global Illumination for all renderers in the prefab.");
            }
            else
            {
                Debug.LogError("Failed to instantiate the prefab.");
            }
        }
        else
        {
            Debug.LogError("Please select a prefab in the Project window.");
        }
    }

    private void TurnOffContributeGlobalIlluminationRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Renderer renderer = child.GetComponent<Renderer>();

            if (renderer != null)
            {
                // Turn off Contribute Global Illumination
                renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            }

            // Recursively call for child GameObjects
            TurnOffContributeGlobalIlluminationRecursive(child);
        }
    }

    //    [MenuItem("GameObject/Cut and Paste Collider", false, 0)]
    //    static void CutAndPasteCollider()
    //    {
    //        GameObject[] selectedObjects = Selection.gameObjects;

    //        foreach (GameObject selectedObject in selectedObjects)
    //        {
    //            // Check if the selected object is a prefab instance
    //            PrefabType prefabType = PrefabUtility.GetPrefabType(selectedObject);
    //            if (prefabType == PrefabType.PrefabInstance || prefabType == PrefabType.ModelPrefabInstance)
    //            {
    //                // Cut the collider component from the selected object
    //                Collider colliderComponent = selectedObject.GetComponent<Collider>();
    //                if (colliderComponent != null)
    //                {
    //                    // Serialize collider properties
    //                    string serializedProperties = SerializeColliderProperties(colliderComponent);

    //                    Undo.DestroyObjectImmediate(colliderComponent);

    //                    // Paste the collider component to the first child with the serialized properties
    //                    Transform firstChild = selectedObject.transform.GetChild(0);
    //                    Collider copiedCollider = Undo.AddComponent(firstChild.gameObject, colliderComponent.GetType()) as Collider;

    //                    // Deserialize and apply collider properties
    //                    DeserializeAndApplyColliderProperties(copiedCollider, serializedProperties);
    //                    Debug.Log("Collider cut and pasted on " + firstChild.name);
    //                }
    //                else
    //                {
    //                    Debug.LogWarning("No Collider component found on " + selectedObject.name);
    //                }
    //            }
    //            else
    //            {
    //                Debug.LogWarning(selectedObject.name + " is not a prefab instance");
    //            }
    //        }
    //    }

    //    static string SerializeColliderProperties(Collider collider)
    //    {
    //        SerializedObject serializedObject = new SerializedObject(collider);
    //        SerializedProperty iterator = serializedObject.GetIterator();

    //        string serializedData = "";

    //        while (iterator.NextVisible(true))
    //        {
    //            if (iterator.propertyType == SerializedPropertyType.Vector3)
    //            {
    //                serializedData += iterator.propertyPath + ":" + iterator.vector3Value.x + "," + iterator.vector3Value.y + "," + iterator.vector3Value.z + ";";
    //            }
    //            // Add other property types as needed

    //            if (iterator.propertyType == SerializedPropertyType.Float)
    //            {
    //                serializedData += iterator.propertyPath + ":" + iterator.floatValue + ";";
    //            }
    //            // Add other property types as needed
    //        }

    //        return serializedData;
    //    }

    //    static void DeserializeAndApplyColliderProperties(Collider collider, string serializedProperties)
    //    {
    //        SerializedObject serializedObject = new SerializedObject(collider);
    //        SerializedProperty iterator = serializedObject.GetIterator();

    //        string[] propertyData = serializedProperties.Split(';');

    //        foreach (string property in propertyData)
    //        {
    //            string[] keyValue = property.Split(':');

    //            if (keyValue.Length == 2)
    //            {
    //                SerializedProperty prop = serializedObject.FindProperty(keyValue[0]);

    //                if (prop != null)
    //                {
    //                    if (prop.propertyType == SerializedPropertyType.Vector3)
    //                    {
    //                        string[] vectorData = keyValue[1].Split(',');
    //                        Vector3 vectorValue = new Vector3(float.Parse(vectorData[0]), float.Parse(vectorData[1]), float.Parse(vectorData[2]));
    //                        prop.vector3Value = vectorValue;
    //                    }
    //                    // Add other property types as needed

    //                    if (prop.propertyType == SerializedPropertyType.Float)
    //                    {
    //                        prop.floatValue = float.Parse(keyValue[1]);
    //                    }
    //                    // Add other property types as needed
    //                }
    //            }
    //        }

    //        serializedObject.ApplyModifiedProperties();
    //    }
}
#endif