using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/*
	public

    - void Add(T, double) : 아이템-가중치 쌍을 추가.
    - void Remove(T) : 아이템을 목록에서 제거.

    - T GetItem() : 랜덤으로 아이템 하나 Return.

	private

    - WeightNormalize() : 가중치 정규화.
	- CalculateSum() : 가중치 합 계산.

    - ReadOnlyDictionary<T, double> GetNormalizedItemDictReadonly()
		: 가중치가 정규화된 딕셔너리 읽기전용 0 ~ 1
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
			throw new Exception($"이미 존재하는 아이템 : {item}");
		}
		if(weight <= 0.0)
		{
			throw new Exception($"가중치가 0보다 작거나 같습니다 : {weight}");
		}

		itemWeight.Add(item, weight);
		notSummary = true;
	}

	public void Remove(T item)
	{
		if (!itemWeight.ContainsKey(item))
		{
			throw new Exception($"존재하지 않는 아이템 : {item}");
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

		throw new Exception($"에러 ### 랜덤 가중치 : {randomValue}, 현재 가중치 :  {current}");
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
			itemWeightNormalize.Add(item.Key, item.Value / sumWeight); // 개별 가중치 / 전체 가중치 = 확률
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
