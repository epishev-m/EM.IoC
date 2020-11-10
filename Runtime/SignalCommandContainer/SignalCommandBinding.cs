
namespace EM.IoC
{
	using EM.Foundation;

	public sealed class SignalCommandBinding :
		CommandBinding,
		ISignalCommandBinding,
		ISignalCommandBindingOnce
	{
		#region ISignalCommandBindingOnce

		public bool IsOneOff => isOneOff;

		public ISignalCommandBindingComposite Once()
		{
			isOneOff = true;

			return this;
		}

		#endregion
		#region ISignalCommandBindingComposite

		public new ISignalCommandBinding InParallel()
		{
			return base.InParallel() as ISignalCommandBinding;
		}

		public new ISignalCommandBinding InSequence()
		{
			return base.InSequence() as ISignalCommandBinding;
		}

		public new ISignalCommandBinding To<T>()
			where T : ICommand
		{
			return base.To<T>() as ISignalCommandBinding;
		}

		#endregion
		#region SignalCommandBinding

		private bool isOneOff;

		public SignalCommandBinding(
			ICommandContainer container,
			object key,
			object name,
			Resolver resolver) :
			base(
				container,
				key,
				name,
				resolver)
		{
			isOneOff = false;
		}

		#endregion
	}
}