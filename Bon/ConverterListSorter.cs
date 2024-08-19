using Bon.Converters;

namespace Bon
{

    public class ConverterListSorter
    {
        private readonly List<(int start, int count)> _sequences = [];
        private readonly List<int> _skips = [];
        private readonly List<Type> _types = [];
        private readonly List<IBonConvertible> _main = [];
        private const int UNKNOWN = -1;

        public ConverterListSorter(IEnumerable<IBonConvertible> unsorted)
        {
            _main.AddRange(unsorted);
            _types.AddRange(unsorted.Select(u => u.Type));
        }

        public IEnumerable<IBonConvertible> Sort()
        {
            int index = 0;
            while (index < _main.Count)
            {
                if (_skips.Contains(index))
                {
                    index++;
                    continue;
                }
                int parentIndex = GetStraightParentIndex(index);
                if (parentIndex == UNKNOWN)
                {
                    _skips.Add(index);
                    index++;
                    continue;
                }
                var seqIndex = GetSequenceIndex(parentIndex);
                var selfSeqIndex = GetSequenceIndex(index);
                if (seqIndex == UNKNOWN)
                {
                    seqIndex = CreateSequence(parentIndex);
                }
                if (selfSeqIndex == UNKNOWN && parentIndex > UNKNOWN)
                {
                    var parents = GetParrents(seqIndex); 
                    var factNewIndex = Move(index, parentIndex, parents);
                    _skips.Add(factNewIndex);
                }
                else if(selfSeqIndex != UNKNOWN)
                {
                    MoveSequence(selfSeqIndex, seqIndex);
                }
            }
            return _main;
        }

        private int GetStraightParentIndex(int childIndex)
        {
            var childType = _types[childIndex];
            //hops - count of base types from type by typeIndex to childType
            var derivations = new List<(int hops, int typeIndex)?>();
            for (int i=0;i<_main.Count;i++)
            {
                if (i == childIndex)
                {
                    continue;
                }
                var type = _types[i];
                var hops = GetHopsTo(childType,type);

                if (hops.HasValue)
                {
                    derivations.Add((hops.Value, i));
                }
            }

            var min = derivations.OrderBy(val => val?.hops).FirstOrDefault();
            
            return min is null ? UNKNOWN : min.Value.typeIndex;
        }

        private int? GetHopsTo(Type childType, Type type)
        {
            int hops = 0;

            while (childType != null && childType != type)
            {
                childType = childType.BaseType;
                hops++;
            }

            return childType is null ? null : hops;
        }

        private int Move(int index0, int index1, int[] parents)
        {
            var type = _types[index0];
            var convertible = _main[index0];
            _types.Insert(index1, type);
            _main.Insert(index1, convertible);
            var localDelta = index0 > index1 ? 1 : 0;
            var factDelta = index0 > index1 ? 0 : 1;
            _types.RemoveAt(index0+localDelta);
            _main.RemoveAt(index0+localDelta);

            index1 -= factDelta;
            int delta = index0 > index1 ? 1 : -1;

            for (int skipIndex = 0; skipIndex < _skips.Count; skipIndex++)
            {
                int skip = _skips[skipIndex];
                if (index1 <= skip && skip < index0 || index0 < skip && skip <= index1)  
                {
                    _skips[skipIndex] += delta;
                }
            }

            for(int seqIndex = 0; seqIndex < _sequences.Count; seqIndex++)
            {
                var (start, count) = _sequences[seqIndex];
                if (index1 <= start && start < index0 || index0 < start && start <= index1)
                {
                    _sequences[seqIndex] = (start + delta, count);
                }
                if (parents.Contains(seqIndex))
                {
                    (start, count) = _sequences[seqIndex];
                    _sequences[seqIndex] = (start, count+1);
                }
            }

            return index1;
        }
        private void MoveSequence(int seqIndex, int parentSeqIndex)
        {
            var (start,count) = _sequences[seqIndex];
            var (dest, _) = _sequences[parentSeqIndex];
            var parents = GetParrents(parentSeqIndex);
            for(int x = 0; x < count; x++)
            {
                Move(start, dest, parents);
                (start, _) = _sequences[seqIndex];
            }
        }
        private int CreateSequence(int startIndex)
        {
            _sequences.Add((startIndex, 1));
            return _sequences.Count - 1;
        }
        private int GetSequenceIndex(int start)
        {
            for (int index=0;index<_sequences.Count;index++)
            {
                var seq = _sequences[index];
                if (seq.start == start)
                {
                    return index;
                }
            }

            return UNKNOWN;
        }
        private int[] GetParrents(int rootParent)
        {
            var parents = new List<int>() { rootParent };
            var (rootStart,_) = _sequences[rootParent];

            for (int index = 0; index < _sequences.Count; index++)
            {
                var (start,count) = _sequences[index];
                if (rootStart < start && start - count + 1 < rootStart)
                {
                    parents.Add(index);
                }
            }

            return parents.ToArray();
        }
    }
}
