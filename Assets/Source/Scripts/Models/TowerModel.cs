using System.Collections.Generic;
using System.Linq;

public class TowerModel
{
    private List<CubePresenter> _presenters = new List<CubePresenter>();

    public IReadOnlyList<CubePresenter> Presenters => _presenters;
    public int Count => _presenters.Count;
    public CubePresenter this[int index] => _presenters[index];

    public void Add(CubePresenter presenter) => _presenters.Add(presenter);
    public bool Remove(CubePresenter presenter) => _presenters.Remove(presenter);
    public int IndexOf(CubePresenter presenter) => _presenters.IndexOf(presenter);
    public bool Contains(CubePresenter presenter) => _presenters.Contains(presenter);
    public CubePresenter Last() => _presenters.Last();

    public void LoadPresenters(List<CubePresenter> presenters)
    {
        _presenters = presenters;
    }
}
