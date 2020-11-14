using EM.Foundation;
using EM.IoC;
using NUnit.Framework;
using System;

internal sealed class SignalCommandContainerTests
{
	[Test]
	public void SignalCommandContainer_Constructor_Exeption()
	{
		// Arrange
		var actual = false;
		var container = default(IDIContainer);

		// Act
		try
		{
			var unused = new SignalCommandContainer(container);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void SignalCommandContainer_Bind()
	{
		// Arrange
		var container = new DIContainer();

		// Act
		var signalCommandContainer = new SignalCommandContainer(container);
		var actual = signalCommandContainer.Bind<SignalTest>();

		//Assert
		Assert.IsNotNull(actual);
	}

	[Test]
	public void SignalCommandContainer_Bind_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new DIContainer();
		var signalCommandContainer = new SignalCommandContainer(container);

		// Act
		try
		{
			var unused = signalCommandContainer.Bind<SignalFaceTest>();
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		//Assert
		Assert.IsNotNull(actual);
	}

	[Test]
	public void SignalCommandContainer_ReactTo()
	{
		// Arrange
		var actual = 0;
		var expected = 2;
		var container = new DIContainer();
		var commandContainer = new SignalCommandContainer(container);
		CommandTest.Collback = () => actual++;

		// Act
		commandContainer.Bind<SignalTest>()
			.InGlobal()
			.InParallel()
			.To<CommandTest>()
			.To<CommandTest>()
			.Execute();

		//Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void CommandContainer_ReactToAndData()
	{
		// Arrange
		var expected = new Test();
		var container = new DIContainer();
		var commandContainer = new CommandContainer(container);
		CommandTest.Collback = () => { };

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

	[Test]
	public void SignalCommandContainer_Unbind_ReturnFalse()
	{
		// Arrange
		var container = new DIContainer();

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
		var container = new DIContainer();
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

	#region Nested

	internal sealed class SignalTest : SignalEx
	{
	}

	internal sealed class SignalFaceTest : SignalEx
	{
	}

	internal sealed class CommandTest : ICommand
	{
		#region CommandTest

		public static Action Collback;

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
			Collback.Invoke();
			Done.Invoke();
		}

		#endregion
	}

	internal sealed class DIContainer :
		IDIContainer
	{
		private readonly ISignal signal = new SignalTest();

		public IDIBindingLifeTime Bind<T>()
			where T : class
		{
			throw new NotImplementedException();
		}

		public object GetInstance(
			Type type)
		{
			return new CommandTest();
		}

		public T GetInstance<T>()
			where T : class
		{
			return signal as T;
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

	internal sealed class Test
	{
	}

	#endregion
}
