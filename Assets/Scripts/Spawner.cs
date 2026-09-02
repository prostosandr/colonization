using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<TItem> : MonoBehaviour where TItem : MonoBehaviour, IPolledObject<TItem>
{
    [SerializeField] private Transform _container;
    [SerializeField] private TItem _prefab;
    [SerializeField] private int _capacity;
    [SerializeField] private int _maxSize;

    private ObjectPool<TItem> _pool;
    private List<TItem> _activeItems;
    private TItem _currentItem;

    private void Awake()
    {
        _pool = new ObjectPool<TItem>(
            createFunc: () => CreateItem(),
            actionOnGet: (item) => ActOnGet(item),
            actionOnRelease: (item) => item.gameObject.SetActive(false),
            actionOnDestroy: (item) => Destroy(item.gameObject),
            collectionCheck: true,
            defaultCapacity: _capacity,
            maxSize: _maxSize);

        _activeItems = new();
    }

    public virtual void Spawn()
    {
        if (_pool.CountActive < _capacity)
        {
            TItem item = _pool.Get();

            item.gameObject.SetActive(true);
            item.transform.position = GetSpawnPoint();
            _currentItem = item;
        }
    }

    public virtual void Spawn(Vector3 position, Bot bot)
    {
        if (_pool.CountActive < _capacity)
        {
            TItem item = _pool.Get();

            item.gameObject.SetActive(true);
            item.transform.position = position;
            _currentItem = item;
        }
    }

    public List<TItem> GetActiveItems()
    {
        return new(_activeItems);
    }

    public TItem GetCurrentItem()
    {
        return _currentItem;
    }

    protected abstract Vector3 GetSpawnPoint();

    private TItem CreateItem()
    {
        var item = Instantiate(_prefab);
        item.transform.parent = _container;

        return item;
    }

    private void ActOnGet(TItem item)
    {
        item.Deactivated += ReleaseItem;

        _activeItems.Add(item);
    }

    private void ReleaseItem(TItem item)
    {
        item.Deactivated -= ReleaseItem;
        item.transform.parent = _container;

        _pool.Release(item);
        _activeItems.Remove(item);
    }
}
