using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO.Constants
{
    public enum ErrorsEnum
    {
        General = 1,
        Custom = 2,

        UserNotFound = 1001,
        UserAlreadyExists = 1002,
        UserB2CMissing = 1003,

        GroupNotFound = 2001,
        GroupRoleNotFound = 2002,
        GroupSecurityDeviceExists = 2003,
        GroupSecurityDeviceNotFound = 2004
    }

    public class Errors
    {
        public static Dictionary<ErrorsEnum, string> Values = new Dictionary<ErrorsEnum, string>
        {
            { ErrorsEnum.General, "Something went wrong" },
            { ErrorsEnum.Custom, "User message" },

            { ErrorsEnum.UserNotFound, "User was not found, please register in system" },
            { ErrorsEnum.UserAlreadyExists, "User already exists in the system" },
            { ErrorsEnum.UserB2CMissing, "User is missing B2C Id" },

            { ErrorsEnum.GroupNotFound, "Group not found" },
            { ErrorsEnum.GroupRoleNotFound, "Group role not found" },
            { ErrorsEnum.GroupSecurityDeviceExists, "Security device is already in group" },
            { ErrorsEnum.GroupSecurityDeviceNotFound, "Security device in group was not found" }
        };

        public static ErrorDto GetError(ErrorsEnum code)
        {
            return new ErrorDto { ErrorCode = (int)code, ErrorContent = Values[code] };
        }

        public static ErrorDto GetError(ErrorsEnum code, string content)
        {
            return new ErrorDto { ErrorCode = (int)code, ErrorContent = content };
        }
    }

    public class ErrorDto
    {
        public int ErrorCode { get; set; }

        public string ErrorContent { get; set; }
    }
}
