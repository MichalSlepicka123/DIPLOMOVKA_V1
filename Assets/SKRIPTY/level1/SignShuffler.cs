using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignShuffler : MonoBehaviour
{
    public static SignShuffler Instance;

    public Transform znackyParent;
    public Vector3 startPosition = new Vector3(0, 0, 0);
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

        for (int i = 0; i < randomizedSigns.Count; i++)
        {
            Vector3 pos = startPosition + new Vector3(i * spacing, 0, 0);
            randomizedSigns[i].position = pos;
        }
    }

    private List<Transform> ShuffleList(List<Transform> list)
    {
        List<Transform> newList = new List<Transform>(list);
        for (int i = 0; i < newList.Count; i++)
        {
            Transform temp = newList[i];
            int rand = Random.Range(i, newList.Count);
            newList[i] = newList[rand];
            newList[rand] = temp;
        }
        return newList;
    }
}

