using KH = KulAid.Helpers;
using System;
using KulAid.Helpers.ErrorReporting;
namespace AboutPage
{
    public class StaticInfoManager
    {
        public void RetrieveSettingsFromAssembly(Action<StaticInfoModel> completed, Action<Exception> error)
        {
            try
            {
                KH.IO<StaticInfoModel> io = new KH.IO<StaticInfoModel>();
                io.BeginLoadFileFromResource("/AboutPage;component/StaticInfo.xml", completed, error);
            }
            catch (Exception ex)
            {
                ErrorReporter.HandleError(ex);
                error(ex);
            }
        }

    }
}
