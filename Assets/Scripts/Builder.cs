using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private BaseSpawner _baseSpawner;
    [SerializeField] private BaseSelector _baseSelector;
    [SerializeField] private BasePreview _preview;
    [SerializeField] private Camera _camera;

    private Ray _ray;
    private Base _selectedBase;

    private List<Base> _basesInWork;


    private void Awake()
    {
        _basesInWork = new();
    }

    private void Start()
    {
        _preview = Instantiate(_preview);
        _preview.Deactivate();
    }

    private void OnEnable()
    {
        _baseSelector.BaseClicked += SetPreview;
        _baseSelector.Clicked += Build;
    }

    private void OnDisable()
    {
        _baseSelector.BaseClicked -= SetPreview;
        _baseSelector.Clicked -= Build;
    }

    private void FixedUpdate()
    {
        if (_preview.IsActive)
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out RaycastHit hit) && hit.transform.TryGetComponent(out Ground _))
            {
                _preview.transform.position = hit.point;

                Vector3 newPosition = _preview.transform.position;
                newPosition.y += _baseSpawner.YOffset;

                _preview.transform.position = newPosition;
            }
        }
    }

    private void SetPreview(Base selectedBase)
    {
        if (selectedBase != null)
        {
            _selectedBase = selectedBase;
            _preview.Activate();
        }
        else
        {
            _preview.Deactivate();
        }
    }

    private void Build()
    {
        if (_preview.IsActive)
        {
            _selectedBase.Preview.transform.position = _preview.transform.position;
            _selectedBase.Preview.Activate();

            bool _isBaseInBases = false;

            if(_basesInWork.Count > 0)
            {
                foreach (Base item in _basesInWork)
                {
                    if (item == _selectedBase)
                    {
                        _isBaseInBases = true;

                        break;
                    }
                }
            }

            if (_isBaseInBases == false)
            {
                _basesInWork.Add(_selectedBase);
                _selectedBase.BuildComleted += DeactivateCurrentPreview;
                _selectedBase.BuildComleted += _baseSpawner.Spawn;
            }

            _selectedBase.ActivateBuild(_selectedBase.Preview.transform.position);
            _selectedBase.Arrow.Deactivate();

            _preview.Deactivate();
        }
    }

    private void DeactivateCurrentPreview(Vector3 basePosition, Bot bot)
    {
        foreach (Base item in _basesInWork)
        {
            if (item.Preview.transform.position == basePosition)
            {
                item.Preview.Deactivate();
                item.BuildComleted -= DeactivateCurrentPreview;
                item.BuildComleted -= _baseSpawner.Spawn;
                _basesInWork.Remove(item);

                break;
            }
        }
    }
}