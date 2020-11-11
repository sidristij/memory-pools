![Project Build](https://github.com/sidristij/memory-pools/workflows/.NET%20Core/badge.svg)
[![Nuget](https://img.shields.io/nuget/v/MemoryPools?logo=MemoryPools)](https://www.nuget.org/packages/MemoryPools/)

# Memory pools

Contains 2 groups of API. The first one is basic API for pooling any type of objects (who can be created via default ctor) and buffers (IMemoryOwner<T> with incoming links countdown mechanism). The second one - collections and LINQ without traffic allocations.

## Basic API

### Objects pooling

```csharp

public class Foo
{
    public Foo Init(int val)
    {
        SomeValue = val;
    }
    
    public int SomeValue { get; private set; }
}

while(true)
{
  var instance = Pool.Get<Foo>().Init(100);
  // instance.SomeValue == 100 => true;
  Pool.Return(instance);
}

```
*This code will not produce memory traffic.*

### Buffers pooling

```csharp

public class Foo : IDisposable
{
    public Foo Init(iIMemoryOwner<char> val)
    {
        SomeValue = val.AddOwner();
    }
    
    public IMemoryOwner<char> SomeValue { get; private set; }
    
    public void Dispose()
    {
        SomeValue?.Dispose();
        SomeValue = default;
        Pool.Return(this);
    }
}

while(true)
{
  using var instance = Pool.Get<Foo>().Init(buffer);
  Console.WriteLine(instance.Memory);
}

```
*This code will not produce memory traffic.*

## Non-allocating LINQ

You can start using `PoolingEnumerable.Range()` or `PoolingEnumerable.Retry()`

```csharp
while(!Console.KeyAvailable)
    foreach (var i in PoolingEnumerable.Range(10, 10).Concat(PoolingEnumerable.Range(10, 10)).Intersect(PoolingEnumerable.Range(15, 5)))
    {
      // ...
    }
```
*This code will not produce memory traffic.*

Or just add `.AsPooling()` to any `IEnumerable` and continue to write like regular LINQ code:


```csharp
while(!Console.KeyAvailable)
    foreach (var i in list.AsPooling().Concat(PoolingEnumerable.Range(10, 10)).Intersect(PoolingEnumerable.Range(15, 5)))
    {
      // ...
    }
```
*This code will not produce memory traffic.*

## Traffic-free collections

Tired to think about memory traffic in `List<T>`, `Dictionary<TKey, TValue>`, `Stack<T>()` or `Queue<T>`?

In this library you can find Pooling versions of these collections. All of them have regular version and *Canon version. Regular version use `IMemoryOwner<T>.Length=128` to store data, but *Canon version use `IMemoryOwner<object>.Length=128` to store data. 

This means that when you want to store value types (structs, primitives), you should use regular version to avoid boxing. But in case of reference types (i.e. classes) *Canon version is preferred. Single cast for getting item isn't so expensive, but internal chunks can be reused in other collections of Pooling family to store items of different types.

In other words, when you create `PoolingList<Boo>` and fill it with 256 items (2 internal chunks with 128 items in each). After this you clean it or dispose it to return internal chanks to pool. And if you'll create... `PoolingStack<Foo>`, for example, you'll get exactly the same chunks from the pool. Reuse = 0 traffic for GC.
