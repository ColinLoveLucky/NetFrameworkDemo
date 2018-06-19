using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logstash
{

    public class ConfigurationSet
    {
        ///http://blog.csdn.net/tulizi/article/details/52972824
        ///安装的时候首先要配置好Java环境
        ///安装的时候会提示超出JVM在elaticsearch ，elasticSearch 直接在管理界面直接设置java环境
        ///logstash 在setup里面设置出示set LS_HEAP_SIZE=500M 默认是1G的
        ///elasticsearch-service -help
        ///logstash --help
        ///input { 
        //        stdin { }
        //}

        //output { 
        //  elasticsearch {
        //        hosts => "172.16.36.130"
        //	    index => "test-logstash-%{+YYYY-MM}"  
        //    }
        //} 
        ///elasticsearch 在新版本中已改为hosts
        ///Ctr+z 退出命令
    }
}
