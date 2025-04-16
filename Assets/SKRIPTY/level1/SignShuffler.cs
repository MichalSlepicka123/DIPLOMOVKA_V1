using System.Collections.Generic;
using UnityEngine;

public class SignShuffler : MonoBehaviour
{
    public static SignShuffler Instance;

    public Transform znackyParent;
    public Transform[] spawnZones; // napr. Zone1, Zone2, Zone3, Zone4
    public float spacing = 3f;

    public List<Transform> randomizedSigns = new List<Transform>();
    public List<List<Transform>> groupedSigns = new List<List<Transform>>(); // pre kameru

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
        groupedSigns.Clear();

        int signIndex = 0;

        for (int zoneIndex = 0; zoneIndex < spawnZones.Length; zoneIndex++)
        {
            Vector3 basePos = spawnZones[zoneIndex].position;
            List<Transform> zoneGroup = new List<Transform>();

            for (int i = 0; i < 5; i++) // 5 znaèiek na zónu
            {
                if (signIndex >= randomizedSigns.Count)
                {
                    Debug.LogWarning("Nedostatok znaèiek pre všetky zóny!");
                    break;
                }

                Transform sign = randomizedSigns[signIndex];
                sign.position = basePos + new Vector3(i * spacing, 0, 0);
                zoneGroup.Add(sign);

                signIndex++;
            }

            groupedSigns.Add(zoneGroup);
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
