using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

//TODO Make this an instance pattern rather than static list
public class AvailableColors : MonoBehaviour
{
    public static AvailableColors instance;
    public List<ColorSO> availableColorables = new List<ColorSO>();

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

    public ColorSO GetRandomAvailableColor()
    {
        return availableColorables[Random.Range(0, availableColorables.Count)];
    }

    public ColorSO GetColorById(int id)
    {
        return availableColorables.First(x => x.collisionId == id);
    }
}

