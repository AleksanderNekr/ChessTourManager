using System.Reflection;

namespace ChessTourManager.WPF.Helpers.FileHelpers;

public static class GetPropertyValuesMethods
{
    /// Function to get the value of a property with a path that may have multiple levels.
    public static object? GetPropertyValue(object obj, string propertyPath)
    {
        string[] propertyNames = propertyPath.Split('.');
        object?  propertyValue = obj;

        foreach (string propertyName in propertyNames)
        {
            PropertyInfo? property = propertyValue.GetType().GetProperty(propertyName);
            if (property == null)
            {
                return null;
            }

            propertyValue = property.GetValue(propertyValue, null);
            if (propertyValue == null)
            {
                return null;
            }
        }

        return propertyValue;
    }
}
