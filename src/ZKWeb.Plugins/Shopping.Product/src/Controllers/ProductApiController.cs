﻿using System;
using System.Collections.Generic;
using System.Linq;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWebStandard.Ioc;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web;
using ZKWebStandard.Collection;
using ZKWebStandard.Web;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Services;
using ZKWeb.Plugins.Shopping.Product.src.Domain.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataConditionBinders.Bases;
using ZKWeb.Plugins.Shopping.Product.src.UIComponents.ProductMatchedDataAffectsBinders.Bases;
using ZKWeb.Plugins.Shopping.Product.src.Components.ProductMatchedDataMatchers.Interfaces;
using ZKWeb.Plugins.Common.GenericClass.src.Domain.Services;
using ZKWeb.Plugins.Common.GenericClass.src.Domain.Entities;
using ZKWeb.Plugins.Common.GenericTag.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Base.src.Controllers.Extensions;

namespace ZKWeb.Plugins.Shopping.Product.src.Controllers {
	/// <summary>
	/// Api控制器
	/// </summary>
	[ExportMany]
	public class ProductApiController : ControllerBase {
		/// <summary>
		/// 获取指定类目对应的属性编辑器
		/// 这里仅用于获取编辑器Html，不绑定数据也不检查权限
		/// </summary>
		/// <returns></returns>
		[Action("api/product/property_editor")]
		public IActionResult PropertyEditor() {
			var categoryId = Request.Get<Guid>("categoryId");
			var categoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
			// 获取类目
			var category = categoryManager.GetWithCache(categoryId);
			if (category == null) {
				return new PlainResult("");
			}
			// 获取销售和非销售属性的Html列表
			var salesProperties = new List<HtmlString>();
			var nonSalesProperties = new List<HtmlString>();
			foreach (var property in category.OrderedProperties()) {
				var html = property.GetEditHtml(category);
				(property.IsSalesProperty ? salesProperties : nonSalesProperties).Add(html);
			}
			return new TemplateResult("shopping.product/property_editor.html",
				new { salesProperties, nonSalesProperties });
		}

		/// <summary>
		/// 获取类目对应的商品匹配数据的绑定器列表
		/// 这里仅用于获取绑定器的Json，不绑定数据也不检查权限
		/// </summary>
		/// <returns></returns>
		[Action("api/product/matched_data_binders")]
		public IActionResult MatchedDataBinders() {
			var categoryId = Request.Get<Guid?>("categoryId");
			var conditionBinders = Application.Ioc
				.ResolveMany<ProductMatchedDataConditionBinder>()
				.Where(b => b.Init(categoryId))
				.OrderBy(b => b.DisplayOrder).ToList();
			var affectsBinders = Application.Ioc
				.ResolveMany<ProductMatchedDataAffectsBinder>()
				.Where(b => b.Init(categoryId))
				.OrderBy(b => b.DisplayOrder).ToList();
			return new JsonResult(new {
				ConditionBinders = conditionBinders,
				AffectsBinders = affectsBinders
			});
		}

		/// <summary>
		/// 获取商品信息
		/// 不存在时返回null
		/// </summary>
		/// <returns></returns>
		[Action("api/product/info", HttpMethods.POST)]
		public IActionResult ProductInfo() {
			var id = Request.Get<Guid>("id");
			var productManager = Application.Ioc.Resolve<ProductManager>();
			if (id == Guid.Empty && Context.GetIsEditingPage()) {
				id = productManager.SelectOneVisibleProductId();
			}
			var info = productManager.GetProductApiInfo(id);
			return new JsonResult(info);
		}

		/// <summary>
		/// 获取商品匹配数据的匹配器列表
		/// </summary>
		/// <returns></returns>
		[Action("api/product/matched_data_matchers", HttpMethods.POST)]
		public IActionResult MatchedDataMatchers() {
			var matchers = Application.Ioc.ResolveMany<IProductMatchedDataMatcher>();
			return new JsonResult(matchers.Select(m => m.GetJavascriptMatchFunction()).ToList());
		}

		/// <summary>
		/// 商品列表分类过滤器使用的信息
		/// </summary>
		/// <returns></returns>
		[Action("api/product/class_filter_info", HttpMethods.POST)]
		public IActionResult ClassFilterInfo() {
			var classManager = Application.Ioc.Resolve<GenericClassManager>();
			var classTree = classManager.GetTreeWithCache(new ProductClassController().Type);
			var tree = TreeUtils.Transform(classTree,
				c => c == null ? null : new { Id = c.Id.ToString(), c.Name });
			return new JsonResult(new { tree });
		}

		/// <summary>
		/// 商品列表标签过滤器使用的信息
		/// </summary>
		/// <returns></returns>
		[Action("api/product/tag_filter_info", HttpMethods.POST)]
		public IActionResult TagFilterInfo() {
			var tagManager = Application.Ioc.Resolve<GenericTagManager>();
			var tags = tagManager.GetManyWithCache(new ProductTagController().Type)
				.Select(t => new { Id = t.Id.ToString(), t.Name }).ToList();
			return new JsonResult(new { tags });
		}

		/// <summary>
		/// 商品列表排序使用的信息
		/// </summary>
		/// <returns></returns>
		[Action("api/product/sort_info", HttpMethods.POST)]
		public IActionResult SortInfo() {
			var sort_orders = new object[] {
				new { name = "Default", value = "default" },
				new { name = "BestSales", value = "best_sales" },
				new { name = "LowerPrice", value = "lower_price" },
				new { name = "HigherPrice", value = "higher_price" },
				new { name = "NewestOnSale", value = "newest_on_sale" },
			};
			return new JsonResult(new { sort_orders });
		}

		/// <summary>
		/// 搜索商品列表
		/// </summary>
		/// <returns></returns>
		[Action("api/product/search", HttpMethods.POST)]
		public IActionResult ProductSearch() {
			var productManager = Application.Ioc.Resolve<ProductManager>();
			var response = productManager.GetProductSearchResponseFromHttpRequest();
			return new JsonResult(new { response });
		}
	}
}
