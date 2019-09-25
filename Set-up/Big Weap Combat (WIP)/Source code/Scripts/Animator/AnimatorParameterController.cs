using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class AnimatorFloatParameter
{
    [SerializeField, Disable, AnimatorParameter(AnimatorParameterAttribute.ParameterType.Float)]
    public string name;
    [SerializeField, Disable]
    private int _hash;
    public float value;

    public void CalculateHash()
    {
        _hash = Animator.StringToHash(name);
    }
}

[Serializable]
public class AnimatorIntParameter
{
    [SerializeField, Disable, AnimatorParameter(AnimatorParameterAttribute.ParameterType.Int)]
    public string name;
    [SerializeField, Disable]
    private int _hash;
    public int value;

    public void CalculateHash()
    {
        _hash = Animator.StringToHash(name);
    }
}

[Serializable]
public class AnimatorBoolParameter
{
    [SerializeField, Disable, AnimatorParameter(AnimatorParameterAttribute.ParameterType.Bool)]
    public string name;
    [SerializeField, Disable]
    private int _hash;
    public bool value;

    public void CalculateHash()
    {
        _hash = Animator.StringToHash(name);
    }
}

[Serializable]
public class AnimatorTriggerParameter 
{
    [SerializeField, Disable, AnimatorParameter(AnimatorParameterAttribute.ParameterType.Trigger)]
    internal string name;
    [SerializeField, Disable]
    private int _hash;
    internal Animator animator;

    public void CalculateHash()
    {
        _hash = Animator.StringToHash(name);
    }

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        animator.SetTrigger(name);
    }
}

[ExecuteAlways]
public class AnimatorParameterController : MonoBehaviour
{
    //TODO Refactor with collection uses with delegates
    [Serializable]
    public class AnimatorParameters
    {
        [SerializeField]
        internal List<AnimatorFloatParameter> floatParameters = new List<AnimatorFloatParameter>();
        [SerializeField]
        internal List<AnimatorIntParameter> intParameters = new List<AnimatorIntParameter>();
        [SerializeField]
        internal List<AnimatorBoolParameter> boolParameters = new List<AnimatorBoolParameter>();
        [SerializeField]
        internal List<AnimatorTriggerParameter> triggerParameters = new List<AnimatorTriggerParameter>();
    }

    [SerializeField]
    private Animator _animator;
    public bool getParamNamesAndHashInPlay;
    [SerializeField]
    private AnimatorParameters _animatorParameters = new AnimatorParameters();


    void Awake()
    {
        Debug.LogWarning("Warning the " + this.GetType() + " is still a WIP (if you are developer you can ignore this message)");
        _animator = this.GetComponent<Animator>();
        GetMissingParams();
    }

    [ContextMenu("Get Missing Parameters")]
    public void GetMissingParams()
    {
        if (!_animator)
            _animator = this.GetComponent<Animator>();

        _animator.parameters.ToList().ForEach(x =>
        {
            switch (x.type)
            {
                case AnimatorControllerParameterType.Bool:
                {
                    if (_animatorParameters.boolParameters.All(y => x.name != y.name))
                    {
                        _animatorParameters.boolParameters.Add(new AnimatorBoolParameter() {name = x.name, value = _animator.GetBool(x.name) });
                        Debug.Log("Added param: " + x.name);
                    }

                    break;
                }
                case AnimatorControllerParameterType.Float:
                {
                    if (_animatorParameters.floatParameters.All(y => x.name != y.name))
                    {
                        _animatorParameters.floatParameters.Add(new AnimatorFloatParameter() {name = x.name, value = _animator.GetFloat(x.name) });
                        Debug.Log("Added param: " + x.name);
                    }

                    break;
                }
                case AnimatorControllerParameterType.Int:
                {
                    if (_animatorParameters.intParameters.All(y => x.name != y.name))
                    {
                        _animatorParameters.intParameters.Add(new AnimatorIntParameter() {name = x.name, value = _animator.GetInteger(x.name) });
                        Debug.Log("Added param: " + x.name);
                    }

                    break;
                }
                case AnimatorControllerParameterType.Trigger:
                {
                    if (_animatorParameters.triggerParameters.All(y => x.name != y.name))
                    {
                        _animatorParameters.triggerParameters.Add(new AnimatorTriggerParameter() {name = x.name, animator = _animator});
                        Debug.Log("Added param: " + x.name);
                    }
                    break;
                }
            }
        });
        CalculateHash();
    }

    [ContextMenu("Clear Parameters")]
    public void ClearParams()
    {
        _animatorParameters.boolParameters.Clear();
        _animatorParameters.floatParameters.Clear();
        _animatorParameters.intParameters.Clear();
        _animatorParameters.triggerParameters.Clear();
        Debug.Log("Cleared all params!");
    }

    [ContextMenu("(Re-)Calculate Hash")]
    public void CalculateHash()
    {
        _animatorParameters.boolParameters.ForEach(x => x.CalculateHash());
        _animatorParameters.floatParameters.ForEach(x => x.CalculateHash());
        _animatorParameters.intParameters.ForEach(x => x.CalculateHash());
        _animatorParameters.triggerParameters.ForEach(x => x.CalculateHash());
    }
}