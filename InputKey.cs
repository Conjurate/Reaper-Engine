namespace Reaper;

public enum InputKey
{
    // Mouse buttons
    Mouse0 = 0, // Left
    Mouse1 = 1, // Right
    Mouse2 = 2, // Middle / third
    Mouse3 = 3, // Additional
    Mouse4 = 4, // Additional
    Mouse5 = 5, // Additional
    Mouse6 = 6, // Additional

    // Gamepad Axis
    GamepadLeftX = 0, // Left stick X axis
    GamepadLeftY = 1, // Left stick y axis
    GamepadRightX = 2, // Right stick X axis
    GamepadRightY = 3, // Right stick Y axis
    GamepadLeftTrigger = 4, // pressure level: [1..-1]
    GamepadRightTrigger = 5, // pressure level: [1..-1]

    // Gamepad Button
    GamepadLeftFaceUp = 1, // Left DPAD up
    GamepadLeftFaceRight = 2, // Left DPAD right
    GamepadLeftFaceDown = 3, // Left DPAD down
    GamepadLeftFaceLeft = 4, // Left DPAD left
    GamepadRightFaceUp = 5, // Right button up (i.e. PS3: Triangle, Xbox: Y)
    GamepadRightFaceRight = 6, // Right button right (i.e. PS3: Square, Xbox: X)
    GamepadRightFaceDown = 7, // Right button down (i.e. PS3: Cross, Xbox: A)
    GamepadRightFaceLeft = 8, // Right button left (i.e. PS3: Circle, Xbox: B)
    GamepadLeftTrigger1 = 9, // Top/back trigger left (first), it could be a trailing button
    GamepadLeftTrigger2 = 10, // top/back trigger left (second), it could be a trailing button
    GamepadRightTrigger1 = 11, // top/back trigger right (first), it could be a trailing button
    GamepadRightTrigger2 = 12, // top/back trigger right (second), it could be a trailing button
    GamepadMiddleLeft = 13, // Center buttons, left one (i.e. PS3: Select)
    GamepadMiddle = 14, // Center buttons, middle one (i.e. PS3: PS, Xbox: XBOX)
    GamepadMiddleRight = 15, // Center buttons, right one (i.e. PS3: Start)
    GamepadLeftThumb = 16, // Joystick pressed button left
    GamepadRightThumb = 17, // Joystick pressed button right

    // Keyboard
    // Alphanumeric keys
    Apostrophe = 39,
    Comma = 44,
    Minus = 45,
    Period = 46,
    Slash = 47,
    Zero = 48,
    One = 49,
    Two = 50,
    Three = 51,
    Four = 52,
    Five = 53,
    Six = 54,
    Seven = 55,
    Eight = 56,
    Nine = 57,
    Semicolon = 59,
    Equal = 61,
    A = 65,
    B = 66,
    C = 67,
    D = 68,
    E = 69,
    F = 70,
    G = 71,
    H = 72,
    I = 73,
    J = 74,
    K = 75,
    L = 76,
    M = 77,
    N = 78,
    O = 79,
    P = 80,
    Q = 81,
    R = 82,
    S = 83,
    T = 84,
    U = 85,
    V = 86,
    W = 87,
    X = 88,
    Y = 89,
    Z = 90,

    // Function keys
    Space = 32,
    Escape = 256,
    Enter = 257,
    Tab = 258,
    Backspace = 259,
    Insert = 260,
    Delete = 261,
    Right = 262,
    Left = 263,
    Down = 264,
    Up = 265,
    PageUp = 266,
    PageDown = 267,
    Home = 268,
    End = 269,
    CapsLock = 280,
    ScrollLock = 281,
    NumLock = 282,
    PrintScreen = 283,
    Pause = 284,
    F1 = 290,
    F2 = 291,
    F3 = 292,
    F4 = 293,
    F5 = 294,
    F6 = 295,
    F7 = 296,
    F8 = 297,
    F9 = 298,
    F10 = 299,
    F11 = 300,
    F12 = 301,
    LeftShift = 340,
    LeftControl = 341,
    LeftAlt = 342,
    LeftSuper = 343,
    RightShift = 344,
    RightControl = 345,
    RightAlt = 346,
    RightSuper = 347,
    KeyboardMenu = 348,
    LeftBracket = 91,
    Backslash = 92,
    RightBracket = 93,
    Grave = 96,

    // Keypad keys
    Keypad0 = 320,
    Keypad1 = 321,
    Keypad2 = 322,
    Keypad3 = 323,
    Keypad4 = 324,
    Keypad5 = 325,
    Keypad6 = 326,
    Keypad7 = 327,
    Keypad8 = 328,
    Keypad9 = 329,
    KeypadDecimal = 330,
    KeypadDivide = 331,
    KeypadMultiply = 332,
    KeypadSubtract = 333,
    KeypadAdd = 334,
    KeypadEnter = 335,
    KeypadEqual = 336,

    // Android key buttons
    Back = 4,
    Menu = 82,
    VolumeUp = 24,
    VolumeDown = 25
}
