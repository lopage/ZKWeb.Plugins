﻿using System.Collections.Generic;
using System.Linq;
using ZKWeb.Cache;
using ZKWeb.Plugins.Common.Base.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Domain.Services.Bases;
using ZKWeb.Plugins.Common.Region.src.Components.Countries.Bases;
using ZKWeb.Plugins.Common.Region.src.Components.GenericConfigs;
using ZKWebStandard.Collections;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Common.Region.src.Domain.Services {
	/// <summary>
	/// 地区管理器
	/// </summary>
	[ExportMany, SingletonReuse]
	public class RegionManager : DomainServiceBase, ICacheCleaner {
		/// <summary>
		/// 国家/行政区名称到信息的缓存
		/// </summary>
		protected LazyCache<Dictionary<string, Country>> CountryCache = LazyCache.Create(() => {
			return Application.Ioc.ResolveMany<Country>().ToDictionary(c => c.Name);
		});

		/// <summary>
		/// 获取默认的国家/行政区信息，找不到时返回null
		/// </summary>
		/// <returns></returns>
		public virtual Country GetDefaultCountry() {
			var configManager = Application.Ioc.Resolve<GenericConfigManager>();
			var settings = configManager.GetData<RegionSettings>();
			return GetCountry(settings.DefaultCountry);
		}

		/// <summary>
		/// 获取国家/行政区信息，找不到时返回null
		/// </summary>
		/// <param name="name">国家/行政区名称，区分大小写</param>
		/// <returns></returns>
		public virtual Country GetCountry(string name) {
			return CountryCache.Value.GetOrDefault(name);
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public void ClearCache() {
			CountryCache.Reset();
		}
	}
}
