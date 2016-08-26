﻿using ZKWeb.Database;

namespace ZKWeb.Plugins.Common.Base.src.Domain.Filters.Interfaces {
	/// <summary>
	/// 保存过滤器
	/// </summary>
	public interface IEntitySaveFilter {
		/// <summary>
		/// 过滤保存
		/// </summary>
		/// <typeparam name="TEntity">实体类型</typeparam>
		/// <typeparam name="TPrimaryKey">主键类型</typeparam>
		/// <param name="entity">实体对象</param>
		void Filter<TEntity, TPrimaryKey>(TEntity entity)
			where TEntity : class, IEntity<TPrimaryKey>;
	}
}
