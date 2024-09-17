using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LobbyState : IState
    {
        public void Enter()
        {
            Debug.Log("Entering LobbyState");
            // Initialize lobby-specific logic here
        }

        public void Exit()
        {
            Debug.Log("Exiting LobbyState");
        }
    }
}