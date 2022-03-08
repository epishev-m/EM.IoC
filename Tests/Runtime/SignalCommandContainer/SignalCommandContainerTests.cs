using EM.Foundation;
using EM.IoC;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

internal sealed class SignalCommandContainerTests
{
	#region Constructor

	[Test]
	public void SignalCommandContainer_Constructor_Exception()
	{
		// Arrange
		var actual = false;

		// Act
		try
		{
			var unused = new SignalCommandContainer(null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region Bind

	[Test]
	public void SignalCommandContainer_Bind()
	{
		// Arrange
		var container = new DiContainer();

		// Act
		var signalCommandContainer = new SignalCommandContainer(container);
		var actual = signalCommandContainer.Bind<SignalTest>();

		//Assert
		Assert.IsNotNull(actual);
	}

	[Test]
	public void SignalCommandContainer_Bind_Exception()
	{
		// Arrange
		var actual = false;
		var container = new DiContainer();
		var signalCommandContainer = new SignalCommandContainer(container);

		// Act
		try
		{
			var unused = signalCommandContainer.Bind<SignalFakeTest>();
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		//Assert
		Assert.IsNotNull(actual);
	}

	#endregion
	#region ReactTo

	[Test]
	public void SignalCommandContainer_ReactTo()
	{
		// Arrange
		var actual = 0;
		var container = new DiContainer();
		var commandContainer = new SignalCommandContainer(container);
		CommandTest.Callback = () => actual++;

		// Act
		commandContainer.Bind<SignalTest>()
			.InGlobal()
			.InParallel()
			.To<CommandTest>()
			.To<CommandTest>()
			.Execute();

		//Assert
		Assert.AreEqual(2, actual);
	}

	[Test]
	public void CommandContainer_ReactToAndData()
	{
		// Arrange
		var expected = new Test();
		var container = new DiContainer();
		var commandContainer = new CommandContainer(container);
		CommandTest.Callback = () => { };

		// Act
		commandContainer.Bind<SignalTest>()
			.InGlobal()
			.InParallel()
			.To<CommandTest>()
			.Execute(expected);

		var actual = CommandTest.DataTest;

		//Assert
		Assert.AreEqual(expected, actual);
	}

	#endregion
	#region Unbind

	[Test]
	public void SignalCommandContainer_Unbind_ReturnFalse()
	{
		// Arrange
		var container = new DiContainer();

		// Act
		var commandContainer = new SignalCommandContainer(container);
		var actual = commandContainer.Unbind<SignalTest>();

		//Assert
		Assert.IsFalse(actual);
	}

	[Test]
	public void SignalCommandContainer_Unbind_ReturnTrue()
	{
		// Arrange
		var container = new DiContainer();
		var commandContainer = new SignalCommandContainer(container);

		commandContainer.Bind<SignalTest>()
			.InGlobal()
			.InParallel()
			.To<CommandTest>();

		// Act
		var actual = commandContainer.Unbind<SignalTest>();

		//Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void SignalCommandContainer_Unbind_SignalRelease()
	{
		// Arrange
		var container = new DiContainer();
		var commandContainer = new SignalCommandContainer(container);
		var signal = container.GetInstance<SignalTest>();

		commandContainer.Bind<SignalTest>()
			.InGlobal()
			.InParallel()
			.To<CommandTest>();

		// Act
		commandContainer.Unbind<SignalTest>();
		var actual = signal.Dispatch();

		//Assert
		Assert.IsFalse(actual);
	}

	[Test]
	public void SignalCommandContainer_UnbindAll()
	{
		// Arrange
		var container = new DiContainer();
		var commandContainer = new SignalCommandContainer(container);
		var expected = commandContainer.Bind<SignalTest>();

		var unused = expected.InGlobal()
			.InParallel()
			.To<CommandTest>();

		// Act
		commandContainer.UnbindAll();
		var actual = commandContainer.Bind<SignalTest>();

		//Assert
		Assert.AreNotEqual(expected, actual);
	}

	[Test]
	public void SignalCommandContainer_UnbindAll_SignalRelease()
	{
		// Arrange
		var container = new DiContainer();
		var commandContainer = new SignalCommandContainer(container);
		var signal = container.GetInstance<SignalTest>();

		var unused = commandContainer.Bind<SignalTest>()
			.InGlobal()
			.InParallel()
			.To<CommandTest>();

		// Act
		commandContainer.UnbindAll();
		var actual = signal.Dispatch();

		//Assert
		Assert.IsFalse(actual);
	}

	[Test]
	public void SignalCommandContainer_UnbindLifeTime_AreNotEqual()
	{
		// Arrange
		var container = new DiContainer();
		var commandContainer = new SignalCommandContainer(container);
		var expected = commandContainer.Bind<SignalTest>();

		var unused = expected.InLocal()
			.InParallel()
			.To<CommandTest>();

		// Act
		commandContainer.Unbind(LifeTime.Local);
		var actual = commandContainer.Bind<SignalTest>();

		//Assert
		Assert.AreNotEqual(expected, actual);
	}

	[Test]
	public void SignalCommandContainer_UnbindLifeTime_AreEqual()
	{
		// Arrange
		var container = new DiContainer();
		var commandContainer = new SignalCommandContainer(container);
		var expected = commandContainer.Bind<SignalTest>();

		var unused = expected.InGlobal()
			.InParallel()
			.To<CommandTest>();

		// Act
		commandContainer.Unbind(LifeTime.Local);
		var actual = commandContainer.Bind<SignalTest>();

		//Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void SignalCommandContainer_UnbindLifeTime_SignalRelease()
	{
		// Arrange
		var container = new DiContainer();
		var commandContainer = new SignalCommandContainer(container);
		var signal = container.GetInstance<SignalTest>();

		var unused = commandContainer.Bind<SignalTest>()
			.InLocal()
			.InParallel()
			.To<CommandTest>();

		// Act
		commandContainer.Unbind(LifeTime.Local);
		var actual = signal.Dispatch();

		//Assert
		Assert.IsFalse(actual);
	}

	#endregion
	#region Nested

	private sealed class SignalTest : SignalEx
	{
	}

	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
	private sealed class SignalFakeTest : SignalEx
	{
	}

	private sealed class CommandTest : ICommand
	{
		#region CommandTest

		public static Action Callback;

		public static Test DataTest;

		#endregion
		#region ICommand

		public object Data
		{
			set => DataTest = value as Test;
		}

		public bool IsDone => throw new NotImplementedException();

		public bool IsFailed => throw new NotImplementedException();

		public event Action Done;

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public void Execute()
		{
			Callback.Invoke();
			Done?.Invoke();
		}

		#endregion
	}

	private sealed class DiContainer :
		IDiContainer
	{
		private readonly ISignal signal = new SignalTest();

		public IDiBindingLifeTime Bind<T>()
			where T : class
		{
			throw new NotImplementedException();
		}

		public object GetInstance(Type type)
		{
			return new CommandTest();
		}

		public T GetInstance<T>()
			where T : class
		{
			return signal as T;
		}

		public void Inject(object obj)
		{
			throw new NotImplementedException();
		}

		public bool Unbind<T>()
			where T : class
		{
			throw new NotImplementedException();
		}

		public void UnbindAll()
		{
			throw new NotImplementedException();
		}

		public void Unbind(LifeTime lifeTime)
		{
			throw new NotImplementedException();
		}
	}

	private sealed class Test
	{
	}

	#endregion
}
