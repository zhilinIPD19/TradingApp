namespace TradingApi.Data
{
    public static class IdempotencyStore
    {
        private static readonly Dictionary<string, object> KeyValuePairs = new Dictionary<string, object>();
        public static bool TryGet(string key, out object? value)
        {
            return KeyValuePairs.TryGetValue(key, out value);
        }

        public static void Add(string key, object value)
        {
            KeyValuePairs[key] = value;
        }
    }
}
