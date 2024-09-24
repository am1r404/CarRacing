using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CodeBase.Services
{
    public class CarPositionManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> _carPositions = new();
        private int _currentFriendCarIndex = 1;

        [Inject]
        private void Construct()
        {
            // Initialization if needed
        }

        private void Awake()
        {
            LoadPositions();
        }

        private void LoadPositions()
        {
            Transform carPositionsParent = transform;
            for (int i = 0; i < carPositionsParent.childCount; i++)
            {
                _carPositions.Add(carPositionsParent.GetChild(i));
            }
        }

        public Transform GetPlayerCarPosition()
        {
            return _carPositions.Count > 0 ? _carPositions[0] : null;
        }

        public Transform GetNextFriendCarPosition()
        {
            if (_currentFriendCarIndex < _carPositions.Count)
            {
                return _carPositions[_currentFriendCarIndex++];
            }

            return null;
        }

        public void ResetFriendCarIndex()
        {
            _currentFriendCarIndex = 1;
        }

        public int RemainingPositions => _carPositions.Count - _currentFriendCarIndex;
    }
}