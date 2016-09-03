namespace Dickson.Library.Security.Principal
{
    /// <summary>
    /// 描述一个用户身份。
    /// </summary>
    /// <typeparam name="TKey">用户Id的类型。</typeparam>
    public interface IUser<out TKey>
    {
        /// <summary>
        /// 用户标识。
        /// </summary>
        TKey Id { get; }

        string UserName { get; }
    }

    /// <summary>
    /// 描述一个用户身份。
    /// </summary>
    public interface IUser : IUser<string>
    {
    }

}