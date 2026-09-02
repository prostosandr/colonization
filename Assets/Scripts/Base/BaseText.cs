using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class BaseText : MonoBehaviour
{
    [SerializeField] private Base _base;

    private TextMeshPro _text;
    private Camera _camera;

    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        _base.BaseTextChanged += ChangeText;
        _base.CameraChanged += SetCamera;
    }

    private void OnDisable()
    {
        _base.BaseTextChanged -= ChangeText;
        _base.CameraChanged -= SetCamera;
    }

    private void Update()
    {
        if (_camera != null)
        {
            transform.rotation = _camera.transform.rotation;
        }
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    private void ChangeText(int crystalValue, int botValue)
    {
        _text.text = $"Кристаллы: {crystalValue}\n" +
            $"Ботов: {botValue}";
    }
}
