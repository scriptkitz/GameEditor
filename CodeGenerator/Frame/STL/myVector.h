﻿#ifndef _MY_VECTOR_H_
#define _MY_VECTOR_H_

#include "mySTL.h"
#include <vector>

template<typename T>
class myVector : public mySTL
{
public:
	typedef typename vector<T>::iterator iterator;
	typedef typename vector<T>::reverse_iterator reverse_iterator;
	typedef typename vector<T>::const_iterator const_iterator;
public:
	myVector(){}
	virtual ~myVector(){ clear(); }
	T* data() const { return (T*)mVector.data(); }
	uint size() const { return mVector.size(); }
	iterator begin() { return mVector.begin(); }
	iterator end() { return mVector.end(); }
	reverse_iterator rbegin() { return mVector.rbegin(); }
	reverse_iterator rend() { return mVector.rend(); }
	const_iterator cbegin() const { return mVector.cbegin(); }
	const_iterator cend() const { return mVector.cend(); }
	void push_back(const T& elem, bool check = true)
	{
#if _DEBUG
		if (check)
		{
			checkLock();
		}
#endif
		mVector.push_back(elem);
	}
	iterator erase(const iterator& iter, bool check = true)
	{
#if _DEBUG
		if (check)
		{
			checkLock();
		}
#endif
		return mVector.erase(iter);
	}
	iterator erase(uint index, bool check = true)
	{
#if _DEBUG
		if (index < 0 || index >= (int)mVector.size())
		{
			return mVector.end();
		}
		return erase(mVector.begin() + index, check);
#else
		return mVector.erase(mVector.begin() + index);
#endif
		
	}
	void clear()
	{
#if _DEBUG
		checkLock();
#endif
		mVector.clear();
	}
	void merge(const myVector<T>& other)
	{
		uint count = other.mVector.size();
		FOR_I(count)
		{
#if _DEBUG
			push_back(other[i]);
#else
			mVector.push_back(other.mVector[i]);
#endif
		}
	}
	void insert(const iterator& iter, const T& elem, bool check = true)
	{
#if _DEBUG
		if (check)
		{
			checkLock();
		}
#endif
		mVector.insert(iter, elem);
	}
	const T& operator[](uint i) const
	{
		if (i < 0 || i >= mVector.size())
		{
			// 因为函数为const属性,但是directError不是const属性,所以此处调用directError会报错
			//directError("vector index out of range!");
		}
		return mVector[i];
	}
	T& operator[](uint i)
	{
		if (i < 0 || i >= mVector.size())
		{
			directError("vector index out of range!");
		}
		return mVector[i];
	}
	void resize(uint s, bool check = true)
	{
#if _DEBUG
		if (check)
		{
			checkLock();
		}
#endif
		mVector.resize(s);
	}
	void reserve(int size)
	{
		mVector.reserve(size);
	}
	bool contains(const T& value) const
	{
		return std::find(mVector.begin(), mVector.end(), value) != mVector.end();
	}
	int find(const T& value)
	{
		uint count = mVector.size();
		FOR_I(count)
		{
			if (mVector[i] == value)
			{
				return i;
			}
		}
		return -1;
	}
	void clone(myVector<T>& target) const
	{
		target.mVector.clear();
		auto& temp = target.mVector;
		uint curSize = mVector.size();
		FOR_I(curSize)
		{
			temp.push_back(mVector[i]);
		}
#if _DEBUG
		target.resetLock();
#endif
	}
	void removeAll(T& value, bool check = true)
	{
#if _DEBUG
		if (check)
		{
			checkLock();
		}
#endif
		for (uint i = 0; i < mVector.size(); )
		{
			if (mVector[i] == value)
			{
				mVector.erase(mVector.begin() + i);
			}
			else
			{
				++i;
			}
		}
	}
	void remove(T& value, bool check = true)
	{
#if _DEBUG
		if (check)
		{
			checkLock();
		}
#endif
		uint count = mVector.size();
		FOR_I(count)
		{
			if (mVector[i] == value)
			{
				mVector.erase(mVector.begin() + i);
				break;
			}
		}
	}
	void shrink_to_fit()
	{
		mVector.shrink_to_fit();
	}
public:
	vector<T> mVector;
};

#endif