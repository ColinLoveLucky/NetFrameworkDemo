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
    
    public partial class APP_DFORM_FORMBUILDER : BasicEntity
    {
        public APP_DFORM_FORMBUILDER(): base(false){
                this.APP_DFORM_FORMINFO = new HashSet<APP_DFORM_FORMINFO>();
        }
        public APP_DFORM_FORMBUILDER(bool isGenerated = false): base(isGenerated)
        {
            this.APP_DFORM_FORMINFO = new HashSet<APP_DFORM_FORMINFO>();
        }
    
            
    	 [Sequence("SEQ_APP_DFORM_FORMBUILDER")]
    		
         public long ID { get; set; }     
            
    		
         public string NAME { get; set; }     
            
    		
         public string VERSION { get; set; }     
            
    		
         public Nullable<long> PRODUCT_ID { get; set; }     
            
    		
         public string ACTION_EDIT { get; set; }     
            
    		
         public string ACTION_READ { get; set; }     
            
    		
         public string CREATED_USER { get; set; }     
            
    		
         public Nullable<System.DateTime> CREATED_TIME { get; set; }     
            
    		
         public string CHANGED_USER { get; set; }     
            
    		
         public Nullable<System.DateTime> CHANGED_TIME { get; set; }     
            
    		
         public string CODE { get; set; }     
        public virtual ICollection<APP_DFORM_FORMINFO> APP_DFORM_FORMINFO { get; set; }
        
    }
}
