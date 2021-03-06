﻿using DotLiquid;
using ZKWeb.Plugin;
using ZKWeb.Templating.DynamicContents;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Theme.VisualEditor.src {
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
			var adminNavBar = areaManager.GetArea("admin_navbar_right");
			adminNavBar.DefaultWidgets.Add("theme.visualeditor.widgets/enter_visual_editor");
		}
	}
}
