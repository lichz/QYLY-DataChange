namespace RoadFlow.Data.Model
{
    /// <summary>
    /// 执行过程结果信息类。
    /// </summary>
    public class Result
    {
        /// 初始化结果信息。
        /// </summary>
        public Result()
            : this(string.Empty)
        { }

        /// <summary>
        /// 以指定的错误信息初始化结果信息。
        /// </summary>
        /// <param name="err_msg">指定过程错误信息。</param>
        public Result(string err_msg)
        {
            this.ErrMSG = err_msg;
            this.Success = false;
        }

        /// <summary>
        /// 获取或设置执行过程中的错误信息。
        /// </summary>
        public string ErrMSG { get; set; }

        /// <summary>
        /// 获取或设置执行过程是否完成。
        /// </summary>
        public bool Success { get; set; }
    }

    /// <summary>
    /// 带返回值的结果信息类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// 初始化带返回值的结果信息。
        /// </summary>
        public Result()
            : this(string.Empty)
        { }

        /// <summary>
        /// 以指定的错误信息初始化带返回值的结果信息。
        /// </summary>
        /// <param name="err_msg">指定过程错误信息。</param>
        public Result(string err_msg)
            : base(err_msg)
        {
            this.Data = default(T);
        }

        /// <summary>
        /// 获取或设置执行过程的返回值。
        /// </summary>
        public T Data { get; set; }
    }

}
