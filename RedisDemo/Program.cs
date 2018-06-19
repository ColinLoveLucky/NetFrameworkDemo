using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ///http://www.yiibai.com/redis/redis_quick_guide.html
            ///http://blog.csdn.net/zyz511919766/article/details/42268219
            /////http://www.runoob.com/redis/server-client-kill.html
            ///http://blog.csdn.net/tianmangshan80/article/details/51704974
            ///https://rubygems.org/gems/redis/versions/3.2.2 
            ///http://www.cnblogs.com/tommy-huang/p/6240083.html
            ///http://doc.redisfans.com/topic/cluster-tutorial.html#id5
            ///http://www.cnblogs.com/mantian2020/p/6239264.html
            ///redis-cli -h 172.16.36.130 -p 6379
            ///redis-cli -c -p 6379 -h 172.16.36.130
            ///http://www.cnblogs.com/gossip/p/5993922.html
            ///http://blog.csdn.net/mindfloating/article/details/50458768
            ///http://blog.csdn.net/aquester/article/details/50150163 redis-trib.rb cluster 命令
            //DataNode 
            //CLuster Slot
            //slot Key 
            //slot DataNode
            //cluster myid
            //cluster nodes
            //cluster info
            //迁移slot如果有内容会出问题
            //如果想要添加slot，首先的条件是必须不被节点引用，条件就是删除掉，然后在添加上去
            //如果slot有所遗漏，在会出现问题整个集群
            //注意不要用cluster来分配，迁移，这样会出现问题，slot也不成功
            //若果要slot分配节点，可以通过redis-trib 的命令来操作哈
            //http://blog.csdn.net/huwei2003/article/details/50973967 redis-trib 命令
            //http://blog.csdn.net/xu470438000/article/details/42972123
            //http://blog.csdn.net/gqtcgq/article/details/51722852
            //删除节点之后，如果需要重新配置上去，则需要.aof 与nodes.conf 文件，然后在此重新执行add-node操作
            //把节点设置为从节点 =》进入要设置的节点=》cluster replicate 主节点nodeId
            //如果主节点挂掉，配置的从节点会自动升级为主节点哈，再次回复，依然为从节点，只不过有原来的主节点，变为从节点哈
        }

    }
}
