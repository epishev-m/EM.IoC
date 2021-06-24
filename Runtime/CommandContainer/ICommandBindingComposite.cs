namespace EM.IoC
{

public interface ICommandBindingComposite
{
	ICommandBinding InParallel();

	ICommandBinding InSequence();
}

}
