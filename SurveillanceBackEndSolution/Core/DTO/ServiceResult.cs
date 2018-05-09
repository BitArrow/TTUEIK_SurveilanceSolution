using Core.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class ServiceResult
    {
        //private static readonly Logger Nlog = LogManager.GetCurrentClassLogger();

        public static ServiceResult Ok()
        {
            return new ServiceResult(true);
        }

        public static ServiceResult Ok(object obj)
        {
            return new ServiceResult(true, obj);
        }

        public static ServiceResult Error(ErrorsEnum code)
        {
            return new ServiceResult(code);
        }

        public static ServiceResult Error(string error)
        {
            return new ServiceResult(error);
        }


        public ServiceResult(bool success)
        {
            Succeeded = success;
            Content = null;
        }

        public ServiceResult(bool success, object obj)
        {
            Succeeded = success;
            Content = obj;
        }

        public ServiceResult(ErrorsEnum code)
        {
            Succeeded = false;
            ErrorResult = Errors.GetError(code);
        }

        public ServiceResult(string error)
        {
            Succeeded = false;
            ErrorResult = Errors.GetError(ErrorsEnum.Custom, error);
        }


        public object Content { get; private set; }

        public bool Succeeded { get; private set; }

        public ErrorDto ErrorResult { get; private set; }
    }
}
