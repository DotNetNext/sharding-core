using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sample.SqlServer.Domain.Entities;
using ShardingCore.Core.VirtualRoutes;
using ShardingCore.VirtualRoutes;
using ShardingCore.VirtualRoutes.Mods;

namespace Sample.SqlServer.Shardings
{
/*
* @Author: xjm
* @Description:
* @Date: Thursday, 14 January 2021 15:39:27
* @Email: 326308290@qq.com
*/
    public class SysUserModVirtualTableRoute : AbstractSimpleShardingModKeyStringVirtualTableRoute<SysUserMod>
    {
        /// <summary>
        /// 开启提示路由
        /// </summary>
        protected override bool EnableHintRoute => true;
        protected override bool EnableAssertRoute => true;

        public SysUserModVirtualTableRoute() : base(2,3)
        {
        }
    }
}