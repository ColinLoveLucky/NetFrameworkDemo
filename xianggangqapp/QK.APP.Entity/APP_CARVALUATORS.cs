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
    
    public partial class APP_CARVALUATORS : BasicEntity
    {
         public APP_CARVALUATORS(): base(false){ }
         public APP_CARVALUATORS(bool isGenerated = false) : base(isGenerated) { }
            
    	 [Sequence("SEQ_APP_CARVALUATORS")]
    		
         public long ID { get; set; }     
            
    		
         public string CITY_CODE { get; set; }     
            
    		
         public string CITY_NAME { get; set; }     
            
    		
         public string VALUATOR_CODE { get; set; }     
            
    		
         public string VALUATOR_NAME { get; set; }     
        
    }
}
