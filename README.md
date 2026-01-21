Architect and develop a primitive copy of Dictionary<string, int>, using only primitive types.\
.NET collections should not be used (List<T>, Dictionary<TKey,TValue>, HashSet<T> and others).\
\
New class PrimitiveMap, perhaps, should have the following interface:\
void Put(string key, int value) -- Adds new key-value pair. Updates the value if the key is already in map.\
bool TryGet(string key, out int value) -- Returns true if the key was found, and sets value. Returns false if key is not present.\
bool Remove(string key) -- Removes the pair by key, returns true if the element was found and removed.\
int Count { get; } -- Amount of stored elements.

Restrictions and conditions:
- Internal implemenation may consist only arrays of primitive types and strings, aryphmetical operations, cycles, conditions.
- Should not be used: collections from System.Collections / System.Collections.Generic.
- Keys are strings, values are int.

A structure should:
- grow when overflow (dynamic array growth or rehashing),
- provide median access time better than a linear traversal (i.e. some hash algorythm is required and should be a collision resolution strategy).

Thought-through collision resolution strategy (for example, Open Addressing or chains on index arrays).\
Think over structure behaviour when:
- adding already stored key,
- removing not stored key,
- multiple inserts and deletions should have some strategy to stop performance degradation.
