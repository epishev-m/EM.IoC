
namespace EM.IoC
{
	using EM.Foundation;

	public class CommandBinding :
		Binding,
		ICommandBinding,
		ICommandBindingComposite,
		ICommandBindingLifeTime
	{
		#region ICommandBindingLifeTime

		public LifeTime LifeTime => lifeTime;

		public ICommandBindingComposite InGlobal()
		{
			Requires.IsValidOperation(lifeTime == LifeTime.External, this, nameof(InGlobal));

			lifeTime = LifeTime.Global;

			return this;
		}

		public ICommandBindingComposite InLocal()
		{
			Requires.IsValidOperation(lifeTime == LifeTime.External, this, nameof(InLocal));

			lifeTime = LifeTime.Local;

			return this;
		}

		#endregion
		#region ICommandBindingComposite

		public bool? IsSequence => isSequence;

		public ICommandBinding InParallel()
		{
			Requires.IsValidOperation(lifeTime != LifeTime.External, this, nameof(InParallel));
			Requires.IsValidOperation(isSequence == null, this, nameof(InParallel));

			isSequence = false;

			return this;
		}

		public ICommandBinding InSequence()
		{
			Requires.IsValidOperation(lifeTime != LifeTime.External, this, nameof(InSequence));
			Requires.IsValidOperation(isSequence == null, this, nameof(InSequence));

			isSequence = true;

			return this;
		}

		#endregion
		#region ICommandBinding

		public new ICommandBinding To<T>()
			where T : ICommand
		{
			Requires.IsValidOperation(lifeTime != LifeTime.External, this, nameof(To));
			Requires.IsValidOperation(isSequence != null, this, nameof(isSequence));

			return base.To<T>() as ICommandBinding;
		}

		public void Execute(object data = null)
		{
			Requires.IsValidOperation(lifeTime != LifeTime.External, this, nameof(Execute));
			Requires.IsValidOperation(isSequence != null, this, nameof(isSequence));

			container.ReactTo(Key, data);
		}

		#endregion
		#region CommandBinding

		private readonly ICommandContainer container;

		private bool? isSequence = null;

		private LifeTime lifeTime = LifeTime.External;

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
