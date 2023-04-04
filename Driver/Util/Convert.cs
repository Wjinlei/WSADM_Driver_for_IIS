namespace Driver.Util;

internal static class Convert
{
    /// <summary>
    /// Convert.ToInt32 or default value
    /// </summary>
    /// <param name="value">The value to be converted</param>
    /// <param name="defaultValue">If the conversion fails, this default value is returned</param>
    /// <returns></returns>
    internal static int ToInt32OrDefault(string value, int defaultValue)
    {
        try
        {
            return System.Convert.ToInt32(value);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }
}
