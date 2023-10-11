namespace EM.IoC
{

using System.Globalization;
using System.Runtime.CompilerServices;

internal static class DiStringResources
{
	internal static string FailedToCreate(InstanceProviderActivator provider,
		[CallerMemberName] string memberName = "",
		[CallerLineNumber] int lineNumber = 0)
	{
		return string.Format(CultureInfo.InvariantCulture,
			"[Error] Failed to create the specified type. \n {0}.{1}:{2}",
			provider.GetType(),
			memberName,
			lineNumber);
	}
	
	internal static string CouldNotFindConstructor(InstanceProviderActivator provider,
		[CallerMemberName] string memberName = "",
		[CallerLineNumber] int lineNumber = 0)
	{
		return string.Format(CultureInfo.InvariantCulture,
			"[Error] Could not find a suitable constructor for the specified type. \n {0}.{1}:{2}",
			provider.GetType(),
			memberName,
			lineNumber);
	}
}

}