using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        var list1 = new List<TestData>();

        for (int i = 0; i < 10; i++)
        {
            list1.Add(new TestData(Random.Range(0, 10)));
        }

        for (int i = 0; i < 10; i++)
        {
            Debug.Log(list1[i].number);
        }

        var list2 = new List<TestData>(list1);

        for (int i = 0; i < 10; i++)
        {
            Debug.Log(list2[i].number);

            var data = list2[i];
            data.number = Random.Range(0, 10);
            list2[i] = data;
        }

        for (int i = 0; i < 10; i++)
        {
            Debug.Log(list1[i].number);
        }
    }
}


public struct TestData
{
    public int number;

    public TestData(int number)
    {
        this.number = number;
    }
}