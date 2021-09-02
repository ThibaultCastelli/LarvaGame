
public interface IState
{
    public void OnEnter();
    public void Tick();
    public void FixedTick();
    public void OnExit();
}
