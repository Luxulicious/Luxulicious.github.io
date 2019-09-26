using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Keeps track of all available colors
/// </summary>
public class AvailableColors : MonoBehaviour
{
    public static AvailableColors instance;
    [SerializeField]
    private List<ColorSO> _availableColorables = new List<ColorSO>();

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogError("Only one instance of " + this.GetType().ToString() + " is allowed within a scene.");
            Destroy(this.gameObject);
        }
    }

    public List<ColorSO> GetAvailableColors()
    {
        return _availableColorables;
    }

    public ColorSO GetRandomAvailableColor()
    {
        return _availableColorables[Random.Range(0, _availableColorables.Count)];
    }

    public ColorSO GetColorById(string id)
    {
        return _availableColorables.First(x => x.id == id);
    }
}

