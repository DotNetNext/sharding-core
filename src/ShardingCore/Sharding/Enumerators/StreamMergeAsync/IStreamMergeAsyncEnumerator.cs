using System.Collections.Generic;

namespace ShardingCore.Sharding.Enumerators
{
/*
* @Author: xjm
* @Description:
* @Date: Saturday, 14 August 2021 21:21:44
* @Email: 326308290@qq.com
*/
    public interface IStreamMergeAsyncEnumerator<T>:IAsyncEnumerator<T>
    {
        bool SkipFirst();
        bool HasElement();
        T ReallyCurrent { get; }
        
    }
}