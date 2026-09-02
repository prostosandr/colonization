using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
[RequireComponent(typeof(BotSpawner))]
public class Base : MonoBehaviour, IPolledObject<Base>
{
    [SerializeField] private Arrow _arrow;
    [SerializeField] private BasePreview _preview;
    [SerializeField] private float _scanTime;
    [SerializeField] private int _botPrice;
    [SerializeField] private int _buildPrice;

    private Camera _camera;
    private Storage _storage;
    private Scanner _scanner;
    private BotSpawner _spawner;

    private bool _isBuild;
    private Vector3 _buildPosition;

    private List<Bot> _bots;
    private Queue<Crystal> _crystals;

    public Arrow Arrow => _arrow;
    public BasePreview Preview => _preview;
    public int NumberOfBots => _bots.Count;
    public bool IsBuild => _isBuild;

    public event Action<Base> Deactivated;
    public event Action<int, int> BaseTextChanged;
    public event Action<Camera> CameraChanged;
    public event Action<Vector3, Bot> BuildComleted;

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
        _spawner = GetComponent<BotSpawner>();

        _crystals = new();
        _bots = new();
    }

    private void Start()
    {
        CameraChanged?.Invoke(_camera);
        
        var wait = new WaitForSeconds(_scanTime);

        StartCoroutine(Scan(wait));

        BaseTextChanged?.Invoke(_crystals.Count, _bots.Count);
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    public void SetStorage(Storage storage)
    {
        _storage = storage;
    }

    public Arrow GetArrow()
    {
        return _arrow;
    }

    public void ActivateBuild(Vector3 buildPosition)
    {
        _isBuild = true;
        _buildPosition = buildPosition;
    }

    public void SpawnFirstBots(int numberOfBots)
    {
        for (int i = 0; i < numberOfBots; i++)
        {
            _spawner.Spawn();

            _bots.Add(_spawner.GetCurrentItem());
        }
    }

    public void AddBot(Bot bot)
    {
        _bots.Add(bot);
    }

    public void AddNumberOfCrystals(Bot bot, Crystal crystal)
    {
        _crystals.Enqueue(crystal);

        bot.Worked -= AddNumberOfCrystals;

        if (_isBuild == false)
        {
            if (_crystals.Count >= _botPrice)
            {
                for (int i = 0; i < _botPrice; i++)
                {
                    _crystals.Dequeue();
                }

                _spawner.Spawn();

                _bots.Add(_spawner.GetCurrentItem());
            }
        }

        BaseTextChanged?.Invoke(_crystals.Count, _bots.Count);
    }

    private IEnumerator Scan(WaitForSeconds wait)
    {
        Bot freeBot;
        Crystal freeCrystal;

        while (enabled)
        {
            _storage.SetFoundCrystals(_scanner.GetScanObjects());
            freeBot = GetFreeBot();
            freeCrystal = _storage.GetFreeCrystal();

            if (_isBuild && _crystals.Count >= _buildPrice && freeBot != null)
            {
                _bots.Remove(freeBot);

                SendToBuild(freeBot, _buildPosition);
            }
            else
            {
                if (freeBot != null && freeCrystal != null)
                    SendToWork(freeBot, freeCrystal);
            }

            yield return wait;
        }
    }

    private void SendToBuild(Bot freeBot, Vector3 buildPosition)
    {
        for (int i = 0; i < _buildPrice; i++)
        {
            _crystals.Dequeue();
        }

        BaseTextChanged?.Invoke(_crystals.Count, _bots.Count);

        _isBuild = false;

        freeBot.SetBuild(buildPosition);
        freeBot.BuildComleted += InvokeBuildCompleted;
    }

    private void InvokeBuildCompleted(Vector3 buildPosition, Bot bot)
    {
        _isBuild = false;

        BuildComleted?.Invoke(buildPosition, bot);

        bot.BuildComleted -= InvokeBuildCompleted;
    }

    private void SendToWork(Bot freeBot, Crystal freeCrystal)
    {
        freeBot.Worked += AddNumberOfCrystals;
        freeBot.Worked += _storage.RemoveOccupiedCrystal;
        freeBot.SetWork(freeCrystal, transform.position);
    }

    private Bot GetFreeBot()
    {
        Bot bot = null;

        foreach (Bot currentBot in _bots)
        {
            if (currentBot.IsWork == false)
            {
                bot = currentBot;

                break;
            }
        }

        return bot;
    }
}