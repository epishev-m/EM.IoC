namespace EM.IoC
{

public interface ICommandBindingComposite
{
	bool IsSequence
	{
		get;
	}

	ICommandBinding InParallel();

	ICommandBinding InSequence();
}

}
