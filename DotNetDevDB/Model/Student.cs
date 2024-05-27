using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDevDB.Model {
    [Table("Students")]
    public class Student {

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// 艺名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        ///// <summary>
        ///// 🐖猪
        ///// </summary>
        //[NotMapped]
        //public DateTime Code { get; set; }
    }
}
