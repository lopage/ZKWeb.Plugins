﻿using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.Translates {
	/// <summary>
	/// 中文翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class zh_CN : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "zh-CN" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Admin Login", "管理员登陆" },
			{ "Please enter username", "请填写用户名" },
			{ "Please enter password", "请填写密码" },
			{ "Username", "用户名" },
			{ "Password", "密码" },
			{ "Login", "登陆" },
			{ "Register new user", "注册新用户" },
			{ "{0} is required", "请填写{0}" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}