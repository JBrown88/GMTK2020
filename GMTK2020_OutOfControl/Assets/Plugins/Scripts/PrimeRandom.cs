using UnityEngine;
using System.Collections;

namespace GameSystems.Utils
{
    public class PrimeRandom
    {
        //=====================================================================================================================//
        //=================================================== Private Fields ==================================================//
        //=====================================================================================================================//

        #region Private Fields

        private static readonly int[] PrimeArray = {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97,
            101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197,
            199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313,
            317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439,
            443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571,
            577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691,
            701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829,
            839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977,
            983, 991, 997, 1009, 1013, 1019, 1021, 1031, 1033, 1039, 1049, 1051, 1061, 1063, 1069, 1087, 1091
        };

        private int _skip;
        private int _currentPosition;
        private int _maxElements;
        private int _currentPrime;
        private int _numSearches;
        private bool _autoRestart;

        #endregion

        //=====================================================================================================================//
        //=================================================== Public Methods ==================================================//
        //=====================================================================================================================//

        #region Public Methods

        public int Capacity
        {
            get { return _maxElements; }
        }

        public int Count
        {
            get { return _maxElements - _numSearches; }
        }

        public PrimeRandom(int numElements, bool autoRestart = false)
        {
            if (numElements > PrimeArray[PrimeArray.Length - 1])
            {
                Debug.Log(string.Format("Current array size ({0}) is too large. Capping number of elements to {1}.", numElements, PrimeArray[PrimeArray.Length - 1] - 1));
                numElements = PrimeArray[PrimeArray.Length - 1] - 1;
            }

            var a = (int)(Random.value * 10000);
            var b = (int)(Random.value * 10000);
            var c = (int)(Random.value * 10000);

            _maxElements = numElements;
            
            _skip = a * (numElements * numElements) + b * numElements + c;

            foreach (var prime in PrimeArray)
            {
                _currentPrime = prime;
                if (_currentPrime > _maxElements)
                {
                    break;
                }
            }

            this._autoRestart = autoRestart;
            _currentPosition = 0;
        }


        public int Next(bool restart = false)
        {
            if (restart || _autoRestart)
                Restart();

            if (Done())
                return -1;

            do
            {
                _currentPosition += _skip;
                _currentPosition %= _currentPrime;
            } while (_currentPosition >= _maxElements && _currentPosition < _currentPrime);

            _numSearches++;

            return _currentPosition;
        }

        public bool Done()
        {
            return _numSearches == _maxElements;
        }

        public void Restart(int numElements)
        {
            if (numElements > PrimeArray[PrimeArray.Length - 1])
            {                
                numElements = PrimeArray[PrimeArray.Length - 1] - 1;
            }

            var a = (int)(Random.value * 10000);
            var b = (int)(Random.value * 10000);
            var c = (int)(Random.value * 10000);

            _maxElements = numElements;
            _skip = a * (numElements * numElements) + b * numElements + c;

            foreach (var prime in PrimeArray)
            {
                _currentPrime = prime;
                if (_currentPrime > _maxElements)
                {                    
                    break;
                }
            }

            _currentPosition = 0;
            _numSearches = 0;
        }

        public void Restart()
        {
            var a = (int)(Random.value * 10000);
            var b = (int)(Random.value * 10000);
            var c = (int)(Random.value * 10000);

            _skip = a * (_maxElements * _maxElements) + b * _maxElements + c;

            _currentPosition = 0;
            _numSearches = 0;
        }

        #endregion
    }
}