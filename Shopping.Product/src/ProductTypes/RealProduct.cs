﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Product.src.ProductTypes {
	/// <summary>
	/// 实体商品
	/// </summary>
	[ExportMany]
	public class RealProduct : IProductType {
		/// <summary>
		/// 商品类型
		/// </summary>
		public string Type { get { return "RealProduct"; } }
	}
}
