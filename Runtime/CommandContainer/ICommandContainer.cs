
namespace EM.IoC
{
	public interface ICommandContainer
	{
		void ReactTo<T>(
			object data = null);

		void ReactTo(
			object trigger,
			object data = null);

		ICommandBindingComposite Bind<T>();

		ICommandBindingComposite Bind(
			object key);

		bool Unbind<T>();

		bool Unbind(
			object key);
	}
}
