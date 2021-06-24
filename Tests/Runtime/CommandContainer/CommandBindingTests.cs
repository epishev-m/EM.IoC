using EM.Foundation;
using EM.IoC;
using NUnit.Framework;
using System;
using System.Linq;

internal sealed class CommandBindingTests
{
	#region Constructor

	[Test]
	public void CommandBinding_ConstructorParam1_Exeption()
	{
		// Arrange
		var actual = false;
		var key = typeof(object);

		// Act
		try
		{
			var unused = new CommandBinding(null, key, null, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void CommandBinding_ConstructorParam2_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();

		// Act
		try
		{
			var unused = new CommandBinding(container, null, null, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region LifeTime

	[Test]
	public void CommandBinding_InGlobal()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);

		// Act
		binder.InGlobal();
		var actual = binder.LifeTime;

		// Assert
		Assert.AreEqual(LifeTime.Global, actual);
	}

	[Test]
	public void CommandBinding_InGlobal_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);
		binder.InGlobal();

		// Act
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
	public void CommandBinding_InLocal()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);

		// Act
		binder.InLocal();
		var actual = binder.LifeTime;

		// Assert
		Assert.AreEqual(LifeTime.Local, actual);
	}

	[Test]
	public void CommandBinding_InLocal_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);
		binder.InLocal();

		// Act
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
	public void CommandBinding_InSequence()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);
		binder.InGlobal();

		// Act
		binder.InSequence();
		var actual = binder.IsSequence;

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void CommandBinding_InSequence_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);

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
	public void CommandBinding_InSequence_InSequence_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);
		binder.InGlobal().InSequence();

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
	public void CommandBinding_InParallel()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);
		binder.InGlobal().InParallel();

		// Act
		var actual = binder.IsSequence;

		// Assert
		Assert.IsFalse(actual);
	}

	[Test]
	public void CommandBinding_InParallel_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);

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
	public void CommandBinding_InParallel_InParallel_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);
		binder.InGlobal().InParallel();

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

	#endregion
	#region To

	[Test]
	public void CommandBinding_To_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);
		binder.InGlobal();

		// Act
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
	public void CommandBinding_InParallelAnd2To()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);

		// Act
		binder.InGlobal()
			.InParallel()
			.To<CommandTest>()
			.To<CommandTest>();
		var values = binder.Values;
		var actual = values.Count();

		// Assert
		Assert.AreEqual(2, actual);
	}

	[Test]
	public void CommandBinding_InSequenceAnd2To()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);

		// Act
		binder.InGlobal()
			.InSequence()
			.To<CommandTest>()
			.To<CommandTest>();
		var values = binder.Values;
		var actual = values.Count();

		// Assert
		Assert.AreEqual(2, actual);
	}

	#endregion
	#region Execute

	[Test]
	public void CommandBinding_ExecuteAndInSequence()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);

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
	public void CommandBinding_ExecuteAndInParallel()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);
		var binder = new CommandBinding(container, key, null, null);

		// Act
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
		ICommandContainer
	{
		#region ICommandContainer

		public ICommandBindingLifeTime Bind<T>()
		{
			throw new NotImplementedException();
		}

		public ICommandBindingLifeTime Bind(object key)
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

		public bool Unbind<T>()
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
