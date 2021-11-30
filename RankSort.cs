using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class RankSort<T>
{
	public delegate bool Comparer(T t1, T t2);
	private static Group[] indexs;
	private int groupCount;
	private T maxValue;
	private int curIndex;
	private int maxIndex;

	private struct Group
	{
		public Group(int i, int s, int l)
		{
			startIndex = s;
			length = l;
		}
		public int startIndex;
		public int length;
	}
	/// <summary>
	/// 对最大值排序 
	/// </summary>
	/// <param name="arr"></param>
	private void Sort(Group[] arr, T[] datas, Comparer action)
	{
		Group temp;
		for (int i = 0; i < arr.Length; i++)
		{
			for (int j = i + 1; j < arr.Length; j++)
			{
				if (!action(datas[arr[i].startIndex], datas[arr[j].startIndex]))
				{
					temp = arr[i];
					arr[i] = arr[j];
					arr[j] = temp;
				}
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="arr"></param>
	/// <param name="num"></param>
	/// <param name="action"></param>
	/// <returns></returns>
	public T[] Sort(T[] arr, int num, Comparer action)
	{
		indexs = new Group[num];
		Group tempGroup;
		groupCount = (arr.Length + 1) / num;
		int length;
		T temp;
		for (int i = 0; i < num; i++)
		{
			maxIndex = i * groupCount;
			maxValue = arr[maxIndex];
			length = groupCount;
			if (i == num - 1)
			{
				length = arr.Length - (num - 1) * groupCount;
			}
			for (int j = 0; j < length; j++)
			{
				curIndex = i * groupCount + j;
				if (!action(maxValue, arr[curIndex]))
				{
					maxIndex = curIndex;
					maxValue = arr[curIndex];
				}
			}
			temp = arr[maxIndex];
			arr[maxIndex] = arr[i * groupCount];
			arr[i * groupCount] = temp;
			indexs[i] = new Group(i * groupCount, i * groupCount, length);
		}
		Sort(indexs, arr, action);
		for (int i = 0; i < num-1; i++)
		{
			if(indexs[i].length>1)
			{
				maxIndex = indexs[i].startIndex + 1;
				maxValue = arr[indexs[i].startIndex+1];
				for (int j=1;j< indexs[i].length; j++)
				{
					curIndex = indexs[i].startIndex + j;
					if (!action(maxValue, arr[curIndex]))
					{
						maxIndex = curIndex;
						maxValue = arr[curIndex];
					}
				}
				curIndex = indexs[i].startIndex + 1;
                temp = arr[maxIndex];
				arr[maxIndex] = arr[curIndex];
				arr[curIndex] = temp;
				tempGroup = new Group(curIndex, curIndex, indexs[i].length-1);
				for(var j=num-1;j> i;j--)
				{
                    if (!action(arr[indexs[j].startIndex], arr[tempGroup.startIndex]))
					{
						if(j+1!=num)
						{
							indexs[j + 1] = indexs[j];
                        }
					}
					else
					{
						if (j + 1 != num)
						{
							indexs[j + 1] = tempGroup;
						}
						break;
					}
				}
			}
		}
		T[] result = new T[num];
        for (int i = 0; i < num; i++)
		{
			result[i] = arr[indexs[i].startIndex];
        }
		return result;
	}
}
