
namespace EM.IoC
{
	using EM.Foundation;

	public sealed class SignalCommandContainer :
		CommandContainer,
		ISignalCommandContainer
	{
		#region ISignalCommandContainer

		public new ISignalCommandBindingLifeTime Bind<T>()
			where T : class, ISignal 
		{
			var signal = container.GetInstance<T>();

			return Bind(signal) as ISignalCommandBindingLifeTime;
		}

		public new bool Unbind<T>()
			where T : class, ISignal
		{
			var signal = container.GetInstance<T>();
			signal.RemoveListener(ReactTo);

			return Unbind(signal);
		}

		#endregion
		#region CommandContainer

		public override void ReactTo(
			object trigger,
			object data)
		{
			base.ReactTo(trigger, data);

			if (GetBinding(trigger) is SignalCommandBinding binding)
			{
				if (binding.IsOneOff)
				{
					Unbind(binding);
				}
			}
		}

		protected override IBinding GetRawBinding(
			object key,
			object name)
		{
			return new SignalCommandBinding(this, key, name, BindingResolver);
		}

		protected override void BindingResolver(IBinding binding)
		{
			base.BindingResolver(binding);

			var signal = binding.Key as ISignal;
			signal.AddListener(ReactTo);
		}

		#endregion
		#region SignalCommandContainer

		public SignalCommandContainer(
			IDIContainer container) :
			base(container)
		{
		}

		#endregion
	}
}
