namespace EM.IoC
{

using System;

public interface IStateFactory<out TState>
{
	TState Create(Type stateType);
}

}