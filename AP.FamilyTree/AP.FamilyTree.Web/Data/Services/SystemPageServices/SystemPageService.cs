using System;
using System.Collections.Generic;
using System.Linq;
using AP.FamilyTree.Db;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.PageModels.System;

namespace AP.FamilyTree.Web.Data.Services.SystemPageServices
{
    public class SystemPageService
    {
        private EFRepository<LogApplicationError> mAppRepository { get; set; }
        public SystemPageService(FamilyTreeDbContext dbContext)
        {
            mAppRepository = new EFRepository<LogApplicationError>(dbContext);
        }

        public List<ErrorLogModel> GetModel(FilterErrorLogModel filter)
        {
            var endDate = filter.EndDate == null ? null : filter.EndDate?.AddDays(1);

            return mAppRepository.GetQueryable().Where(r => (string.IsNullOrEmpty(filter.ErrorFltr) || r.ErrorMsg.Contains(filter.ErrorFltr))
                                                            && (string.IsNullOrEmpty(filter.UserFltr) || r.UserData.Contains(filter.UserFltr))
                                                            && (filter.StartDate == null || r.InsertDate >= filter.StartDate)
                                                            && (endDate == null || r.InsertDate < endDate))
                .OrderByDescending(r => r.InsertDate)
                .Select(r => new ErrorLogModel()
                {
                    Id = r.LogApplicationErrorId,
                    UserData = r.UserData,
                    ErrorLevel = r.ErrorLevel,
                    ErrorMsg = r.ErrorMsg,
                    ErrorContext = r.ErrorContext,
                    InsertDate = r.InsertDate,
                    BrowserInfo = r.BrowserInfo,
                    AppVersion = r.AppVersion
                }).ToList();
        }
        public List<ErrorLogModel> GetModel()
        {
            var list = mAppRepository.Get().OrderByDescending(r => r.InsertDate);
            var result = list.Select(r => new ErrorLogModel()
            {
                Id = r.LogApplicationErrorId,
                UserData = r.UserData,
                ErrorLevel = r.ErrorLevel,
                ErrorMsg = r.ErrorMsg,
                ErrorContext = r.ErrorContext,
                InsertDate = r.InsertDate,
                BrowserInfo = r.BrowserInfo,
                AppVersion = r.AppVersion
            }).ToList();
            list = null;
            GC.SuppressFinalize(this);
            return result;
        }

        //public List<ErrorLogModel> GetModelUserMsg()
        //{
        //    return mUserRepository.Get().OrderByDescending(r => r.InsertDate)
        //        .Select(r => new ErrorLogModel()
        //        {
        //            Id = r.LogUserErrorRequestId,
        //            UserData = r.UserData,
        //            ErrorLevel = r.ErrorLevel,
        //            ErrorMsg = r.ErrorMsg,
        //            ErrorContext = r.ErrorContext,
        //            InsertDate = r.InsertDate,
        //            ErrorMsgUser = r.ErrorMsgUser
        //        }).ToList();
        //}

        public void DeleteByErrorId(int id)
        {
            var error = mAppRepository.FindById(id);
            mAppRepository.Remove(error);
        }
    }
}
