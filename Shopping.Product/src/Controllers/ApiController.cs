﻿using DryIoc;
using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Plugins.Common.Base.src.Extensions;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.GenericClass.src.Manager;
using ZKWeb.Plugins.Common.GenericTag.src.Manager;
using ZKWeb.Plugins.Shopping.Product.src.Extensions;
using ZKWeb.Plugins.Shopping.Product.src.GenericClasses;
using ZKWeb.Plugins.Shopping.Product.src.GenericTags;
using ZKWeb.Plugins.Shopping.Product.src.Managers;
using ZKWeb.Plugins.Shopping.Product.src.Model;
using ZKWeb.Utils.Extensions;
using ZKWeb.Utils.Functions;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.Shopping.Product.src.Controllers {
	/// <summary>
	/// Api控制器
	/// </summary>
	[ExportMany]
	public class ApiController : IController {
		/// <summary>
		/// 获取指定类目对应的属性编辑器
		/// 这里仅用于获取编辑器Html，不绑定数据也不检查权限
		/// </summary>
		/// <returns></returns>
		[Action("api/product/property_editor")]
		public IActionResult PropertyEditor() {
			var categoryId = HttpContextUtils.CurrentContext.Request.Get<long>("categoryId");
			var categoryManager = Application.Ioc.Resolve<ProductCategoryManager>();
			// 获取类目
			var category = categoryManager.FindCategory(categoryId);
			if (category == null) {
				return new PlainResult("");
			}
			// 获取销售和非销售属性的Html列表
			var salesProperties = new List<HtmlString>();
			var nonSalesProperties = new List<HtmlString>();
			foreach (var property in categoryManager.GetProperties(category)) {
				var html = property.GetEditHtml(category);
				(property.IsSaleProperty ? salesProperties : nonSalesProperties).Add(html);
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
			var categoryId = HttpContextUtils.CurrentContext.Request.Get<long?>("categoryId");
			var conditionBinders = Application.Ioc
				.ResolveMany<ProductMatchedDataConditionBinder>()
				.Where(b => b.Init(categoryId)).ToList();
			var affectsBinders = Application.Ioc
				.ResolveMany<ProductMatchedDataAffectsBinder>()
				.Where(b => b.Init(categoryId)).ToList();
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
			var id = HttpContextUtils.CurrentContext.Request.Get<long>("id");
			var productManager = Application.Ioc.Resolve<ProductManager>();
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
			var classTree = classManager.GetClassTree(new ProductClass().Type);
			var tree = TreeUtils.Transform(classTree, c => c == null ? null : new { c.Id, c.Name });
			return new JsonResult(new { tree = tree });
		}

		/// <summary>
		/// 商品列表标签过滤器使用的信息
		/// </summary>
		/// <returns></returns>
		[Action("api/product/tag_filter_info", HttpMethods.POST)]
		public IActionResult TagFilterInfo() {
			var tagManager = Application.Ioc.Resolve<GenericTagManager>();
			var tags = tagManager.GetTags(new ProductTag().Type)
				.Select(t => new { t.Id, t.Name }).ToList();
			return new JsonResult(new { tags });
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
