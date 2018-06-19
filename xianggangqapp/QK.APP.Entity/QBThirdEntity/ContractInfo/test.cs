using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.QBThirdEntity.ContractInfo
{
    public class test
    {
        public string APP_ID = "AAA";
        public string ACTION = "CONT_CREATE";

        public string SIGN_TYPE = "E_AUTO";
        public CA_INFO CA_INFO
        {
            get
            {
                return new CA_INFO()
                {
                    RA_CODE = "FDD",
                    CA_USER = new CA_INFO._CA_USER()
                    {
                        USER_NAME = "ABC",
                        NATIONAL_ID = "320219198102155778",
                        MOBILE_NO = "13888888888",
                        EMAIL_ID = "88888888@qq.com"
                    }

                };

            }
        }

        public List<BIZ_KEY_VAL> BIZ_TEMP_MAP
        {
            get
            {
                return new List<BIZ_KEY_VAL>(){
              new BIZ_KEY_VAL(){ BIZ_KEY="BRAND_CODE",
                                 BIZ_VAL="LOAN_MICR_CAR" },
                                  new BIZ_KEY_VAL(){ BIZ_KEY="CHANNEL_CODE",
                                 BIZ_VAL="P2P" }
            };
            }
        }

        public BIZ_INFO BIZ_INFO
        {
            get
            {
                return new BIZ_INFO()
                {
                    BASE_INFO = new List<BIZ_KEY_VAL>(){
            new BIZ_KEY_VAL(){
               BIZ_KEY="BIZ_ID",
               BIZ_VAL="CNT20160315_009"
            },
             new BIZ_KEY_VAL(){
               BIZ_KEY="CONTRACT_ID",
               BIZ_VAL="CNT20160315_009"
            }
            },
                    BIZ_ENTS = new List<object>(){
                 new BIZ_ENTS<BASE_INFO>(){
                  BIZ_ENT = new BASE_INFO()
                 }
            }
                };
            }
        }


        public class BASE_INFO
        {
            public string YEAR = "2020-1";
        }

    }

}
