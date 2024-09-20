namespace WUM.Lib.Models.Common
{
    public class CommAPIModel<T>
    {
        /// <summary>
        /// api狀態
        /// </summary>
        public bool Success { get; set; } = true;
        /// <summary>
        /// 說明內容
        /// </summary>
        public string Msg { get; set; } = "";

        public T Data { get; set; }
    }
}
