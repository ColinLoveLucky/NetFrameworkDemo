using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;

namespace QK.QAPP.IServices
{
    public partial interface IAPP_QUANTSERVICE
    {
        QuantImportResult ImportData(System.Web.HttpRequestBase request);

        QuantHandleResult HandleQuantData();

        QuantHandleResult HandleInfo();
    }
}
