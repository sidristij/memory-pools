# Memory pools

Contains 2 groups of API. The first one is basic API for pooling any type of objects (who can be created via default ctor) and buffers (IMemoryOwner<T> with incoming links countdown mechanism).

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

This code will not produce memory traffic.

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
This code will not produce memory traffic.

## Non-allocating LINQ

Just add `.AsPooling()` to any `IEnumerable` and continue to write like regular LINQ code:

```csharp
while(!Console.KeyAvailable)
    foreach (var i in PoolingEnumerable.Range(10, 10).Concat(PoolingEnumerable.Range(10, 10)).Intersect(PoolingEnumerable.Range(15, 5)))
    {
      // ...
    }
```
This code will not produce memory traffic.
