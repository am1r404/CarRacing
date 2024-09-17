using CodeBase.Infrastructure.States;
using UnityEngine;
using Zenject;

public class GameBootstrapper : MonoBehaviour
{
    private IGameStateMachine _gameStateMachine;

    [Inject]
    private void Construct(IGameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        _gameStateMachine.Enter<BootstrapState>();
    }
}