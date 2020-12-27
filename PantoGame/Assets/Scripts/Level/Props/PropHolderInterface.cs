using System.Collections.Generic;

public interface PropHolderInterface
{
	bool CanHoldProp(PropInterface prop);
	void AddProp(PropInterface prop);
	void RemoveProp(PropInterface prop);
}