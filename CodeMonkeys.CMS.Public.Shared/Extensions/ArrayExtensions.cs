namespace CodeMonkeys.CMS.Public.Shared.Extensions
{
    public static class ArrayExtensions
    {
        public static int FindIndex<T>(this T[] array, Predicate<T> match)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            for (int i = 0; i < array.Length; i++)
            {
                if (match(array[i])) return i;
            }

            return -1;
        }
    }
}