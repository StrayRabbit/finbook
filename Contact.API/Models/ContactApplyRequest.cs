using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Contact.API.Models
{
    [BsonIgnoreExtraElements]
    public class ContactApplyRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 工作职位
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 申请人Id
        /// </summary>
        public int ApplierId { get; set; }
        /// <summary>
        /// 是否通过 0未通过 1已通过
        /// </summary>
        public int Approvaled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ApplyTime { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime HandleTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
