using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.Common.Utilities
{
    public class LGSEEnums
    {

    }
    public enum ErrorCodes
    {
        USER_REGISTERED=1000,
        USER_ACTIVATED,
        USER_ALREADY_ACTIVATED,
        USER_NOT_ACVTD,
        INVALID_USER_PWD,
        UNAUTHORIZED_ACCESS,
        UNAUTHORIZED_REQUEST,
        PWD_CHANGED,
        PASSWORD_NOTMATCHED,
        PASSWORD_ALREADY_USED,
        INVALID_TOKEN,
        INVALID_USER,
        INVALID_ROLE,
        INVALID_REQUEST_TYPE,
        INVALID_USER_ID,
        UNKNOWN_ERROR,
        SYSTEM_ERROR,
        ERROR_ON_SAVING,
        UNABLE_TO_SAVE,
        OTP_GENERATED,
        OTP_GENERATED_ALREADY,
        EMPTY_USER_ID,
        PWD_UPDATED,
        USER_EXISTS,
        USER_NOT_ALLOWED,
        EMAIL_ADD_REQ,
        OTP_EXPIRED,
        INVALID_OTP_CODE,
        ACCOUNT_LOCKED,
        EUSR_REQ,
        INCIDENT_ID_REQD,
        INVALID_INCIDENT_ID,
        USER_DOES_NOT_EXISTS,
        INPUT_DOES_NOT_HAVE_DATA,
        ROLE_EXISTS_ALREADY,
        DUPLICATE_USERS_FOUND,
        DOMAIN_EXISTS_ALREADY,
        DOMAIN_MOD_NOT_ALLOWED,
        DOMAIN_IS_INACTIVE,
        ORG_EXISTS_ALREADY,
        ROLE_DOES_NOT_EXISTS,
        USER_ROLE_MAP_EXISTS,
        USER_CREATED,
        USER_ROLE_MAP_DOES_NOT_EXISTS,
        USER_FIRSTNAME_REQ,
        USER_LASTNAME_REQ,
        USER_DEACTIVATED_BY_ADMIN,
        INPROGRESS_INCIDENT_EXISTS,
        USER_PREFERED_ROLE_NOTSET,
        USER_DOES_NOT_HAVE_ROLES,
        USER_DOES_NOT_PERM_ON_OPERATION,
        USER_ID_RQD,
        USER_EXISTS_NOT_ACTIVATED,
        USER_MPRN_MAPPING_EXISTS,
        TOKEN_DOES_NOT_EXISTS,
        DUPLICATE_MPRNS_FOUND_IN_REQ,
        UNASSIGN_MPRNs_BEFORE_DEACTIVATE,
        NO_INPROGRESS_USER_MPRN_MAPPING,
        EUSR_DOES_NOT_EXISTS,
        OLD_PWD_NOTMATCHED
        
    }
    public enum Features
    {
        INCIDENTMGT,
        RESOURCEMGT,
        MAPWEB,
        MAPMOBILE,
        SOCIALMEDIA,
        DASHBOARD,
        PORTALMANAGEMENT,
        DOWNLOADMPRN,
        SIGNUP,
        MPRNSTATUS,
        ASSIGNEDMPRN
    }
    public enum OperationType
    {
        CREATE=1,
        READ=2,
        UPDATE=3,
        DELETE=4
    }
    public enum AuditTrialOpType
    {
        LOGIN,
        LOGOUT
    }
    public enum AuditTrialStatus
    {
        SUCCESS,
        FAILURE
    }

}
