using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FList<T> : IEnumerable<T> {
	readonly IEnumerable<T> collection;
		
	//Use FList.Create instead of this constructor directly
	public FList() {
		collection = new T[0];
	}

	//Use FList.Create instead of this constructor directly
	public FList(T singleValue) {
		collection = new T[1] { singleValue };
	}
		
	//Use FList.Cast instead of this constructor directly
	public FList(IEnumerable<T> collection) {
		this.collection = collection;
	}

	//Specificity to resolve ambiguity between FList-IEnumerable
	public static FList<T> operator+(FList<T> lhs, FList<T> rhs) {
		return FList.Cast(lhs.collection.Concat(rhs.collection));
	}

	public static FList<T> operator+(FList<T> lhs, IEnumerable<T> rhs) {
		return FList.Cast(lhs.collection.Concat(rhs));
	}

	public static FList<T> operator+(IEnumerable<T> lhs, FList<T> rhs) {
		return FList.Cast(FList.Cast(lhs).collection.Concat(rhs));
	}

	//This isn't semantically correct, but it's cleaner to read.
	public static FList<T> operator+(FList<T> lhs, T rhs) {
		return lhs + FList.Create(rhs);
	}

	//This isn't semantically correct, but it's cleaner to read.
	public static FList<T> operator+(T lhs, FList<T> rhs) {
		return FList.Create(lhs) + rhs;
	}
		
	public IEnumerator<T> GetEnumerator() {
		foreach(var element in collection)
			yield return element;
	}
		
	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}
}

public static class FList {
	public static FList<T> ToFList<T>(this IEnumerable<T> lhs) {
		return Cast(lhs);
	}

	public static FList<T> Create<T>() {
		return new FList<T>();
	}

	public static FList<T> Create<T>(T singleValue) {
		return new FList<T>(singleValue);
	}
		
	public static FList<T> Cast<T>(IEnumerable<T> collection) {
		return new FList<T>(collection);
	}
}
