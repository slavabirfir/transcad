using System.Collections.Generic;
using BLEntities.Entities;
using BLEntities.Model;
using DAL;
using IDAL;

namespace BLManager
{
    public class BlLoginsManager
    {
        private readonly IInternalBaseDal _internalBaseDal;
        public BlLoginsManager()
        {
            _internalBaseDal = new InternalTvunaImplementationDal();
        }

        public List<TranscadLogin> GetAllTranscadLogins()
        {
            return _internalBaseDal.GetAllTranscadLogins();
        }
        public bool  DeleteLogin()
        {
            if (GlobalData.LoginUser != null && !string.IsNullOrEmpty( GlobalData.LoginUser.UserName))
                return _internalBaseDal.TranscadLoginDelete(GlobalData.LoginUser.UserName);
            return false;
        }

        public bool DeleteLogin(TranscadLogin transcadLogin)
        {
            if (transcadLogin!=null && !string.IsNullOrEmpty(transcadLogin.UserName))
               return _internalBaseDal.TranscadLoginDelete(transcadLogin.UserName);
            return false; 
        }
    }
}
