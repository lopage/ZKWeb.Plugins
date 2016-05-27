﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Plugins.Common.Base.src.Repositories;

namespace ZKWeb.Plugins.Common.Base.src.Extensions {
	/// <summary>
	/// 静态表格数据的搜索请求的扩展函数
	/// </summary>
	public static class StaticTableSearchRequestExtensions {
		/// <summary>
		/// 从数据库中的数据构建搜索回应
		/// 支持自动分页和配合表格回调设置结果
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="request">搜索请求</param>
		/// <param name="callbacks">表格回调</param>
		/// <returns></returns>
		public static StaticTableSearchResponse BuildResponseFromDatabase<TData>(
			this StaticTableSearchRequest request, IEnumerable<IStaticTableCallback<TData>> callbacks)
			where TData : class {
			var response = new StaticTableSearchResponse();
			UnitOfWork.Read(context => {
				// 从数据库获取数据，过滤并排序
				var query = context.Query<TData>();
				foreach (var callback in callbacks) {
					callback.OnQuery(request, context, ref query);
				}
				foreach (var callback in callbacks) {
					callback.OnSort(request, context, ref query);
				}
				// 分页并设置分页信息
				// 当前页没有任何内容时返回最后一页的数据
				var queryResult = response.Pagination.Paging(request, query);
				response.PageIndex = request.PageIndex;
				response.PageSize = request.PageSize;
				// 选择数据
				// 默认把对象转换到的字符串保存到ToString中
				var pairs = queryResult
					.Select(r => new KeyValuePair<TData, Dictionary<string, object>>(
						r, new Dictionary<string, object>()))
					.ToList();
				foreach (var pair in pairs) {
					pair.Value["ToString"] = pair.Key.ToString();
				}
				foreach (var callback in callbacks) {
					callback.OnSelect(request, pairs);
				}
				response.Rows = pairs.Select(p => p.Value).ToList();
			});
			return response;
		}
	}
}