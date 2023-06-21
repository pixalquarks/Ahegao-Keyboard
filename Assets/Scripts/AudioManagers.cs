using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class KeyAudio
{
    public string name;
    public AudioClip audioClip;
}

[Serializable]
public class Button
{
    public string name;
    public AudioSource audioSource;
}

[Serializable]
public class Schema
{
    public string schemaName;
    public bool hasDefault;
    public List<KeyAudio> keyAudios = new List<KeyAudio>();
    public Dictionary<string, AudioClip> KeyAudioDict = new Dictionary<string, AudioClip>();
    public List<AudioClip> jingleAudioClips = new List<AudioClip>();
}

public class AudioManagers : MonoBehaviour
{
    public AudioSource singleAudioSource;
    public List<Schema> schemaList = new List<Schema>();
    public Dictionary<string, Schema> SchemaDict = new Dictionary<string, Schema>();
    public List<Button> buttons = new List<Button>();
    public Dictionary<string, AudioSource> Buttons = new Dictionary<string, AudioSource>();


    private void Awake()
    {
        ConfigSchemas();
        SetSchema();
    }

    public void SetSchema()
    {
        var schemaName = DataConfigs.SchemaName;
        try
        {
            if (SchemaDict.ContainsKey(schemaName))
            {
                var currentSchema = SchemaDict[schemaName];
                if (currentSchema.hasDefault)
                {
                    foreach (var button in buttons)
                    {
                        Buttons.Add(button.name, button.audioSource);
                        Buttons[button.name].clip = currentSchema.KeyAudioDict.ContainsKey(button.name)
                            ? currentSchema.KeyAudioDict[button.name]
                            : currentSchema.KeyAudioDict["Default"];
                    }
                }
                else
                {
                    Shuffle(currentSchema.jingleAudioClips);
                    foreach (var button in buttons)
                    {
                        Buttons.Add(button.name, button.audioSource);
                        Buttons[button.name].clip = currentSchema.KeyAudioDict.ContainsKey(button.name)
                            ? currentSchema.KeyAudioDict[button.name]
                            : currentSchema.jingleAudioClips[Random.Range(0, currentSchema.jingleAudioClips.Count)];
                    }
                }
            }
            else
            {
                Debug.Log("No schema Created");
            }
        }
        catch
        {
            if (SchemaDict.ContainsKey(schemaName))
            {
                var currentSchema = SchemaDict[schemaName];
                if (currentSchema.hasDefault)
                {
                    foreach (var button in buttons)
                        Buttons[button.name].clip = currentSchema.KeyAudioDict.ContainsKey(button.name)
                            ? currentSchema.KeyAudioDict[button.name]
                            : currentSchema.KeyAudioDict["Default"];
                }
                else
                {
                    Shuffle(currentSchema.jingleAudioClips);
                    foreach (var button in buttons)
                        Buttons[button.name].clip = currentSchema.KeyAudioDict.ContainsKey(button.name)
                            ? currentSchema.KeyAudioDict[button.name]
                            : currentSchema.jingleAudioClips[Random.Range(0, currentSchema.jingleAudioClips.Count)];
                }
            }
        }
    }

    public void ConfigSchemas()
    {
        foreach (var item in schemaList)
        {
            if (!SchemaDict.ContainsKey(item.schemaName))
            {
                foreach (var i in item.keyAudios) item.KeyAudioDict.Add(i.name, i.audioClip);
                SchemaDict.Add(item.schemaName, item);
            }
            else
            {
                foreach (var i in item.keyAudios) SchemaDict[item.schemaName].KeyAudioDict[i.name] = i.audioClip;
            }
            DataConfigs.SchemaList.Add(item.schemaName);
        }
    }

    public void EnableDisableAudioSource(string state)
    {
        var isEnabled = state == "Multiple Audio";
        foreach (var item in Buttons) item.Value.enabled = isEnabled;

        singleAudioSource.enabled = !isEnabled;
    }

    public void AssignAudioClipToKeys()
    {
    }

    private void Shuffle<T>(List<T> lst)
    {
        for (var i = 0; i < lst.Count; i++)
        {
            var temp = lst[i];
            var randomIndex = Random.Range(i, lst.Count);
            lst[i] = lst[randomIndex];
            lst[randomIndex] = temp;
        }
    }
}