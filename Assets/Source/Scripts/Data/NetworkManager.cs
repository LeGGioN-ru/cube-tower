using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class NetworkManager : ICubesDatabase
{
    public UniTask<List<CubeModel>> TakeCubes()
    {
        //Тут уже будет реализация через Rest Api      
        throw new NotImplementedException();
    }
}
