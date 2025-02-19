using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sample.SqlServer.DbContexts;
using Sample.SqlServer.Domain.Entities;
using ShardingCore;

namespace Sample.SqlServer
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Tuesday, 26 January 2021 12:29:04
    * @Email: 326308290@qq.com
    */
    public static class DIExtension
    {
        public static IApplicationBuilder UseShardingCore(this IApplicationBuilder app)
        {
            var shardingBootstrapper = app.ApplicationServices.GetRequiredService<IShardingBootstrapper>();
            shardingBootstrapper.Start();
            return app;
        }

        public static void DbSeed(this IApplicationBuilder app)
        {
            using (var scope=app.ApplicationServices.CreateScope())
            {
                var virtualDbContext =scope.ServiceProvider.GetService<DefaultShardingDbContext>();
                if (!virtualDbContext.Set<SysUserMod>().Any())
                {
                    var ids = Enumerable.Range(1, 1000);
                    var userMods = new List<SysUserMod>();
                    var SysTests = new List<SysTest>();
                    foreach (var id in ids)
                    {
                        userMods.Add(new SysUserMod()
                        {
                            Id = id.ToString(),
                            Age = id,
                            Name = $"name_{id}",
                        });
                        SysTests.Add(new SysTest()
                        {
                            Id = id.ToString(),
                            UserId = id.ToString()
                        });
                    }

                    virtualDbContext.AddRange(userMods);
                    virtualDbContext.AddRange(SysTests);
                    virtualDbContext.SaveChanges();
                }
            }
        }
    }
}