namespace HunterPie.Core.Extensions;

public static class UIntExtensions
{
    /// <summary>
    /// Finds the nearest value that is also larger than the specified value.
    /// </summary>
    public static uint ApproximateHigh(this uint self, uint[] values)
    {
        uint closest = self;

        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] < closest)
                continue;

            if (values[i] >= closest)
            {
                closest = values[i];
                break;
            }
        }

        return closest;
    }
}