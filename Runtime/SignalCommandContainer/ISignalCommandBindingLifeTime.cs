namespace EM.IoC
{

public interface ISignalCommandBindingLifeTime
{
	ISignalCommandBindingComposite InGlobal();

	ISignalCommandBindingComposite InLocal();
}

}
