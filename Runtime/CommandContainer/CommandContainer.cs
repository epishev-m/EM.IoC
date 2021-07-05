namespace EM.IoC
{
using Foundation;
using System;
using System.Collections.Generic;

public class CommandContainer :
	Binder,
	ICommandContainer
{
	protected readonly IDiContainer Container;

	private readonly Pool<CommandSequence> poolSequences;

	private readonly Pool<CommandBatch> poolBatches;

	private readonly Dictionary<Type, Pool<ICommand>> poolCommands;

	#region ICommandContainer

	public ICommandBindingLifeTime Bind<T>()
	{
		return base.Bind<T>() as ICommandBindingLifeTime;
	}

	public ICommandBindingLifeTime Bind(
		object key)
	{
		return base.Bind(key) as ICommandBindingLifeTime;
	}

	public void ReactTo<T>(
		object data = null)
	{
		ReactTo(typeof(T), data);
	}

	public virtual void ReactTo(
		object trigger,
		object data = null)
	{
		if (!(GetBinding(trigger) is CommandBinding binding))
		{
			return;
		}

		var composite = binding.IsSequence ? GetSequence() : GetBatch();
		var commandTypesArray = binding.Values;

		foreach (var type in commandTypesArray)
		{
			var command = GetCommand(type as Type);
			command.Data = data;
			composite.Add(command);
		}

		composite.Done += () => PutComposite(composite, binding.IsSequence);
		composite.Execute();
	}

	public bool Unbind<T>()
	{
		return base.Unbind<T>();
	}

	public bool Unbind(
		object key)
	{
		return base.Unbind(key);
	}

	public void Unbind(
		LifeTime lifeTime)
	{
		Unbind(binding =>
		{
			var result = binding is CommandBinding diBinding && diBinding.LifeTime == lifeTime;

			return result;
		});
	}

	#endregion
	#region Binder

	protected override IBinding GetRawBinding(
		object key,
		object name)
	{
		return new CommandBinding(this, key, name, BindingResolver);
	}

	#endregion
	#region CommandContainer

	public CommandContainer(
		IDiContainer container)
	{
		Requires.NotNull(container, nameof(container));

		Container = container;
		poolSequences = new Pool<CommandSequence>();
		poolBatches = new Pool<CommandBatch>();
		poolCommands = new Dictionary<Type, Pool<ICommand>>();
	}

	private void PutComposite(
		ICommandComposite composite,
		bool isSequence)
	{
		PutCommands(composite.Commands);

		if (isSequence)
		{
			poolSequences.PutObject(composite as CommandSequence);
		}
		else
		{
			poolBatches.PutObject(composite as CommandBatch);
		}
	}

	private ICommandComposite GetSequence()
	{
		var sequence = poolSequences.GetObject();
		sequence ??= new CommandSequence();

		return sequence;
	}

	private ICommandComposite GetBatch()
	{
		var batch = poolBatches.GetObject();
		batch ??= new CommandBatch();

		return batch;
	}

	private ICommand GetCommand(
		Type commandType)
	{
		var command = default(ICommand);

		if (poolCommands.TryGetValue(commandType, out var pool))
		{
			command = pool.GetObject();
		}
		else
		{
			pool = new Pool<ICommand>();
			poolCommands.Add(commandType, pool);
		}

		command ??= Container.GetInstance(commandType) as ICommand;

		return command;
	}

	private void PutCommands(
		IEnumerable<ICommand> commands)
	{
		foreach (var command in commands)
		{
			var commandType = command.GetType();
			var pool = poolCommands[commandType];
			pool.PutObject(command);
		}
	}

	#endregion
}

}
