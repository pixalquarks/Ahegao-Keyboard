using TMPro;
using UnityEngine;
using UnityRawInput;

public class InputManager : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private TMP_Dropdown audioType;
    [SerializeField] private TMP_Dropdown schema;

    private bool _isPaused;

    public AudioManagers audioManagers;

    private readonly bool _workInBackground = true;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        RawKeyInput.Start(_workInBackground);
        SetValuesForDropDowns();
    }

    private void Start()
    {
        schema.AddOptions(DataConfigs.SchemaList);
    }

    private void SetValuesForDropDowns()
    {
        audioType.value = audioType.options.FindIndex(option => option.text == DataConfigs.AudioType);
        schema.value = schema.options.FindIndex(option => option.text == DataConfigs.SchemaName);
    }

    private void OnEnable()
    {
        RawKeyInput.OnKeyDown += HandleKeyDown;
    }

    private void HandleKeyDown(RawKey key)
    {
        if (key == RawKey.F4 && RawKeyInput.IsKeyDown(RawKey.LeftControl) || RawKeyInput.IsKeyDown(RawKey.RightControl))
            _isPaused = !_isPaused;
        if (SaveLoadAudio.BuildingNewSchema)
        {
            if (!_isPaused) _isPaused = !_isPaused;
            SaveLoadAudio.OpenFileExplorer();
            return;
        }
        if (_isPaused) return;
        var nameOfKey = key.ToString();
        var isMultipleAudioEnabled = DataConfigs.AudioType == "Multiple Audio";
        if (isMultipleAudioEnabled && audioManagers.Buttons.ContainsKey(nameOfKey))
        {
            audioManagers.Buttons[nameOfKey].Play();
        }
        else if (!isMultipleAudioEnabled && audioManagers.Buttons.ContainsKey(nameOfKey))
        {
            _audioSource.clip = audioManagers.Buttons[nameOfKey].clip;
            _audioSource.Play();
        }
    }

    public void SetAudio(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
    }

    private void OnDisable()
    {
        RawKeyInput.OnKeyDown -= HandleKeyDown;
    }

    public void OpenMenu()
    {
        mainCanvas.enabled = false;
        menuCanvas.enabled = true;
    }

    public void OpenMainScreen()
    {
        menuCanvas.enabled = false;
        mainCanvas.enabled = true;
    }

    public void Save()
    {
        var audioValue = audioType.value;
        var schemaValue = schema.value;
        DataConfigs.AudioType = audioType.options[audioValue].text;
        DataConfigs.SchemaName = schema.options[schemaValue].text;
        audioManagers.SetSchema();
        audioManagers.EnableDisableAudioSource(DataConfigs.AudioType);
        OpenMainScreen();
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        RawKeyInput.Stop();
    }
}