using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;

namespace QK.QAPP.IServices
{
    public partial interface IAPP_CARPRICESERVICE
    {
        /// <summary>
        /// 分页查询车辆信息列表
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <returns></returns>
        ViewListByPage<APP_CARPRICE> GetCarList(CarListSearchPara para, int pageIndex, int pageSize);

        /// <summary>
        /// 从Excel导入数据
        /// </summary>
        /// <param name="request">Http请求</param>
        /// <returns>提示信息（包含成功或错误提示）</returns>
        string ImportFromExcel(System.Web.HttpRequestBase request);

        /// <summary>
        /// 删除表中所有数据
        /// </summary>
        /// <returns></returns>
        string DeleteAll();

        /// <summary>
        /// 批量导入数据（通过SQL实现）
        /// </summary>
        /// <param name="list">数据集合</param>
        void AddMultipleBySql(List<APP_CARPRICE> list);
    }
}
