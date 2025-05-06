using System;

namespace WPFMVVMDemo.Models
{
    /// <summary>
    /// 产品数据模型
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 获取或设置产品ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置产品价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 获取或设置产品数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 获取或设置产品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置产品类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 获取或设置产品上架日期
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 获取或设置产品是否有货
        /// </summary>
        public bool IsInStock { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Product()
        {
            CreatedDate = DateTime.Now;
            IsInStock = true;
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public Product(int id, string name, decimal price, int quantity, string category = null, string description = null)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
            Description = description;
            CreatedDate = DateTime.Now;
            IsInStock = quantity > 0;
        }
    }
} 