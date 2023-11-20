using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/*
	public

    - void Add(T, double) : ������-����ġ ���� �߰�.
    - void Remove(T) : �������� ��Ͽ��� ����.

    - T GetItem() : �������� ������ �ϳ� Return.

	private

    - WeightNormalize() : ����ġ ����ȭ.
	- CalculateSum() : ����ġ �� ���.

    - ReadOnlyDictionary<T, double> GetNormalizedItemDictReadonly()
		: ����ġ�� ����ȭ�� ��ųʸ� �б����� 0 ~ 1
*/


public class GachaSystem<T>
{
	private System.Random random;
	private readonly Dictionary<T, double> itemWeight;
	private readonly Dictionary<T, double> itemWeightNormalize;

	private bool notSummary;
	private double sumWeight;

	public GachaSystem() 
	{ 
		random = new System.Random();
		itemWeight = new Dictionary<T, double>();
		itemWeightNormalize = new Dictionary<T, double>();
		notSummary = true;
		sumWeight = 0.0;
	}

	public void Add(T item, double weight)
	{
		if(itemWeight.ContainsKey(item))
		{
			throw new Exception($"�̹� �����ϴ� ������ : {item}");
		}
		if(weight <= 0.0)
		{
			throw new Exception($"����ġ�� 0���� �۰ų� �����ϴ� : {weight}");
		}

		itemWeight.Add(item, weight);
		notSummary = true;
	}

	public void Remove(T item)
	{
		if (!itemWeight.ContainsKey(item))
		{
			throw new Exception($"�������� �ʴ� ������ : {item}");
		}

		itemWeight.Remove(item);
		notSummary = true;
	}

	public T GetItem()
	{
		CalculateSum();

		double randomValue = random.NextDouble(); //0.0 ~ 1.0
		randomValue *= sumWeight;

		if (randomValue < 0.0) randomValue = 0.0;
		if (randomValue > sumWeight) randomValue = sumWeight - 0.000001;

		double current = 0.0;

		foreach (var pair in itemWeight)
		{
			current += pair.Value;

			if (randomValue < current)
			{
				return pair.Key;
			}
		}

		throw new Exception($"���� ### ���� ����ġ : {randomValue}, ���� ����ġ :  {current}");
	}

	public T[] GetItem(int count)
	{
		T[] itemArr = new T[count];

		for (int i = 0; i < count; i++)
		{
			itemArr[i] = GetItem();
		}

		return itemArr;
	}


	private void WeightNormalize()
	{
		itemWeightNormalize.Clear();
		foreach (var item in itemWeight)
		{
			itemWeightNormalize.Add(item.Key, item.Value / sumWeight); // ���� ����ġ / ��ü ����ġ = Ȯ��
		}
	}

	private void CalculateSum()
	{
		if (!notSummary) return;
		notSummary = false;

		sumWeight = 0.0;

		foreach (var item in itemWeight)
		{
			sumWeight += item.Value;
		}

		WeightNormalize();
	}

	public ReadOnlyDictionary<T, double> GetNormalizedDictReadonly()
	{
		return new ReadOnlyDictionary<T, double>(itemWeightNormalize);
	}
}
