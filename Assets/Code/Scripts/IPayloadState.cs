namespace CodeBase.Infrastructure.States
{
    public interface IPayloadState<TPayload> : IState
    {
        void Enter(TPayload payload);
    }
}