using UnityEngine;

public class PrefabSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabArray = new GameObject[4];
    [SerializeField] private Transform spawnPoint;
    private int currentPrefabIndex = 0;

    private void Start()
    {
        // Ensure the first prefab is visible at the start
        if (prefabArray.Length > 0 && prefabArray[0] != null)
        {
            ChangePrefab(0);
        }
        else
        {
            Debug.LogWarning("Prefab array is empty or the first prefab is not assigned.");
        }
    }


    /// Changes the current prefab to the one specified by the index in the array.
  
    /// <param name="index">Index of the prefab to use (0-3).</param>
    public void ChangePrefab(int index)
    {
        if (index < 0 || index >= prefabArray.Length)
        {
            Debug.LogError("Index out of range. Please use an index between 0 and " + (prefabArray.Length - 1));
            return;
        }

        // Destroy the existing child (if any) before switching
        if (spawnPoint.childCount > 0)
        {
            Destroy(spawnPoint.GetChild(0).gameObject);
        }

        // Instantiate the selected prefab at the spawn point
        GameObject newPrefab = Instantiate(prefabArray[index], spawnPoint.position, spawnPoint.rotation, spawnPoint);
        currentPrefabIndex = index;
    }

   
    /// Cycles to the next prefab in the array.
   
    public void CycleToNextPrefab()
    {
        int nextIndex = (currentPrefabIndex + 1) % prefabArray.Length;
        ChangePrefab(nextIndex);
    }

  
    /// Cycles to the previous prefab in the array.
 
    public void CycleToPreviousPrefab()
    {
        int previousIndex = (currentPrefabIndex - 1 + prefabArray.Length) % prefabArray.Length;
        ChangePrefab(previousIndex);
    }
}