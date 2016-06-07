﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugin.Interfaces;
using ZKWeb.Templating.AreaSupport;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Shopping.Order.src {
	/// <summary>
	/// 载入插件时的处理
	/// </summary>
	[ExportMany, SingletonReuse]
	public class Plugin : IPlugin {
		/// <summary>
		/// 初始化
		/// </summary>
		public Plugin() {
			// 注册默认模块
			var areaManager = Application.Ioc.Resolve<TemplateAreaManager>();
			// 商品详情页
			areaManager.GetArea("product_sales_info")
				.DefaultWidgets.Add("shopping.order.widgets/product_purchase_buttons");
		}
	}
}
