using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataObserver
{
	public abstract void OnDataUpdated();
}

public interface IDataSubject
{
	public void AddObserver(DataObserver observer);
	public void RemoveObserver(DataObserver observer);
	public void NotifyObservers();
}
