using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShardingCore.Sharding.Enumerators.StreamMergeAsync
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Sunday, 15 August 2021 06:39:52
    * @Email: 326308290@qq.com
    */
    public class PaginationStreamMergeAsyncEnumerator<T> : IStreamMergeAsyncEnumerator<T>
    {
        private readonly StreamMergeContext<T> _mergeContext;
        private readonly IStreamMergeAsyncEnumerator<T> _enumerator;
        private readonly int? _skip;
        private readonly int? _take;
        private int realSkip = 0;
        private int realTake = 0;

        public PaginationStreamMergeAsyncEnumerator(StreamMergeContext<T> mergeContext, IEnumerable<IStreamMergeAsyncEnumerator<T>> sources)
        {
            _mergeContext = mergeContext;
            _skip = mergeContext.Skip;
            _take = mergeContext.Take;
            if (_mergeContext.HasGroupQuery())
                _enumerator = new MultiAggregateOrderStreamMergeAsyncEnumerator<T>(_mergeContext, sources);
            else
                _enumerator = new MultiOrderStreamMergeAsyncEnumerator<T>(_mergeContext, sources);
        }
#if !EFCORE2
        public async ValueTask<bool> MoveNextAsync()
#endif
#if EFCORE2
        public async Task<bool> MoveNext(CancellationToken cancellationToken = new CancellationToken())
#endif
        {
            //如果合并数据的时候不需要跳过也没有take多少那么就是直接next
            while (_skip.GetValueOrDefault() > this.realSkip)
            {
#if !EFCORE2
                var has = await _enumerator.MoveNextAsync();
#endif
#if EFCORE2
                var has = await _enumerator.MoveNext(cancellationToken);
#endif

                realSkip++;
                if (!has)
                    return false;
            }

#if !EFCORE2
            var next = await _enumerator.MoveNextAsync();
#endif
#if EFCORE2
            var next = await _enumerator.MoveNext(cancellationToken);
#endif

            if (next)
            {
                if (_take.HasValue)
                {
                    realTake++;
                    if (realTake > _take.Value)
                        return false;
                }
            }

            return next;
        }

        public T Current => _enumerator.Current;
        public bool SkipFirst()
        {
            return _enumerator.SkipFirst();
        }

        public bool HasElement()
        {
            return _enumerator.HasElement();
        }

        public T ReallyCurrent => _enumerator.ReallyCurrent;
#if !EFCORE2

        public ValueTask DisposeAsync()
        {
            return _enumerator.DisposeAsync();
        }
#endif
#if EFCORE2

        public void Dispose()
        {
            _enumerator.Dispose();
        }
#endif
    }
}