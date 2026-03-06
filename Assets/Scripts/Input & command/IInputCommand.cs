// 입력 이벤트를 받아 실행되는 공통 규약
public interface IInputCommand
{
    public void Execute();
}
/*
public interface IInputCommand<T>
{
    public void Execute(T value);
}

public class ValueCommand<T> : IInputCommand<T>
{
    private readonly Action<T> _action;

    public ValueCommand(Action<T> action)
    {
        _action = action;
    }

    public void (T value)
    {
        _action?.Invoke(value);
    }
}
*/
