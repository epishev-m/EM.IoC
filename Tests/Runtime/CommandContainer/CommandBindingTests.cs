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
		var container = default(ICommandContainer);
		var key = typeof(object);

		// Act
		try
		{
			var unused = new CommandBinding(container, key, null, null);
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
		var key = default(object);

		// Act
		try
		{
			var unused = new CommandBinding(container, key, null, null);
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
	public void CommandBinding_InSequence()
	{
		// Arrange
		var expected = true;
		var container = new CommandContainerTest();
		var key = typeof(object);

		// Act
		var binder = new CommandBinding(container, key, null, null);
		binder.InSequence();
		var actual = binder.IsSequence;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void CommandBinding_InSequence_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);

		// Act
		var binder = new CommandBinding(container, key, null, null);
		binder.InSequence();

		try
		{
			binder.InSequence();
		}
		catch (ArgumentException)
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
		var expected = false;
		var container = new CommandContainerTest();
		var key = typeof(object);

		// Act
		var binder = new CommandBinding(container, key, null, null);
		binder.InParallel();
		var actual = binder.IsSequence;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void CommandBinding_InParallel_Exeption()
	{
		// Arrange
		var actual = false;
		var container = new CommandContainerTest();
		var key = typeof(object);

		// Act
		var binder = new CommandBinding(container, key, null, null);
		binder.InParallel();

		try
		{
			binder.InParallel();
		}
		catch (ArgumentException)
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

		// Act
		var binder = new CommandBinding(container, key, null, null);

		try
		{
			binder.To<CommandTest>();
		}
		catch (ArgumentNullException)
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
		var expected = 2;
		var container = new CommandContainerTest();
		var key = typeof(object);

		// Act
		var binder = new CommandBinding(container, key, null, null);
		binder.InParallel()
			.To<CommandTest>()
			.To<CommandTest>();
		var values = binder.Values;
		var actual = values.Count();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void CommandBinding_InSequenceAnd2To()
	{
		// Arrange
		var expected = 2;
		var container = new CommandContainerTest();
		var key = typeof(object);

		// Act
		var binder = new CommandBinding(container, key, null, null);
		binder.InSequence()
			.To<CommandTest>()
			.To<CommandTest>();
		var values = binder.Values;
		var actual = values.Count();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	#endregion
	#region Execute

	[Test]
	public void CommandBinding_ExecuteAndInSequence()
	{
		// Arrange
		var container = new CommandContainerTest();
		var key = typeof(object);

		// Act
		var binder = new CommandBinding(container, key, null, null);
		binder.InSequence()
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

		// Act
		var binder = new CommandBinding(container, key, null, null);
		binder.InParallel()
			.To<CommandTest>()
			.Execute();

		var actual = container.IsExecuted;

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region Nested

	internal sealed class CommandContainerTest : ICommandContainer
	{
		#region ICommandContainer

		public ICommandBindingComposite Bind<T>()
		{
			throw new NotImplementedException();
		}

		public ICommandBindingComposite Bind(object key)
		{
			throw new NotImplementedException();
		}

		public void ReactTo<T>(object data = null)
		{
			throw new NotImplementedException();
		}

		public void ReactTo(object trigger, object data = null)
		{
			isExecuted = true;
		}

		public bool Unbind<T>()
		{
			throw new NotImplementedException();
		}

		public bool Unbind(object key)
		{
			throw new NotImplementedException();
		}

		#endregion
		#region CommandContainerTest

		private bool isExecuted = false;

		public bool IsExecuted => isExecuted;

		#endregion
	}

	internal sealed class CommandTest : ICommand
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
			Done.Invoke();
		}
	}

	#endregion
}
