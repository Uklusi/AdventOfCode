# Important aliases
* `map` is called `Select`
* `reduce` is called `Aggregate`
* `filter` is called `Where`

# Comparers.cs
* `Reverse<T>()`: used when sorting to sort in reverse order
* `ReverseComparer(Comparer)`: sort in revers order wrt Comparer
* `ComparerFactory.Create(comparerFunction)`: Creates a new Comparer using the function (f(x, y) = -1 means x < y)

# Extensions.cs

## IEnumerable
* `Empty()` and `NotEmpty()` are the obvious.
* `All()` is `Aggregate(&&)`.
* `AnyBool()` is `Aggregate(||)`. Unfortunately `Any()` already exists
and checks if the Enumerable has any value `-.-'`
* `Enumerate()` is the obvious.
* `Enumerate2D()` takes a double iterable and Enumerates it using `Point`
* `SelectTuple(f)` is a `Select` that takes a function with two parameters and uses it as a function acting on a tuple.
* `ZipSelect(other, f)` applies `Zip` then `SelectTuple`
* `ComponentEquals(other)` checks is the two containers are equals component by component.
* `Flatten()` does the same as in python
* `SelectInner(f)` applies the function on the inner iterables (`this` must be iterable of iterables)
* `Product(other)` is cartesian product.

## DoubleArrayExtensions
* `GetRows()` takes a 2D array `T[a,b]` and gives an iterable of rows (from the top)
* `GetCols()` does the same but for the columns

## EnumerableConversionExtensions
* `ToJaggedArray()` -> T[][]
* `ToNestedList()`
* `ToDoubleArray()` -> T\[,\]
* `ToDoubleArrayFromCols()` = `ToDoubleArray().Transpose()`
* `JoinString(s)` = `string.Join(s, this)`. No arg means `s = ""`
* `ToStringEnum()` = `.Select(e => e.ToString())`
* `ToStringArray()`
* `ReprList()` = list as text
* `ReprNestedList()`, `ReprDict()`, and `Inline` versions.

## OtherExtensions
* `ToInt`, `ToLong`, `ToBool`
* `IsIn(list)` = `list.Contains(this)`
* `IsInInterval(a, b)` = `a <= this <= b`
* `Copy()` shallow copies lists and arrays

# Miscellaneous
* `SOLID, FULL, EMPTY, PATH`
* `Mod, Gcd`
* `Arr(a, b, ...) = {a, b, ...}`
* `Repeat(x, 100)` = `{x, x, x, ...}`
* `RepeatFlat({x, y}, 100)` = `{x, y, x, y, ...}`
* `Zip(a, b)` = `a.Zip(b)`
* `IntRange` = python `range`
* `DefaultDictionary`
* `Counter` class: an `int` defaultdict with `SetMax` and `SetMin` 


