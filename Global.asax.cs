using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AgCareers.EliteTalent.Models;
using System.Web.Security;
using AgCareers.EliteTalent.Controllers;
using AgCareers.EliteTalent.Models.ViewModel;
using Newtonsoft.Json;
using AgCareers.EliteTalent.Models.Security;
using AgCareers.EliteTalent.App_Start;
using AgCareers.EliteTalent.Models.Repositories;
using System.Security.Principal;
using FluentSecurity;
using System.Web.Helpers;
namespace AgCareers.EliteTalent
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.Start();
            AuthConfig.RegisterAuth();
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            // BootstrapSupport.BootstrapBundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);
            //BootstrapMvcSample.ExampleLayoutsRouteConfig.RegisterRoutes(RouteTable.Routes);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = true;
          
        }
        /*
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }*/
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {

                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                if (serializeModel.roles == MyRoles.Admin)
                {
                    newUser.AdminID = serializeModel.AdminID;
                    newUser.roles = serializeModel.roles;
                }
                else
                {
                    newUser.MemberID = serializeModel.MemberID;
                    newUser.UserId = serializeModel.UserId;
                    newUser.ApplicantID = serializeModel.ApplicantID;
                    newUser.EmployerID = serializeModel.EmployerID;
                    newUser.ProfileID = serializeModel.ProfileID;
                }
              
                newUser.FirstName = serializeModel.FirstName;
                newUser.LastName = serializeModel.LastName;
                newUser.roles = serializeModel.roles;
                newUser.UserName = newUser.UserName;
                HttpContext.Current.User = newUser;
            }
        }
        //protected void Application_AuthenticateRequest(object sender, EventArgs args)
        //{
        //    if (Context.User != null)
        //    {


        //        using (IMemberRepository memberRepository = new MemberRepository())
        //        {
        //            var objUser = memberRepository.FindBy(x => (x.UserName == Context.User.Identity.Name || x.Email == Context.User.Identity.Name) && x.Approved == 1);

        //            List<string> roles = new List<string>();
        //            foreach (Member user in objUser)
        //            {
        //                int index = Convert.ToInt32(user.TypeID);
        //                var role = (MyRoles)index;
        //                roles.Add(role.ToString());

        //            }


        //            //IEnumerable<MyRoles> roles = new UsersService.UsersClient().GetUserRoles(
        //            //                                        Context.User.Identity.Name);


        //            //string[] rolesArray = new string[roles.Count()];
        //            //for (int i = 0; i < roles.Count(); i++)
        //            //{
        //            //    rolesArray[i] = roles.ElementAt(i).RoleName;
        //            //}

        //            GenericPrincipal gp = new GenericPrincipal(Context.User.Identity, roles.ToArray());
        //            Context.User = gp;
        //        }
        //    }
        //}


        //public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        //{
        //    filters.Add(new HandleErrorAttribute());

        //    //Uncomment this line to eable custom attrbute security
        //    filters.Add(new SecurityFilter());

        //    //Uncomment this line to use custom 
        //    //filters.Add(new CustomSecurityAttribute());


        //    SecurityConfigurator.Configure(configuration =>
        //    {
        //        // Let Fluent Security know how to get the authentication status of the current user
        //        configuration.GetAuthenticationStatusFrom(() => HttpContext.Current.User.Identity.IsAuthenticated);

        //        // Let Fluent Security know how to get the roles for the current user
        //        configuration.GetRolesFrom(() =>
        //        {
        //            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

        //            if (authCookie != null)
        //            {
        //                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        //                return authTicket.UserData.Split(',');
        //            }
        //            else
        //            {
        //                return new[] { "" };
        //            }
        //        });

        //        // This is where you set up the policies you want Fluent Security to enforce
        //        configuration.For<MemberController>().Ignore();

        //        configuration.For<MemberController>(x => x.Dashboard()).RequireRole("Public");

        //        //configuration.For<MemberController>(x => x.Index()).DenyAnonymousAccess();
        //        //configuration.For<MemberController>(x => x.Dashboard()).RequireRole("Registered", "Admin");
        //        //configuration.For<MemberController>(x => x.Home()).RequireRole("Registered", "Admin");
        //        //configuration.For<MemberController>(x => x.MyAge()).RequireRole("Registered", "Admin");
        //        //configuration.For<MemberController>(x => x.Profile()).RequireRole("Registered", "Admin");

        //        configuration.For<AdminController>(x => x.Index()).DenyAnonymousAccess();
        //        configuration.For<AdminController>(x => x.Dashboard()).RequireRole("Admin");
        //        configuration.For<AdminController>(x => x.Home()).RequireRole("Admin");
        //        configuration.For<AdminController>(x => x.Denied()).RequireRole("Admin");
        //        configuration.For<JobController>(x => x.Jobs()).RequireRole("Employer");
        //    });

        //    //Uncomment this line to enable fluent security
        //    //GlobalFilters.Filters.Add(new HandleSecurityAttribute(), 0);
        //}
    }
}