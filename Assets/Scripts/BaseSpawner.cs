using UnityEngine;

public class BaseSpawner : Spawner<Base>
{
    [SerializeField] private Storage _storage;
    [SerializeField] private Camera _camera;
    [SerializeField] private Ground _ground;
    [SerializeField] private BaseSelector _selector;
    [SerializeField] private float _yOffset;
    [SerializeField] private int _numberOfBotsStartBase;

    private Base _selectedBase;

    public float YOffset => _yOffset;

    private void Start()
    {
        Spawn();
        GetCurrentItem().SpawnFirstBots(_numberOfBotsStartBase);
    }

    private void OnEnable()
    {
        _selector.BaseClicked += Select;
    }

    public override void Spawn()
    {
        base.Spawn();

        Base currentBase = GetCurrentItem();
        currentBase.SetStorage(_storage);
        currentBase.SetCamera(_camera);
    }

    public override void Spawn(Vector3 position, Bot bot)
    {
        base.Spawn(position, bot);

        Base currentBase = GetCurrentItem();
        currentBase.SetStorage(_storage);
        currentBase.SetCamera(_camera);
        currentBase.AddBot(bot);
        
        Select(null);
    }

    protected override Vector3 GetSpawnPoint()
    {
        return new Vector3(
            _ground.Bounds.center.x,
            _ground.Bounds.center.y + _yOffset,
            _ground.Bounds.center.z);
    }

    private void Select(Base selectedBase)
    {
        if (selectedBase == null && _selectedBase != null)
        {
            _selectedBase.GetArrow().Deactivate();
            _selectedBase = null;
        }
        else if (selectedBase != null)
        {
            _selectedBase = selectedBase;
            _selectedBase.GetArrow().Activate();
        }
    }
}