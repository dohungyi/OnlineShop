namespace SharedKernel.Libraries;

public static class DictionaryExtensions
{
    public static void RenameDictionaryKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey fromKey, TKey toKey)
    {
        TValue value = dict[fromKey];
        dict.Remove(fromKey);
        dict[toKey] = value;
    }
}