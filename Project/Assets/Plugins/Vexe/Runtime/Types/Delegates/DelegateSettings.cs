namespace Assets.Plugins.Vexe.Runtime.Types.Delegates
{
    public static class DelegateSettings
    {
        /// <summary>
        /// Any method name defined in this array will be ignored in the delegates methods popup
        /// </summary>
        public static string[] IgnoredMethods =
        {
            "CancelInvoke",
            "StopAllCoroutines",
        };
    }
}