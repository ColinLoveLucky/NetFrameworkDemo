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
    
    public partial class APP_HOUSE : BasicEntity
    {
         public APP_HOUSE(): base(false){ }
         public APP_HOUSE(bool isGenerated = false) : base(isGenerated) { }
            
    	 [Sequence("SEQ_APP_HOUSE")]
    		
         public long ID { get; set; }     
            
    		
         public long APP_ID { get; set; }     
            
    		
         public string COLLATERAL_TYPE { get; set; }     
            
    		
         public string COLLATERAL_STATUS { get; set; }     
            
    		
         public string HOUSE_TYPE { get; set; }     
            
    		
         public string HOUSE_PROVINCE { get; set; }     
            
    		
         public string HOUSE_CITY { get; set; }     
            
    		
         public string HOUSE_ADDRESS { get; set; }     
            
    		
         public string LIVING_QUARTER_NAME { get; set; }     
            
    		
         public Nullable<decimal> COVERED_AREA { get; set; }     
            
    		
         public Nullable<short> COMPLETE_AGE { get; set; }     
            
    		
         public Nullable<short> PROPERTYRIGHT_PERSONS { get; set; }     
            
    		
         public string LOAN_BANK { get; set; }     
            
    		
         public Nullable<decimal> REMAIN_PRINCIPAL { get; set; }     
            
    		
         public Nullable<decimal> ASSESSMENT_VALUE { get; set; }     
            
    		
         public string COLLATERAL_STATUS_OTHER { get; set; }     
            
    		
         public string HOUSE_TYPE_OTHER { get; set; }     
        public virtual APP_MAIN APP_MAIN { get; set; }
        
    }
}