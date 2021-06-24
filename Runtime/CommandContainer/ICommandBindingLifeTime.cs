namespace EM.IoC
{

public interface ICommandBindingLifeTime
{
	ICommandBindingComposite InGlobal();

	ICommandBindingComposite InLocal();
}

}
