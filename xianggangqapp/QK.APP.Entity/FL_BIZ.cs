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
    
    public partial class FL_BIZ : BasicEntity
    {
         public FL_BIZ(): base(false){ }
         public FL_BIZ(bool isGenerated = false) : base(isGenerated) { }
            
    	 [Sequence("SEQ_FL_BIZ")]
    		
         public long ID { get; set; }     
            
    		
         public string BIZ_CODE { get; set; }     
            
    		
         public string ID_NO { get; set; }     
            
    		
         public string ID_TYPE { get; set; }     
            
    		
         public string USER_NAME { get; set; }     
            
    		
         public Nullable<short> FILE_NUM { get; set; }     
            
    		
         public string CREATED_USER { get; set; }     
            
    		
         public Nullable<System.DateTime> CREATED_TIME { get; set; }     
            
    		
         public string CHANGED_USER { get; set; }     
            
    		
         public Nullable<System.DateTime> CHANGED_TIME { get; set; }     
        
    }
}
