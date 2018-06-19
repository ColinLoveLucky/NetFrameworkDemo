using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{

    /// <summary>
    /// 模块（菜单）权限
    /// </summary>
    [Serializable]
    public class APP_Menu
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 模块菜单主键
        /// </summary>
        public string MenuId { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 模块菜单名称
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Img { get; set; }
        /// <summary>
        /// 链接WEB URL
        /// </summary>
        public string NavigateUrl { get; set; }
        /// <summary>
        /// Winfrom 链接地址
        /// </summary>
        public string FormName { get; set; }
        /// <summary>
        /// 链接目标
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 是否展开
        /// </summary>
        public string IsUnfold { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public int AllowDisplay { get; set; }

        /// <summary>
        /// 模块所拥有的按钮
        /// </summary>
        public List<APP_Button> Button { get; set; }
    }
    [Serializable]
    public class APP_Button
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 模块主键
        /// </summary>
        public string MenuId { get; set; }
        /// <summary>
        /// 按钮主键
        /// </summary>
        public string ButtonId { get; set; }
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 按钮图标
        /// </summary>
        public string Img { get; set; }
        /// <summary>
        /// 按钮事件
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 按钮控件ID
        /// </summary>
        public string Control_ID { get; set; }
        /// <summary>
        /// 按钮分类
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 是否分割
        /// </summary>
        public string Split { get; set; }
        /// <summary>
        /// 按钮描述
        /// </summary>
        public string Description { get; set; }

    }
}
