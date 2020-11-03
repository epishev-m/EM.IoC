
namespace EM.IoC
{
	using EM.Foundation;

	public interface ICommandBinding
	{
		bool IsSequence
		{
			get;
		}

		ICommandBinding InParallel();

		ICommandBinding InSequence();

		ICommandBinding To<T>()
			where T : ICommand;
	}
}
