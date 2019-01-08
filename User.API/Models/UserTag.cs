using System;

namespace User.API.Models
{
    public class UserTag
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
