using EM.Foundation;
using EM.IoC;
using NUnit.Framework;
using System;
using System.Linq;

internal sealed class SignalCommandBindingTests
{
	#region Constructor

	[Test]
	public void SignalCommandBinding_ConstructorParam1_Exeption()
	{
		// Arrange
		var actual = false;
		var key = typeof(object);

		// Act
		try
		{
			var unused = new SignalCommandBinding(null, key, null, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void SignalCommandBinding_ConstructorParam2_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();

		// Act
		try
		{
			var unused = new SignalCommandBinding(container, null, null, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region Composite

	[Test]
	public void SignalCommandBinding_InGlobal()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);

		// Act
		binder.InGlobal();
		var actual = binder.LifeTime;

		// Assert
		Assert.AreEqual(LifeTime.Global, actual);
	}

	[Test]
	public void SignalCommandBinding_InGlobal_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);

		// Act
		binder.InGlobal();

		try
		{
			binder.InGlobal();
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void SignalCommandBinding_InLocal()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);

		// Act
		binder.InLocal();
		var actual = binder.LifeTime;

		// Assert
		Assert.AreEqual(LifeTime.Local, actual);
	}

	[Test]
	public void SignalCommandBinding_InLocal_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);

		// Act
		binder.InLocal();

		try
		{
			binder.InLocal();
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region Composite

	[Test]
	public void SignalCommandBinding_InSequence()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);
		binder.InGlobal();

		// Act
		binder.InSequence();
		var actual = binder.IsSequence;

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void SignalCommandBinding_InSequence_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);

		// Act
		try
		{
			binder.InSequence();
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void SignalCommandBinding_InSequence_InSequence_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);
		binder.InGlobal();

		// Act
		binder.InSequence();

		try
		{
			binder.InSequence();
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void SignalCommandBinding_InParallel()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);
		binder.InGlobal();

		// Act
		binder.InParallel();
		var actual = binder.IsSequence;

		// Assert
		Assert.IsFalse(actual);
	}

	[Test]
	public void SignalCommandBinding_InParallel_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);

		// Act
		try
		{
			binder.InParallel();
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void SignalCommandBinding_InParallel_InParallel_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);
		binder.InGlobal();

		// Act
		binder.InParallel();

		try
		{
			binder.InParallel();
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region Once

	[Test]
	public void SignalCommandBinding_Once()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);

		// Act
		var binder = new SignalCommandBinding(container, key, null, null);
		binder.Once();
		var actual = binder.IsOneOff;

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region To

	[Test]
	public void SignalCommandBinding_To_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);

		// Act
		var binder = new SignalCommandBinding(container, key, null, null);
		binder.InGlobal();

		try
		{
			binder.To<CommandTest>();
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void SignalCommandBinding_InParallelAnd2To()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);

		binder.InGlobal()
			.InParallel()
			.To<CommandTest>()
			.To<CommandTest>();

		// Act
		var values = binder.Values;
		var actual = values.Count();

		// Assert
		Assert.AreEqual(2, actual);
	}

	[Test]
	public void SignalCommandBinding_InSequenceAnd2To()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);

		binder.InGlobal()
			.InSequence()
			.To<CommandTest>()
			.To<CommandTest>();

		// Act
		var values = binder.Values;
		var actual = values.Count();

		// Assert
		Assert.AreEqual(2, actual);
	}

	#endregion
	#region Execute

	[Test]
	public void SignalCommandBinding_ExecuteAndInSequence()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new SignalCommandBinding(container, key, null, null);

		// Act
		binder.InGlobal()
			.InSequence()
			.To<CommandTest>()
			.Execute();

		var actual = container.IsExecuted;

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void SignalCommandBinding_ExecuteAndInParallel()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);

		// Act
		var binder = new SignalCommandBinding(container, key, null, null);
		binder.InGlobal()
			.InParallel()
			.To<CommandTest>()
			.Execute();

		var actual = container.IsExecuted;

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region Nested

	private sealed class CommandContainerTest :
		ICommandContainer,
		ISignalCommandContainer
	{
		#region ISignalCommandContainer

		public ISignalCommandBindingLifeTime Bind<T>()
			where T : class, ISignal
		{
			throw new NotImplementedException();
		}

		public bool Unbind<T>()
			where T : class, ISignal
		{
			throw new NotImplementedException();
		}

		#endregion
		#region ICommandContainer

		ICommandBindingLifeTime ICommandContainer.Bind<T>()
		{
			throw new NotImplementedException();
		}

		ICommandBindingLifeTime ICommandContainer.Bind(object key)
		{
			throw new NotImplementedException();
		}

		public void ReactTo<T>(object data = null)
		{
			throw new NotImplementedException();
		}

		public void ReactTo(object trigger, object data = null)
		{
			IsExecuted = true;
		}

		bool ICommandContainer.Unbind<T>()
		{
			throw new NotImplementedException();
		}

		public bool Unbind(object key)
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

		#endregion
		#region CommandContainerTest

		public bool IsExecuted { get; private set; }

		#endregion
	}

	private sealed class CommandTest :
		ICommand
	{
		public object Data
		{
			set => throw new NotImplementedException();
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
			Done?.Invoke();
		}
	}

	#endregion
}
