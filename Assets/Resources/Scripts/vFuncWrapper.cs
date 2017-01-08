
//// <summary>
/// Delegate Wrapper for T  where T is the Nougat-y goodness - Allows null-default param arguments for structs
/// It is similar to the null-able structs you get in C# but with 
/// that added bonus of being a class and having an implicit cast to a delegate that also return the wrapped type.
/// </summary>
public class vFuncWrapper<T>
{
    /// <summary>
    /// The shell that contains the stored T - yum
    /// </summary>
    public delegate T vFunc();
    /// <summary>
    /// Instance of candy shell
    /// </summary>
    vFunc _shell = null;
    public vFuncWrapper(T _candy = default(T)) { _shell = new vFunc(delegate () { return _candy; }); }
    public vFuncWrapper(vFuncWrapper<T> vfw) : this(vfw._shell()) { }
    public static implicit operator vFuncWrapper<T>(T _shell)
    {
        return new vFuncWrapper<T>(_shell);
    }
    public static implicit operator vFunc(vFuncWrapper<T> vfw)
    {
        return vfw ? vfw._shell : null;
    }
    public static implicit operator vFuncWrapper<T>(vFunc vf)
    {
        return vf != null ? new vFuncWrapper<T>(vf) : null;
    }
    public static implicit operator bool(vFuncWrapper<T> vfw)
    {
        return (vfw as object) != null && vfw._shell != null;
    }
    public static implicit operator T(vFuncWrapper<T> vfw)
    {
        try
        {
            return vfw._shell();
        }
        catch
        {
            return default(T);
        }
    }

}
/// <summary>
/// Extension Class for vFuncWrapper. Allows Objects to Wrap themselves
/// </summary>
public static class vFuncWrapper
{
    /// <summary>
    /// Wraps any object / primitive into a delagate based wrapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="candy">The object to wrap</param>
    /// <returns></returns>
    public static vFuncWrapper<T> Create<T>(T candy)
    {
        return new vFuncWrapper<T>(candy);
    }
    /// <summary>
    /// Extension Method that Wraps any object / primitive into a delagate based wrapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="candy">The object to wrap</param>
    /// <returns></returns>
    public static vFuncWrapper<T> vWrap<T>(this T candy)
    {
        return new vFuncWrapper<T>(candy);
    }
}
