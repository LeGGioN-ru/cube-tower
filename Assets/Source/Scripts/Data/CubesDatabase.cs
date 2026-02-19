using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Cube Tower/Cubes Database", fileName = "Cubes Database")]
public class CubesDatabase : ScriptableObject, ICubesDatabase
{
    [SerializeField] private List<CubeModel> _cubeDatabaseRecords;

    private void OnValidate()
    {
        foreach (CubeModel cubeDatabaseRecord in _cubeDatabaseRecords)
        {
            cubeDatabaseRecord.Name = cubeDatabaseRecord.ColorType.ToString();
        }
    }


    public UniTask<List<CubeModel>> TakeCubes()
    {
        return UniTask.FromResult(_cubeDatabaseRecords);
    }
}