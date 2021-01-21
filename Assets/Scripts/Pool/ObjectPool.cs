using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
	public int CountInQueue => queue.Count;
	public int CountUsed => usedItems.Count;

	private Queue<T> queue;
	private List<T> usedItems;
	private Func<T> factoryFunc;

	public ObjectPool(Func<T> factoryFunc, int initialSize)
	{
		this.factoryFunc = factoryFunc;

		queue = new Queue<T>(initialSize);
		usedItems = new List<T>(initialSize);

		Warm(initialSize);
	}

	private void Warm(int capacity)
	{
		for (int i = 0; i < capacity; i++)
		{
			queue.Enqueue(factoryFunc());
		}
	}

	#region API

	public T GetItem()
	{
		if (queue.Count == 0)
			queue.Enqueue(factoryFunc());

		T item = queue.Dequeue();

		usedItems.Add(item);

		return item;
	}

	public void ReleaseItem(object item)
	{
		if (item is T)
		{
			ReleaseItem((T)item);
		}
		else
		{
			Debug.LogWarning("This object pool does not contain the item provided: " + item);
		}
	}

	public void ReleaseItem(T item)
	{
		if (usedItems.Contains(item))
			queue.Enqueue(item);
	}

	#endregion
}
