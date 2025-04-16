using System.Collections.Generic;
using UnityEngine;

public class SignShuffler : MonoBehaviour
{
    public static SignShuffler Instance;

    public Transform znackyParent;
    public Transform[] spawnZones; // napr. Zone1, Zone2, Zone3, Zone4
    public float spacing = 3f;
    public List<Transform> randomizedSigns = new List<Transform>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        List<Transform> signs = new List<Transform>();

        foreach (Transform child in znackyParent)
        {
            signs.Add(child);
        }

        randomizedSigns = ShuffleList(signs);

        int zoneIndex = 0;

        for (int i = 0; i < randomizedSigns.Count; i++)
        {
            int localIndex = i % 5; // poradie v rámci jednej zóny

            if (i > 0 && i % 5 == 0) zoneIndex++; // každých 5 znaèiek prejde do ïalšej zóny

            if (zoneIndex < spawnZones.Length)
            {
                Vector3 basePos = spawnZones[zoneIndex].position;
                randomizedSigns[i].position = basePos + new Vector3(localIndex * spacing, 0, 0);
            }
            else
            {
                Debug.LogWarning("Nastavených menej spawn zón ako treba!");
            }
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


