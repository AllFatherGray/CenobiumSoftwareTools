namespace Cenobium
{
    //// <summary>
    /// Generic Wrapper for objects  - Allows null-default param arguments for structs
    /// It is similar to the null-able structs you get in C# but with 
    /// that added bonus of being a class and having an implicit cast to a delegate that also returns the wrapped type.
    /// </summary>
    public class vFuncWrapper<T>
    {
        /// <summary>
        /// The delegate that contains the stored object
        /// </summary>
        public delegate T vFunc();
        /// <summary>
        /// Instance of vFunc shell to be used by an instance of the class
        /// </summary>
        vFunc _shell = null;
        /// <summary>
        /// Default constructor. Wraps the object passed to method. Here , _shell is assigned a delegate that returns the parameter arg passed to the constructor
        /// </summary>
        /// <param name="_candy"></param>
        public vFuncWrapper(T _candy = default(T)) { _shell = new vFunc(delegate () { return _candy; }); }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="vfw"></param>
        public vFuncWrapper(vFuncWrapper<T> vfw) : this(vfw._shell()) { }

        /// <summary>
        /// The _shell is wrapped in a new vFuncWrapper object - similar to default constructor
        /// </summary>
        /// <param name="_shell"></param>
        /// <returns></returns>
        public static implicit operator vFuncWrapper<T>(T _shell)
        {
            return new vFuncWrapper<T>(_shell);
        }
        
        /// <summary>
        /// Wrapper returns the vFunc delegate
        /// </summary>
        /// <param name="vfw"></param>
        /// <returns></returns>
        public static implicit operator vFunc(vFuncWrapper<T> vfw)
        {
            return vfw ? vfw._shell : null;
        }
        /// <summary>
        /// Converts a delegate into a vFuncWrapper
        /// </summary>
        /// <param name="vf"></param>
        /// <returns></returns>
        public static implicit operator vFuncWrapper<T>(vFunc vf)
        {
            return vf != null ? new vFuncWrapper<T>(vf) : null;
        }
        /// <summary>
        /// Returns true if object isn't null
        /// </summary>
        /// <param name="vfw"></param>
        /// <returns></returns>
        public static implicit operator bool(vFuncWrapper<T> vfw)
        {
            return vfw != null && vfw._shell != null;
        }
        /// <summary>
        /// Returns the wrapped object - similar to GetValueOrDefault
        /// </summary>
        /// <param name="vfw"></param>
        /// <returns></returns>
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
        public static vFuncWrapper<T> vCreate<T>(T candy)
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
}