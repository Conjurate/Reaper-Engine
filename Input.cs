using Raylib_cs;
using System.Runtime.CompilerServices;

namespace Reaper;

public static class Input
{
    #region Input Layout

    public static InputLayout Layout
    {
        get => layout;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            layout = value;
        }
    }

    private static InputLayout layout = new InputLayout();

    /// <summary>
    /// Detect if a registered input is being pressed using the global layout.
    /// </summary>
    public static bool IsDown(string name, int gamepad = 0) => IsDown(name, layout, gamepad);

    /// <summary>
    /// Detect if a registered input is being pressed.
    /// </summary>
    public static bool IsDown(string name, InputLayout layout, int gamepad = 0)
    {
        return layout?.GetBindings(name)?.Any(binding =>
            binding.Type switch
            {
                BindingType.Mouse => IsMouseDown(binding.Key),
                BindingType.Keyboard => IsKeyDown(binding.Key),
                BindingType.Gamepad => IsGamepadButtonDown(gamepad, binding.Key),
                _ => false
            }) ?? false;
    }

    /// <summary>
    /// Detect if a registered input is not being pressed using the global layout.
    /// </summary>
    public static bool IsUp(string name, int gamepad = 0) => IsUp(name, layout, gamepad);

    /// <summary>
    /// Detect if a registered input is not being pressed.
    /// </summary>
    public static bool IsUp(string name, InputLayout layout, int gamepad = 0)
    {
        return layout?.GetBindings(name)?.Any(binding =>
            binding.Type switch
            {
                BindingType.Mouse => IsMouseUp(binding.Key),
                BindingType.Keyboard => IsKeyUp(binding.Key),
                BindingType.Gamepad => IsGamepadButtonUp(gamepad, binding.Key),
                _ => false
            }) ?? false;
    }

    /// <summary>
    /// Detect if a registered input has been pressed this frame using the global layout.
    /// </summary>
    public static bool IsPressed(string name, int gamepad = 0) => IsPressed(name, layout, gamepad);

    /// <summary>
    /// Detect if a registered input has been pressed this frame.
    /// </summary>
    public static bool IsPressed(string name, InputLayout layout, int gamepad = 0)
    {
        return layout?.GetBindings(name)?.Any(binding =>
            binding.Type switch
            {
                BindingType.Mouse => IsMousePressed(binding.Key),
                BindingType.Keyboard => IsKeyPressed(binding.Key),
                BindingType.Gamepad => IsGamepadButtonPressed(gamepad, binding.Key),
                _ => false
            }) ?? false;
    }

    /// <summary>
    /// Detect if a registered input has been released this frame using the global layout.
    /// </summary>
    public static bool IsReleased(string name, int gamepad = 0) => IsReleased(name, layout, gamepad);

    /// <summary>
    /// Detect if a registered input has been released this frame.
    /// </summary>
    public static bool IsReleased(string name, InputLayout layout, int gamepad = 0)
    {
        return layout?.GetBindings(name)?.Any(binding =>
            binding.Type switch
            {
                BindingType.Mouse => IsMouseReleased(binding.Key),
                BindingType.Keyboard => IsKeyReleased(binding.Key),
                BindingType.Gamepad => IsGamepadButtonReleased(gamepad, binding.Key),
                _ => false
            }) ?? false;
    }

    #endregion Input Layout

    #region Mouse

    /// <summary>
    /// Detect if a mouse button is being pressed.
    /// </summary>
    public static bool IsMouseDown(InputKey key) => Raylib.IsMouseButtonDown((MouseButton)key);

    /// <summary>
    /// Detect if a mouse button is not being pressed.
    /// </summary>
    public static bool IsMouseUp(InputKey key) => Raylib.IsMouseButtonUp((MouseButton)key);

    /// <summary>
    /// Detect if a mouse button has been pressed this frame.
    /// </summary>
    public static bool IsMousePressed(InputKey key) => Raylib.IsMouseButtonPressed((MouseButton)key);

    /// <summary>
    /// Detect if a mouse button has been released this frame.
    /// </summary>
    public static bool IsMouseReleased(InputKey key) => Raylib.IsMouseButtonReleased((MouseButton)key);

    #endregion

    #region Keyboard

    /// <summary>
    /// Get the id of a key pressed; call multiple times for keys queued; 0 is returned when empty
    /// </summary>
    /// <returns></returns>
    public static int GetKeyPressed() => Raylib.GetKeyPressed();

    /// <summary>
    /// Detect if a key is being pressed.
    /// </summary>
    public static bool IsKeyDown(InputKey key) => Raylib.IsKeyDown((KeyboardKey)key);

    /// <summary>
    /// Detect if a key is not being pressed.
    /// </summary>
    public static bool IsKeyUp(InputKey key) => Raylib.IsKeyUp((KeyboardKey)key);

    /// <summary>
    /// Detect if a key has been pressed this frame.
    /// </summary>
    public static bool IsKeyPressed(InputKey key) => Raylib.IsKeyPressed((KeyboardKey)key);

    /// <summary>
    /// Detect if a key has been released this frame.
    /// </summary>
    public static bool IsKeyReleased(InputKey key) => Raylib.IsKeyReleased((KeyboardKey)key);

    /// <summary>
    /// Get mouse wheel movement for X or Y, whichever is larger.
    /// </summary>
    public static float GetMouseWheelMove() => Raylib.GetMouseWheelMove();

    #endregion Keyboard

    #region Gamepad

    /// <summary>
    /// Detect if a gamepad is available.
    /// </summary>
    public static bool IsGamepadAvailable(int gamepad) => Raylib.IsGamepadAvailable(gamepad);

    /// <summary>
    /// Detect if a gamepad button is being pressed.
    /// </summary>
    public static bool IsGamepadButtonDown(int gamepad, InputKey button) => Raylib.IsGamepadButtonDown(gamepad, (GamepadButton)button);

    /// <summary>
    /// Detect if a gamepad button is not being pressed.
    /// </summary>
    public static bool IsGamepadButtonUp(int gamepad, InputKey button) => Raylib.IsGamepadButtonUp(gamepad, (GamepadButton)button);

    /// <summary>
    /// Detect if a gamepad button has been pressed this frame.
    /// </summary>
    public static bool IsGamepadButtonPressed(int gamepad, InputKey button) => Raylib.IsGamepadButtonPressed(gamepad, (GamepadButton)button);

    /// <summary>
    /// Detect if a gamepad button has been pressed this frame.
    /// </summary>
    public static bool IsGamepadButtonReleased(int gamepad, InputKey button) => Raylib.IsGamepadButtonReleased(gamepad, (GamepadButton)button);

    #endregion Gamepad
}
