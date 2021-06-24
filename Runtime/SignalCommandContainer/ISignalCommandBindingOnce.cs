namespace EM.IoC
{

public interface ISignalCommandBindingOnce : ISignalCommandBindingComposite
{
	bool IsOneOff
	{
		get;
	}

	ISignalCommandBindingComposite Once();
}

}
