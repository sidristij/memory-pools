using System;
using System.Runtime.CompilerServices;

namespace MemoryPools.Memory
{
	internal class JetStack<T>
    {
    	private ObjectWrapper[] _array;
    	
    	private int _size;
    	private T _firstItem;
    
    	private const int DefaultCapacity = 4;

    	public JetStack()
    	{
    		_array = new ObjectWrapper[DefaultCapacity];
    	}

    	// Create a stack with a specific initial capacity.  The initial capacity
    	// must be a non-negative number.
    	public JetStack(int capacity)
    	{
    		_array = new ObjectWrapper[capacity];
    	}

    	public int Count => _size;
        
    	// Removes all Objects from the Stack.
    	[MethodImpl(MethodImplOptions.AggressiveInlining)]
    	public void Clear()
    	{
    		Array.Clear(_array, 0, _size); // Don't need to doc this but we clear the elements so that the gc can reclaim the references.
    		_size = 0;
    	}

    	[MethodImpl(MethodImplOptions.AggressiveInlining)]
    	public T Peek() => _array[_size - 1].Element;

    	// Pops an item from the top of the stack.  If the stack is empty, Pop
    	// throws an InvalidOperationException.
    	[MethodImpl(MethodImplOptions.AggressiveInlining)]
    	public T Pop()
    	{
    		var item = _firstItem;
    		if (_firstItem != null)
    		{
    			_firstItem = default;
    			return item;
    		}
    		return _array[--_size].Element;
    	}

    	// Pushes an item to the top of the stack.
    	[MethodImpl(MethodImplOptions.AggressiveInlining)]
    	public void Push(T item)
    	{
    		if (_firstItem == null)
    		{
    			_firstItem = item;
    			return;
    		}
    		if (_size >= _array.Length)
    		{
    			PushWithResize(item);
    		}
    		else
    		{
    			_array[_size++].Element = item;
    		}
    	}

    	// Non-inline from Stack.Push to improve its code quality as uncommon path
    	[MethodImpl(MethodImplOptions.NoInlining)]
    	private void PushWithResize(T item)
    	{
    		Array.Resize(ref _array, _array.Length<<1);
    		_array[_size].Element = item;
    		_size++;
    	}
    	
    	// PERF: the struct wrapper avoids array-covariance-checks from the runtime when assigning to elements of the array.
    	private struct ObjectWrapper
    	{
    		public T Element;
    	}
    }
}