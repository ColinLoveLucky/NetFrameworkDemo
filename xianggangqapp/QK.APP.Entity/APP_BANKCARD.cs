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
    
    public partial class APP_BANKCARD : BasicEntity
    {
         public APP_BANKCARD(): base(false){ }
         public APP_BANKCARD(bool isGenerated = false) : base(isGenerated) { }
            
    	 [Sequence("SEQ_APP_BANKCARD")]
    		
         public long ID { get; set; }     
            
    		
         public long APP_ID { get; set; }     
            
    		
         public string BANK_CODE { get; set; }     
            
    		
         public string BANK_NAME { get; set; }     
            
    		
         public string BANK_ACCOUNT { get; set; }     
            
    		
         public string BANK_PROVINCE { get; set; }     
            
    		
         public string BANK_CITY { get; set; }     
            
    		
         public string BANK_SUB { get; set; }     
            
    		
         public string MAIL_ADDRESS_TYPE { get; set; }     
            
    		
         public string MAIL_PROVINCE { get; set; }     
            
    		
         public string MAIL_CITY { get; set; }     
            
    		
         public string MAIL_ADDRESS { get; set; }     
            
    		
         public Nullable<System.DateTime> DATE_SIGNATURE { get; set; }     
            
    		
         public string BANK_MOBILE { get; set; }     
        public virtual APP_MAIN APP_MAIN { get; set; }
        
    }
}