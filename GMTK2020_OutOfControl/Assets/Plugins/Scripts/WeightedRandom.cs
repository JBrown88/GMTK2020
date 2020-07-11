using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class WeightedRandom
{
    //=====================================================================================================================//
    //================================================== Internal Classes =================================================//
    //=====================================================================================================================//

    #region Internal Classes

    #endregion

    //=====================================================================================================================//
    //=================================================== Private Fields ==================================================//
    //=====================================================================================================================//

    #region Private Fields

    private readonly float[] _indexWeights;
    private float _totalWeights = 0f;

    private int Count => _indexWeights?.Length ?? 0;

    private readonly HashSet<int> _availableIndices;
    private readonly HashSet<int> _usedIndices;

    #endregion

    //=====================================================================================================================//
    //================================================= Public Properties ==================================================//
    //=====================================================================================================================//

    #region Public Properties

    public float this[int idx]
    {
        get
        {
            if (idx < 0 || idx >= Count)
                return -1f;

            return _indexWeights[idx];
        }
    }

    #endregion

    //=====================================================================================================================//
    //=================================================== Public Methods ==================================================//
    //=====================================================================================================================//

    #region Public Methods

    public WeightedRandom(int numElements)
    {
        _indexWeights = new float[numElements];
        _availableIndices = new HashSet<int>();
        _usedIndices = new HashSet<int>();
    }

    public void Add(int index, float weight)
    {
        if (index < 0 || index >= Count || weight < 0)
            return;

        _totalWeights -= _indexWeights[index];
        _indexWeights[index] = weight;
        _totalWeights += weight;

        _availableIndices.Add(index);
    }

    //Linear scan
    public int Next()
    {
        if (_availableIndices.Count == 0)
            return -1;

        var randomWeight = Random.Range(0, _totalWeights);
        foreach (var idx in _availableIndices)
        {
            randomWeight -= _indexWeights[idx];
            if (randomWeight < 0)
            {
                _totalWeights -= _indexWeights[idx];
                _availableIndices.Remove(idx);
                _usedIndices.Add(idx);
                return idx;
            }
        }

        return -1;
    }

    public void Reset()
    {
        foreach (var index in _usedIndices)
        {
            _availableIndices.Add(index);
        }

        _totalWeights = 0;
        Array.ForEach(_indexWeights, weight => _totalWeights += weight);

        _usedIndices.Clear();
    }

    #endregion
}