using System;

namespace AP.FamilyTree.Web.Data.Exceptions
{
    public class ExceptionByType : Exception
    {
        public ExeptionTypeEnum mExeptionType;

        public ExceptionByType(ExeptionTypeEnum exeptionType) : base(Global.ExceptionText[exeptionType])
        {
            mExeptionType = exeptionType;
        }

        public ExceptionByType(ExeptionTypeEnum exeptionType, string message) : base(message)
        {
            mExeptionType = exeptionType;
        }
    }
}
