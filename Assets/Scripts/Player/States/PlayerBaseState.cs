namespace Player.States
{
    public abstract class PlayerBaseState
    {
        public abstract void EnterState(PlayerManager player);
        public abstract void UpdateState(PlayerManager player);
    }
}