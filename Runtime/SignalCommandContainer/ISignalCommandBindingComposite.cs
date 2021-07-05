namespace EM.IoC
{

public interface ISignalCommandBindingComposite
{
	bool IsSequence
	{
		get;
	}

	ISignalCommandBinding InParallel();

	ISignalCommandBinding InSequence();
}

}
