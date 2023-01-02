#region

using System.Reflection;

#endregion

namespace Api.Areas.Edu.DTOs;

public static class HoTro
{
	public static TOutputType Convert<TInputType, TOutputType>(this TInputType @base)
		where TInputType : class
		where TOutputType : class, new()
	{
		if (typeof(TInputType) == typeof(TOutputType))
			return (TOutputType)(object)@base;

		TOutputType soYeuLyLich = new();

		List<PropertyInfo> properties = LayToanBoPropertyTheoInterface(typeof(TInputType));
		properties.ForEach(property =>
						   {
							   try
							   {
								   property.SetValue(soYeuLyLich, property.GetValue(@base));
							   }
							   catch (Exception)
							   {
								   // ignored
							   }
						   });
		return soYeuLyLich;
	}

	public static List<PropertyInfo> LayToanBoPropertyTheoInterface(Type type)
	{
		List<PropertyInfo> properties = new();
		if (type.IsInterface) properties.AddRange(type.GetProperties());
		Type[] interfaces = type.GetInterfaces();
		foreach (Type @interface in interfaces)
			properties.AddRange(LayToanBoPropertyTheoInterface(@interface));
		return properties;
	}
}