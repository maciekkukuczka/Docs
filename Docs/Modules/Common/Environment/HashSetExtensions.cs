namespace Mac.Modules.Common;

public static class HashSetExtensions
{
    public static async Task<HashSet<T>> ToHashSetByEnvironment<T>(this IQueryable<T> queryable)
    {
#if NET9
        return await queryable.ToHashSetAsync();
#endif
        return new HashSet<T>(queryable);
    }
}