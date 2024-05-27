namespace Reaper;

public static class Util
{
    #region List

    public static List<T> QuickSort<T>(List<T> list, int leftIndex, int rightIndex, Comparison<T> compare)
    {
        int left = leftIndex;
        int right = rightIndex;
        T pivot = list[left];

        while (left <= right)
        {
            while (compare.Invoke(list[left], pivot) < 0)
            {
                left++;
            }

            while (compare.Invoke(list[right], pivot) > 0)
            {
                right--;
            }

            if (left <= right)
            {
                (list[right], list[left]) = (list[left], list[right]);
                left++;
                right--;
            }
        }

        if (leftIndex < right)
            QuickSort(list, leftIndex, right, compare);

        if (left < rightIndex)
            QuickSort(list, left, rightIndex, compare);

        return list;
    }

    #endregion List
}
