using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public partial interface IAPP_MSGBOX_USERSERVICE : IRepositoryBase<APP_MSGBOX_USER> 
    {
        /// <summary>
        /// 添加在线用户
        /// </summary>
        /// <param name="UserName">用户登录名</param>
        /// <param name="ConnectionId">浏览器唯一ID</param>
        void AddConnection(string UserName, string ConnectionId);
        /// <summary>
        /// 移除当前用户
        /// </summary>
        /// <param name="ConnectionId"></param>
        void RemoveConnection(string ConnectionId);
    }
}
