
/// <summary>
/// упрощённый аналог словаря Dictionary<string, int>, c использованием только примитивных типов и массивов
/// </summary>
public class PrimitiveMap
{
    private readonly int initialSize = 4;

    //the partition index is hash%storageSize and its value is index of a storage cell
    private int[] partitions;

    //the storage, filled in from start
    private Data[] storage;

    private struct Data
    {
        public int hash;
        public string? key;
        public int value;
        //in case of hash collision points to next present node
        public int next;

        public int partition;//for debug

        public override string ToString()
        {
            return $"{partition}->{key}:{value}->{next}({hash})";
        }
    }

    private int count;
    public int Count
    {
        get { return count - freeCount; }
    }

    //the removed stays in store as chain
    private int freeIndex = -1;
    private int freeCount = 0;

    public void Put(string key, int value)
    {
        if (key == null) throw new ArgumentException("the key could not be null", nameof(key));

        int hash = key.GetHashCode();

        //items should be partitioned in storage by index to take O(n) time for access
        int partition = Math.Abs(hash) % storage.Length;
        int point = partitions[partition];

        //partition may point to a chain of datas, we need its end
        while (point >= 0)
        {
            //when we get some item in storage already, just update
            if (hash == storage[point].hash && key.Equals(storage[point].key))
            {
                storage[point].value = value;
                return;
            }
            if (storage[point].next <= 0)
                break; 
            point = storage[point].next;
        }

        //point is to the last thing from the partition or -1

        //lets figure out where to place new one
        int index;

        //can be some free space at the moment, 
        if (freeCount > 0)
        {
            //lets use free space
            index = freeIndex;
            //move free chain
            freeCount--;
            freeIndex = storage[index].next; //eventually should become -1
        }
        //no free space -- put to an end
        else
        {
            index = count;
            //resize may be in order
            if (count >= storage.Length)
            {
                //if nowhere to add new data it is recreated twice as big
                Resize(storage.Length * 2);
                //partition can be changed after the resize 
                partition = Math.Abs(hash) % storage.Length;
            }
            count++;
        }

        //put item in place,
        storage[index] = new Data
        {
            hash = hash,
            key = key,
            value = value,
            next = -1,
            partition = partition
        };

        //and make proper point for future
        if (point >= 0)
        {
            storage[point].next = index;
        }
        else
        {
            partitions[partition] = index;
        }
    }


    public PrimitiveMap(int size)
    {
        var s = Math.Max(size, initialSize);
        partitions = new int[s];
        Array.Fill(partitions, -1);
        storage = new Data[s];
    }

    private void Resize(int size)
    {
        //make new storage and new partitions
        var newPartitions = new int[size];
        var newStorage = new Data[size];
        Array.Fill(newPartitions, -1);
        Array.Copy(storage, newStorage, storage.Length);

        //recalc partitions for storage
        for (int i = 0; i < storage.Length; i++)
        {
            var partition = Math.Abs(newStorage[i].hash) % size;
            var point = newPartitions[partition];

            //the item came at root of new index
            if (-1 == point)
            {
                newPartitions[partition] = i;
                newStorage[i].partition = partition;
                newStorage[i].next = -1;
            }
            else
            {
                //the partition already points somewhere, lets find the chain end
                while (newStorage[point].next >= 0)
                {
                    point = newStorage[point].next;
                }
                newStorage[point].next = i;
                newStorage[i].partition = partition;
                newStorage[i].next = -1;
            }
        }

        partitions = newPartitions;
        storage = newStorage;
    }

    public bool TryGet(string key, out int value)
    {
        if (key == null) throw new ArgumentException("the key could not be null", nameof(key));

        int hash = key.GetHashCode();

        //just get partition and find item in chain
        int point = partitions[Math.Abs(hash) % storage.Length];

        while (point >= 0)
        {
            //when we get some item in storage already, just update
            if (hash == storage[point].hash && key.Equals(storage[point].key))
            {
                value = storage[point].value;
                return true;
            }
            point = storage[point].next;
        }

        value = -1;
        return false;
    }

    public bool Remove(string key)
    {
        if (key == null) throw new ArgumentException("the key could not be null", nameof(key));

        int hash = key.GetHashCode();

        int partition = Math.Abs(hash) % storage.Length;
        int point = partitions[partition];

        int prevPoint = -1;
        while (point >= 0)
        {
            if (hash == storage[point].hash && key.Equals(storage[point].key))
            {
                //here storage[point] is the correct element we should remove

                //remove from chain middle
                if (prevPoint == -1)
                {
                    partitions[partition] = storage[point].next;
                }
                else
                {
                    storage[prevPoint].next = storage[point].next;
                }

                //make it empty in storage
                storage[point].key = null;
                storage[point].value = default;
                storage[point].next = -1;

                //put to free chain
                if (freeIndex < 0)
                {
                    freeIndex = point;
                }
                else
                {
                    int lastIndex = freeIndex;
                    //lets find the last removed el
                    while (storage[lastIndex].next > 0)
                    {
                        lastIndex = storage[lastIndex].next;
                    }
                    storage[lastIndex].next = point;
                }
                freeCount++;

                return true;
            }
            prevPoint = point;
            point = storage[point].next;
        }

        return false;
    }
}