﻿using Microsoft.EntityFrameworkCore;
using Sample.SqlServer.Domain.Maps;
using ShardingCore.Core.VirtualRoutes.RouteTails.Abstractions;
using ShardingCore.Sharding.Abstractions;

namespace Sample.SqlServer.DbContexts
{
    public class DefaultTableDbContext: DbContext, IShardingTableDbContext
    {
        public DefaultTableDbContext(DbContextOptions<DefaultTableDbContext> options) :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new SysUserModMap());
            modelBuilder.ApplyConfiguration(new SysTestMap());
        }

        public IRouteTail RouteTail { get; set; }
    }
}
