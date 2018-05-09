using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.DTO
{
    public class ErrorDto
    {
        public int ErrorCode { get; set; }
        public string ErrorContent { get; set; }


        public override string ToString()
        {
            return $"Code: {ErrorCode} | Content: {ErrorContent}";
        }
    }
}
