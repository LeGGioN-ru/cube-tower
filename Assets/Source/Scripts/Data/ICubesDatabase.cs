using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface ICubesDatabase
{
    UniTask<List<CubeModel>> TakeCubes();
}