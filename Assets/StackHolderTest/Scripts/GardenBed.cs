using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.StackHolderTest.Scripts
{
    public class GardenBed : MonoBehaviour
    {
        [SerializeField]
        private GameObject plantPrefab = default;

        [SerializeField]
        private int rows = 1;

        [SerializeField]
        private int columns = 1;

        [SerializeField]
        [Tooltip("Period between spawn of plants in seconds")]
        private float spawnPeriod = 1;

        [SerializeField]
        private Vector2 distanceBetweenPlants = Vector2.one;

        [SerializeField]
        private Transform initialSpawnTransform = default;

        private List<PickableItem> plants;

        private void Awake()
        {
            Assert.IsNotNull(plantPrefab);
            Assert.IsNotNull(initialSpawnTransform);

            Assert.IsTrue((rows * columns) > 0);
            Assert.IsTrue(spawnPeriod > 0);
            Assert.IsTrue(distanceBetweenPlants != Vector2.zero);

            plants = new List<PickableItem>(new PickableItem[rows * columns]);
        }

        private void Start()
        {
            StartCoroutine(PlantSpawnRoutine());
        }

        private IEnumerator PlantSpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnPeriod);

                AttemptSpawnPlant();
            }
        }

        private void AttemptSpawnPlant()
        {
            int newPlantIndex = GetFirstEmptyPlantPosition();
            if (newPlantIndex == -1)
            {
                return;
            }

            Vector2 newPlantCoords = GetPlantCoordsByIndex(newPlantIndex);            

            Vector3 spawnPosition = initialSpawnTransform.TransformPoint(newPlantCoords.x * distanceBetweenPlants.x, 0f, newPlantCoords.y * distanceBetweenPlants.y);

            GameObject newPlantGO = Instantiate(plantPrefab, spawnPosition, Quaternion.LookRotation(initialSpawnTransform.forward, initialSpawnTransform.up));
            
            PickableItem newPlantPickableItem = newPlantGO.GetComponent<PickableItem>();
            newPlantPickableItem.ItemPicked += OnPlantPicked;

            plants[newPlantIndex] = newPlantPickableItem;
        }

        private int GetFirstEmptyPlantPosition()
        {
            return plants.FindIndex(t => t == null);
        }

        private void OnPlantPicked(PickableItem caller)
        {
            caller.ItemPicked -= OnPlantPicked;

            int plantIndex = plants.IndexOf(caller);
            if (plantIndex == -1)
            {
                Debug.LogWarning("A plant in a gardenbed without being added to a list!");
            }

            plants[plantIndex] = null;
        }

        private Vector2 GetPlantCoordsByIndex(int index)
        {
            Vector2 coords = new Vector2();

            coords.x = index % columns;
            coords.y = index / columns;

            return coords;
        }
    }
}