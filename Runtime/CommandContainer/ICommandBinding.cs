
namespace EM.IoC
{
	using EM.Foundation;

	public interface ICommandBinding
	{
		ICommandBinding To<T>()
			where T : ICommand;

		void Execute(object data = null);
	}
}
