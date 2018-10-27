//----------------------------------------------------------------------------------------
//	Copyright © 2006 - 2018 Tangible Software Solutions, Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to convert some of the C++ std::vector methods to C#.
//----------------------------------------------------------------------------------------

using System.Collections.Generic;

internal static class VectorHelper
{
	public static void Resize<T>(this List<T> list, int newSize, T value = default(T))
	{
		if (list.Count > newSize)
			list.RemoveRange(newSize, list.Count - newSize);
		else if (list.Count < newSize)
		{
			for (int i = list.Count; i < newSize; i++)
			{
				list.Add(value);
			}
		}
	}

	public static void Swap<T>(this List<T> list1, List<T> list2)
	{
		List<T> temp = new List<T>(list1);
		list1.Clear();
		list1.AddRange(list2);
		list2.Clear();
		list2.AddRange(temp);
	}

	public static List<T> InitializedList<T>(int size, T value)
	{
		List<T> temp = new List<T>();
		for (int count = 1; count <= size; count++)
		{
			temp.Add(value);
		}

		return temp;
	}

	public static List<List<T>> NestedList<T>(int outerSize, int innerSize)
	{
		List<List<T>> temp = new List<List<T>>();
		for (int count = 1; count <= outerSize; count++)
		{
			temp.Add(new List<T>(innerSize));
		}

		return temp;
	}

	public static List<List<T>> NestedList<T>(int outerSize, int innerSize, T value)
	{
		List<List<T>> temp = new List<List<T>>();
		for (int count = 1; count <= outerSize; count++)
		{
			temp.Add(InitializedList(innerSize, value));
		}

		return temp;
	}
}