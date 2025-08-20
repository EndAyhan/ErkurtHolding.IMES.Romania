using System.Collections.Generic;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Extensions
{
    public static class DataControlExtension
    {
        public static bool HasEntries<T>(this List<T> entites) where T : class
        {
            if (entites != null && entites.Count > 0)
                return true;
            else
                return false;
        }
    }
}
