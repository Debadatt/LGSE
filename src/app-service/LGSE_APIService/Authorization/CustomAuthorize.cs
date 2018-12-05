using LGSE_APIService.Common.Utilities;
using LGSE_APIService.Models;
using LGSE_APIService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Tracing;

namespace LGSE_APIService.Authorization
{
    /// <summary>
    /// Custom attribute for handling  Custom Authorization
    /// </summary>
    public class CustomAuthorize : System.Web.Http.AuthorizeAttribute, IDisposable
    {
        /// <summary>
        /// Manages the checks and verifications for Autorizations
        /// </summary>
        AuthorizationManager authManager = null;


        /// <summary>
        /// Data entities to be protected
        /// </summary>
      //  public Features Module { get; set; }
        public Features[] Module { get; set; }
        //public string Module { get; set; }
        /// <summary>
        /// Types of Operations allowed
        /// </summary>
        public OperationType OperationType { get; set; }

        /// <summary>
        /// Enables the Data level permissions check
        /// </summary>
        public bool IsDataLevelPermReq { get; set; }

        /// <summary>
        /// For custom APIS
        /// </summary>
        public bool IsCustomAPI { get; set; }

        public ITraceWriter traceWriter  { get; set; }
        /// <summary>
        /// Calls when an action is being authorized.
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }
        /// <summary>
        /// Checks for the user authorization
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            try
            {
                // return true;
                //traceWriter = actionContext.ControllerContext.Configuration.Services.GetTraceWriter();
                //traceWriter.Info("Custom Authorization started");
                
                using (var dbContext = new LGSE_APIContext())
                {
                    authManager = new AuthorizationManager(dbContext);
                    string userEmail = HttpUtilities.GetUserNameFromToken(actionContext.Request);
                    // Abhijeet - 30-10-2018 -Added role id
                    string userRole = HttpUtilities.GetUserRoleAccessApi(actionContext.Request);




                    if (authManager.IsUserExists(userEmail))
                    {
                        return authManager.AuthorizeUser(userEmail, userRole,
                            HttpUtilities.GetRequestToken(actionContext.Request), Module, OperationType);

                    }
                    else
                    {
                        HandleUnauthenticatedRequests(actionContext, "User Doesnt Exist");
                        return false;
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                HandleUnauthorizedRequests(actionContext, ex.Message);
                return true;
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                //  traceWriter.Error(ex, actionContext.Request,"CustomAuthorizeError");
                //this.Configuration.Services.GetTraceWriter()
                //log error message
                //throw ex;
                return false;
            }
        }
        /// <summary>
        /// Handles the Exceptions comming in
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="message"></param>
        private void HandleUnauthorizedRequests(HttpActionContext actionContext, string message)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            if (!string.IsNullOrEmpty(message))
                actionContext.Response.ReasonPhrase = message;
        }

        private void HandleUnauthenticatedRequests(HttpActionContext actionContext, string message)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            if (!string.IsNullOrEmpty(message))
                actionContext.Response.ReasonPhrase = message;
        }

        /// <summary>
        /// Safely Disposes the attribute objects
        /// </summary>
        public void Dispose()
        {
            ////todo: Implement dispose
            //if (dbContext != null)
            //{
            //    dbContext.Database.Connection.Close();
            //    dbContext.Dispose();
            //}
        }
    }
}