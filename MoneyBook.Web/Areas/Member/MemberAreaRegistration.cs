using System.Web.Mvc;

namespace MoneyBook.Web.Areas.MoneyBook {
    public class MemberAreaRegistration : AreaRegistration {
        public override string AreaName {
            get {
                return "Member";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "Member_Default",
                "Member/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "MoneyBook.Web.Areas.Member.Controllers" }
            );
        }
    }
}