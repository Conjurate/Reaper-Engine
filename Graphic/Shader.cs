using Raylib_cs;

namespace Reaper;

public struct Shader
{
    internal Raylib_cs.Shader raylibShader;

    internal Shader(Raylib_cs.Shader shader)
    {
        raylibShader = shader;
    }

    /// <summary>
    /// Get uniform location
    /// </summary>
    public int GetLocation(string name)
    {
        return Raylib.GetShaderLocation(raylibShader, name);
    }

    /// <summary>
    /// Set uniform value
    /// </summary>
    public void SetValue<T>(int locIndex, T value, UniformDataType type) where T : unmanaged
    {
        Raylib.SetShaderValue<T>(raylibShader, locIndex, value, (ShaderUniformDataType)type);
    }

    /// <summary>
    /// Set uniform value for texture
    /// </summary>
    public void SetValueTexture(int locIndex, Texture2D texture)
    {
        Raylib.SetShaderValueTexture(raylibShader, locIndex, texture);
    }
}

public enum UniformDataType
{
    Float = 0,
    Vec2,
    Vec3,
    Vec4,
    Int,
    IVec2,
    IVec3,
    IVec4,
    Sampler2D
}