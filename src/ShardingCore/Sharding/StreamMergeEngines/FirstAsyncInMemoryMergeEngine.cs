﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShardingCore.Extensions;
using ShardingCore.Sharding.Abstractions;
using ShardingCore.Sharding.StreamMergeEngines.Abstractions;
using ShardingCore.Sharding.StreamMergeEngines.Abstractions.AbstractGenericExpressionMergeEngines;

namespace ShardingCore.Sharding.StreamMergeEngines
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/8/18 14:11:31
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class FirstAsyncInMemoryMergeEngine<TEntity>: AbstractGenericMethodCallWhereInMemoryAsyncMergeEngine<TEntity>
    {
        public FirstAsyncInMemoryMergeEngine(MethodCallExpression methodCallExpression, IShardingDbContext shardingDbContext) : base(methodCallExpression, shardingDbContext)
        {
        }

        public override TResult MergeResult<TResult>()
        {
            var result =  base.Execute(queryable =>  ((IQueryable<TResult>)queryable).First());
            var q = result.Where(o => o != null).AsQueryable();

            var streamMergeContext = GetStreamMergeContext();
            if (streamMergeContext.Orders.Any())
                return q.OrderWithExpression(streamMergeContext.Orders).First();

            return q.First();
        }

        public override async Task<TResult> MergeResultAsync<TResult>(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.ExecuteAsync( queryable =>  ((IQueryable<TResult>)queryable).FirstAsync(cancellationToken), cancellationToken);
            var q = result.Where(o => o != null).AsQueryable();

            var streamMergeContext = GetStreamMergeContext();
            if (streamMergeContext.Orders.Any())
                return q.OrderWithExpression(streamMergeContext.Orders).First();

            return q.First();
        }
    }
}
