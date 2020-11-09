
namespace EM.IoC
{
	using EM.Foundation;

	public interface ISignalCommandBinding
	{
		ISignalCommandBinding To<T>()
			where T : ICommand;

		void Execute(
			object data = null);
	}
}
