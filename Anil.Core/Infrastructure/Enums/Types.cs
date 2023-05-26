using System.ComponentModel;

namespace Anil.Core.Infrastructure.Enums
{
    public class Types
    {
        public enum OperationResultType
        {
            [Description("ناموفق")]
            Faild = 1,
            [Description("موفق")]
            Success = 0,
            [Description("خطا")]
            Error = 2
        }
    }
}
