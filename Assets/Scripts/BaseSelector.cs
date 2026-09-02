using System;
using UnityEngine;

public class BaseSelector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private int _minBotsInBase;

    private PlayerInput _playerInput;

    private Ray _ray;

    public event Action<Base> BaseClicked;
    public event Action Clicked;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Builder.Select.performed += ctx => Select();
        _playerInput.Builder.Deselect.performed += ctx => Deselect();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Builder.Select.performed -= ctx => Select();
        _playerInput.Builder.Deselect.performed -= ctx => Deselect();
    }

    private void Select()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out RaycastHit hit) && hit.transform.TryGetComponent(out Base currentBase))
        {
            if (currentBase.NumberOfBots > _minBotsInBase)
            {
                BaseClicked?.Invoke(currentBase);
            }
        }
        else
        {
            Clicked?.Invoke();
        }
    }

    private void Deselect()
    {
        BaseClicked?.Invoke(null);
    }
}