using System.Collections.Generic;
using UnityEngine;

public class SignShuffler : MonoBehaviour
{
    public static SignShuffler Instance;

    public Transform znackyParent;
    public Transform[] spawnZones;
    public float spacing = 3f;

    public List<Transform> randomizedSigns = new List<Transform>();
    public List<List<Transform>> groupedSigns = new List<List<Transform>>();  // pre CameraController

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Získaj všetky znaèky
        List<Transform> signs = new List<Transform>();
        foreach (Transform child in znackyParent)
        {
            signs.Add(child);
        }

        randomizedSigns = ShuffleList(signs);

        groupedSigns.Clear();

        int zoneIndex = 0;

        for (int i = 0; i < randomizedSigns.Count; i++)
        {
            int localIndex = i % 5;

            if (i > 0 && i % 5 == 0) zoneIndex++;

            if (zoneIndex >= spawnZones.Length)
            {
                Debug.LogWarning("Nedostatok zón pre všetky znaèky!");
                break;
            }

            Vector3 basePos = spawnZones[zoneIndex].position;
            randomizedSigns[i].position = basePos + new Vector3(localIndex * spacing, 0, 0);
        }

        // Zoskup znaèky po 5 do groupedSigns
        for (int i = 0; i < randomizedSigns.Count; i += 5)
        {
            int count = Mathf.Min(5, randomizedSigns.Count - i);
            List<Transform> group = randomizedSigns.GetRange(i, count);

            // Zoradíme znaèky pod¾a X pozície (z¾ava doprava)
            group.Sort((a, b) => a.position.x.CompareTo(b.position.x));

            groupedSigns.Add(group);
        }
    }

    private List<Transform> ShuffleList(List<Transform> list)
    {
        List<Transform> newList = new List<Transform>(list);
        for (int i = 0; i < newList.Count; i++)
        {
            int rand = Random.Range(i, newList.Count);
            Transform temp = newList[i];
            newList[i] = newList[rand];
            newList[rand] = temp;
        }
        return newList;
    }
}
