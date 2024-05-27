namespace Reaper;

public class InputLayout()
{
    private Dictionary<string, List<IBinding>> layout = [];

    public List<IBinding> GetBindings(string name) => layout.GetValueOrDefault(name, null);

    public void AddBinding(string name, params IBinding[] bindingsToAdd)
    {
        if (!layout.TryGetValue(name, out List<IBinding> bindings))
            bindings = [];

        foreach (IBinding binding in bindingsToAdd)
            bindings.Add(binding);

        layout[name] = bindings;
    }

    public bool Contains(string name) => layout.ContainsKey(name);
}

public interface IBinding
{
    BindingType Type { get; }
    InputKey Key { get; }
}

public struct MouseBinding(InputKey key) : IBinding
{
    public BindingType Type => BindingType.Mouse;
    public InputKey Key => key;
}

public struct KeyboardBinding(InputKey key) : IBinding
{
    public BindingType Type => BindingType.Keyboard;
    public InputKey Key => key;
}

public struct GamepadBinding(InputKey key) : IBinding
{
    public BindingType Type => BindingType.Gamepad;
    public InputKey Key => key;
}

public enum BindingType
{
    Mouse,
    Keyboard,
    Gamepad
}
