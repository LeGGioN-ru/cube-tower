using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GameSaveManager : MonoBehaviour, IGameSaveManager
{
    [SerializeField] private TowerZone _towerZone;
    [SerializeField] private GameCubeView _template;
    private const string SaveKey = "GameProgress";

    [Inject] private CubeFactory _cubeFactory;

    private void Start()
    {
        LoadProgress();
    }

    public void SaveProgress()
    {
        List<CubeModel> cubes = new List<CubeModel>();

        foreach (CubePresenter presenter in _towerZone.Presenters)
        {
            presenter.CubeView.transform.DOComplete();
            presenter.SavePosition();
            cubes.Add(presenter.CubeModel);
        }

        SaveData data = new SaveData { Cubes = cubes };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);

            SaveData data = JsonUtility.FromJson<SaveData>(json);
            List<CubePresenter> presenters = new List<CubePresenter>();

            foreach (CubeModel model in data.Cubes)
            {
                CubePresenter presenter = _cubeFactory.Create(_template, model, _towerZone.transform);
                presenter.CubeView.transform.position = new Vector3(model.PositionX, model.PositionY, model.PositionZ);
                presenters.Add(presenter);
            }

            _towerZone.LoadPresenters(presenters);
        }
    }
}

[Serializable]
public class SaveData
{
    public List<CubeModel> Cubes;
}