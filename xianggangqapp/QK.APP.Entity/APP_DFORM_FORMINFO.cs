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
    
    public partial class APP_DFORM_FORMINFO : BasicEntity
    {
        public APP_DFORM_FORMINFO(): base(false){
                this.APP_DFORM_FORMFIELD = new HashSet<APP_DFORM_FORMFIELD>();
        }
        public APP_DFORM_FORMINFO(bool isGenerated = false): base(isGenerated)
        {
            this.APP_DFORM_FORMFIELD = new HashSet<APP_DFORM_FORMFIELD>();
        }
    
            
    	 [Sequence("SEQ_APP_DFORM_FORMINFO")]
    		
         public long ID { get; set; }     
            
    		
         public Nullable<long> FB_ID { get; set; }     
            
    		
         public string NAME { get; set; }     
            
    		
         public string VERSION { get; set; }     
            
    		
         public string READONLY { get; set; }     
            
    		
         public string ACTION_EDIT { get; set; }     
            
    		
         public string ACTION_READ { get; set; }     
            
    		
         public string CREATED_USER { get; set; }     
            
    		
         public Nullable<System.DateTime> CREATED_TIME { get; set; }     
            
    		
         public string CHANGED_USER { get; set; }     
            
    		
         public Nullable<System.DateTime> CHANGED_TIME { get; set; }     
            
    		
         public Nullable<int> Sort { get; set; }     
            
    		
         public string ADDMORE { get; set; }     
            
    		
         public string ADDMOREKEYWORD { get; set; }     
            
    		
         public string ENABLE { get; set; }     
        public virtual APP_DFORM_FORMBUILDER APP_DFORM_FORMBUILDER { get; set; }
        public virtual ICollection<APP_DFORM_FORMFIELD> APP_DFORM_FORMFIELD { get; set; }
        
    }
}
