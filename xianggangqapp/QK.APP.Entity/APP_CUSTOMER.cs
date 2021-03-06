//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QK.QAPP.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class APP_CUSTOMER : BasicEntity
    {
         public APP_CUSTOMER(): base(false){ }
         public APP_CUSTOMER(bool isGenerated = false) : base(isGenerated) { }
            
    	 [Sequence("SEQ_APP_CUSTOMER")]
    		
         public long ID { get; set; }     
            
    		
         public long APP_ID { get; set; }     
            
    		
         public string NAME { get; set; }     
            
    		
         public string GENDER { get; set; }     
            
    		
         public Nullable<short> AGE { get; set; }     
            
    		
         public string ID_NO { get; set; }     
            
    		
         public string ID_TYPE { get; set; }     
            
    		
         public string EDUCATION { get; set; }     
            
    		
         public string MARRAGE { get; set; }     
            
    		
         public string HAS_LOCAL_HOUSE { get; set; }     
            
    		
         public Nullable<decimal> MORTGAGE_AMT_BY_MONTH { get; set; }     
            
    		
         public string HASCARS { get; set; }     
            
    		
         public string CAR_BRAND_OWNED { get; set; }     
            
    		
         public string CAR_MODEL_OWNED { get; set; }     
            
    		
         public string RESIDENT_STATUS { get; set; }     
            
    		
         public string RESIDENTOTHERS { get; set; }     
            
    		
         public string RESIDENT_PROVINCE { get; set; }     
            
    		
         public string RESIDENT_CITY { get; set; }     
            
    		
         public string RESIDENT_ADDRESS { get; set; }     
            
    		
         public string POSTCODE { get; set; }     
            
    		
         public Nullable<short> YEARS_IN_LOCAL { get; set; }     
            
    		
         public string RESIDENT_TEL_AREA_CODE { get; set; }     
            
    		
         public string RESIDENT_TEL_NO { get; set; }     
            
    		
         public string RELATIONSHIP_OF_RESIDENT_TEL { get; set; }     
            
    		
         public string MOBILE1 { get; set; }     
            
    		
         public string MOBILE2 { get; set; }     
            
    		
         public string REGISTER_ADDR_TYPE { get; set; }     
            
    		
         public string REGISTER_PROVINCE { get; set; }     
            
    		
         public string REGISTER_CITY { get; set; }     
            
    		
         public string REGISTER_ADDRESS { get; set; }     
            
    		
         public Nullable<decimal> MAX_CREDITLIMIT_OF_CCC { get; set; }     
            
    		
         public Nullable<short> NUMS_OF_CCC { get; set; }     
            
    		
         public string EMAIL { get; set; }     
            
    		
         public string QQ { get; set; }     
            
    		
         public Nullable<decimal> INCOME_ANNUAL { get; set; }     
            
    		
         public Nullable<decimal> INCOME_MONTHLY_FROM_JOB { get; set; }     
            
    		
         public Nullable<decimal> INCOME_MONTHLY_FROM_OTHER { get; set; }     
            
    		
         public string MEMO_OF_INCOME_FROM_OTHER { get; set; }     
            
    		
         public Nullable<short> NUMS_OF_PROVIDE { get; set; }     
            
    		
         public string MARRAGE_OTHER { get; set; }     
            
    		
         public Nullable<System.DateTime> BRITHDAY { get; set; }     
            
    		
         public string DRIVING_LICENSE_OWNER { get; set; }     
            
    		
         public string HAS_CHILDREN { get; set; }     
            
    		
         public string CREDIT_CARD_PAYBACK { get; set; }     
            
    		
         public string CREDIT_CARD_TIME { get; set; }     
            
    		
         public string MAX_CREDITLIMIT_DATADIC { get; set; }     
            
    		
         public string INCOME_MONTHLY_DATADIC { get; set; }     
            
    		
         public string APPLICANT_IDENTITY { get; set; }     
            
    		
         public string CUSTOMER_GROUP { get; set; }     
        public virtual APP_MAIN APP_MAIN { get; set; }
        
    }
}
