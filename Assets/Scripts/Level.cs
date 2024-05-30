using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Level",menuName = "ShikakuLevel")]
public class Level : ScriptableObject
{
    public int Rows;
    public int Columns;
    public List<int> Data;
    public List<Vector2Int> NumberPositions;
    public int NumberValue(Vector2Int pos)
    {
        int find = Data[pos.x * Columns + pos.y];
        int result = 0;
        //Debug.Log(find);
        foreach (int d in Data)
        {
            if (d == find)
            {
                result++;
            }
        }
        return result;
    }
}
