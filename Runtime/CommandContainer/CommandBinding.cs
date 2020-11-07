
namespace EM.IoC
{
	using EM.Foundation;

	public class CommandBinding :
		Binding,
		ICommandBinding,
		ICommandBindingComposite
	{
		#region ICommandBindingComposite

		public bool? IsSequence => isSequence;

		public ICommandBinding InParallel()
		{
			Requires.IsNull(isSequence, nameof(isSequence));

			isSequence = false;

			return this;
		}

		public ICommandBinding InSequence()
		{
			Requires.IsNull(isSequence, nameof(isSequence));

			isSequence = true;

			return this;
		}

		#endregion
		#region ICommandBinding

		public new ICommandBinding To<T>()
			where T : ICommand
		{
			Requires.IsNotNull(isSequence, nameof(isSequence));

			return base.To<T>() as ICommandBinding;
		}

		public void Execute(object data = null)
		{
			container.ReactTo(Key, data);
		}

		#endregion
		#region CommandBinding

		private readonly ICommandContainer container;

		private bool? isSequence = null;

		public CommandBinding(
			ICommandContainer container,
			object key,
			object name,
			Resolver resolver) :
			base(
				key,
				name,
				resolver)
		{
			Requires.IsNotNull(container, nameof(container));

			this.container = container;
		}

		#endregion
	}
}
