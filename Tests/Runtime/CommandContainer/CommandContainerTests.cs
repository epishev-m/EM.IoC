using EM.Foundation;
using EM.IoC;
using NUnit.Framework;
using System;

internal sealed class CommandContainerTests
{
	#region Constructor

	[Test]
	public void CommandContainer_Constructor_Exeption()
	{
		// Arrange
		var actual = false;
		var container = default(IDIContainer);

		// Act
		try
		{
			var unused = new CommandContainer(container);
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
	public void CommandContainer_Bind()
	{
		// Arrange
		var container = new DIContainer();
		var commandContainer = new CommandContainer(container);
		var key = typeof(CommandTest);

		// Act
		var actual = commandContainer.Bind(key);

		//Assert
		Assert.IsNotNull(actual);
	}

	[Test]
	public void CommandContainer_Bind_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new DIContainer();
		var commandContainer = new CommandContainer(container);
		var key = default(object);

		// Act
		try
		{
			var unused = commandContainer.Bind(key);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		//Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void CommandContainer_BindGeneric()
	{
		// Arrange
		var container = new DIContainer();

		// Act
		var commandContainer = new CommandContainer(container);
		var actual = commandContainer.Bind<CommandTest>();

		//Assert
		Assert.IsNotNull(actual);
	}

	#endregion
	#region ReactTo

	[Test]
	public void CommandContainer_ReactTo()
	{
		// Arrange
		var actual = 0;
		var expected = 2;
		var container = new DIContainer();
		var commandContainer = new CommandContainer(container);
		var key = typeof(CommandTest);
		CommandTest.Collback = () => actual++;

		// Act
		commandContainer.Bind(key)
			.InParallel()
			.To<CommandTest>()
			.To<CommandTest>();

		commandContainer.ReactTo(key);

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
		var key = typeof(CommandTest);
		CommandTest.Collback = () => { };

		// Act
		commandContainer.Bind(key)
			.InParallel()
			.To<CommandTest>();

		commandContainer.ReactTo(key, expected);
		var actual = CommandTest.DataTest;

		//Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void CommandContainer_ReactToGeneric()
	{
		// Arrange
		var actual = 0;
		var expected = 2;
		var container = new DIContainer();
		var commandContainer = new CommandContainer(container);
		CommandTest.Collback = () => actual++;

		// Act
		commandContainer.Bind<CommandTest>()
			.InParallel()
			.To<CommandTest>()
			.To<CommandTest>();

		commandContainer.ReactTo<CommandTest>();

		//Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void CommandContainer_ReactToGenericAndData()
	{
		// Arrange
		var expected = new Test();
		var container = new DIContainer();
		var commandContainer = new CommandContainer(container);
		CommandTest.Collback = () => { };

		// Act
		commandContainer.Bind<CommandTest>()
			.InParallel()
			.To<CommandTest>();

		commandContainer.ReactTo<CommandTest>(expected);
		var actual = CommandTest.DataTest;

		//Assert
		Assert.AreEqual(expected, actual);
	}

	#endregion
	#region Unbind

	[Test]
	public void CommandContainer_UnbindGeneric_ReturnFalse()
	{
		// Arrange
		var container = new DIContainer();

		// Act
		var commandContainer = new CommandContainer(container);
		var actual = commandContainer.Unbind<CommandTest>();

		//Assert
		Assert.IsFalse(actual);
	}

	[Test]
	public void CommandContainer_UnbindGeneric_ReturnTrue()
	{
		// Arrange
		var container = new DIContainer();

		// Act
		var commandContainer = new CommandContainer(container);
		commandContainer.Bind<CommandTest>()
			.InParallel()
			.To<CommandTest>();
		var actual = commandContainer.Unbind<CommandTest>();

		//Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region Nested

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
		public IDIBinding Bind<T>()
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
			throw new NotImplementedException();
		}

		public bool Unbind<T>()
			where T : class
		{
			throw new NotImplementedException();
		}
	}

	internal sealed class Test
	{
	}

	#endregion
}
