﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Web.ActionResults;
using ZKWeb.Plugins.Common.Admin.src.Database;
using ZKWeb.Plugins.Common.Admin.src.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src;
using ZKWeb.Plugins.Common.Base.src.Managers;
using ZKWeb.Utils.Extensions;
using ZKWeb.Web.Interfaces;
using ZKWeb.Localize;
using ZKWeb.Utils.Functions;
using ZKWeb.Cache.Interfaces;
using ZKWeb.Utils.IocContainer;

namespace ZKWeb.Plugins.Common.Admin.src.Controllers {
	/// <summary>
	/// Api控制器
	/// </summary>
	[ExportMany]
	public class ApiController : IController {
		/// <summary>
		/// 获取当前登陆的用户信息
		/// </summary>
		/// <returns></returns>
		[Action("api/user/login_info", HttpMethods.POST)]
		public IActionResult UserLoginInfo() {
			var sessionManager = Application.Ioc.Resolve<SessionManager>();
			var user = sessionManager.GetSession().GetUser() ?? new User();
			var userManager = Application.Ioc.Resolve<UserManager>();
			return new JsonResult(new {
				userId = user.Id,
				username = user.Username,
				userType = user.Type.ToString(),
				userIsAdmin = UserTypesGroup.Admin.Contains(user.Type),
				userIsParter = UserTypesGroup.Parter.Contains(user.Type),
				userIsAdminOrParter = UserTypesGroup.AdminOrParter.Contains(user.Type),
				avatar = userManager.GetAvatarWebPath(user.Id)
			});
		}

		/// <summary>
		/// 清理缓存
		/// 要求本地访问或管理员登陆
		/// </summary>
		/// <returns></returns>
		[Action("api/cache/clear", HttpMethods.POST)]
		public IActionResult ClearCache() {
			var request = HttpContextUtils.CurrentContext.Request;
			if (!request.IsLocal) {
				PrivilegesChecker.Check(UserTypesGroup.Admin);
			}
			var cleaners = Application.Ioc.ResolveMany<ICacheCleaner>();
			cleaners.ForEach(c => c.ClearCache());
			GC.Collect();
			return new JsonResult(new { message = new T("Clear Cache Successfully") });
		}
	}
}
