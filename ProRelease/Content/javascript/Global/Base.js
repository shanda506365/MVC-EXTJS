/**
 * @author LynnChen
 */
Ext.define('WS.action.Base', {
    extend: 'Ext.Action',
    alternateClassName: ['ActionBase'],
    config: {
        targetView: null,
        category: ''
    },
    getTargetView:function()
    {
    	var me = this;
    	if(!me.tgtView)
    		return null;
    	else if(typeof me.tgtView == 'function')
    		return me.tgtView();
    	else
    		return me.tgtView;
    },
    setTargetView:function(tgtView)
    {
    	var me = this;
    	//if(!me.tgtView)
    	me.tgtView = tgtView;
    },
    statics: {
        actions: [],
		//set some actions' target view(grid, tree...)
		//then these actions can use this.getTargetView()
        setTargetView: function(category, view){
            Ext.Array.each(ActionBase.actions, function(act, index){
                if (act.category == category) 
                    act.setTargetView(view);
                return true;
            });
        },
        setWidth : function(category, width){  
        	  Ext.Array.each(ActionBase.actions, function(act, index){
                if (act.category == category) 
                    act.setWidth(width);
                return true;
            });      
	        
	    },		
		//you can call ActionBase.updateActions(catalog)  to update action status
        updateActions: function(category){
			var prms = arguments;
            Ext.Array.each(ActionBase.actions, function(act, index){
                if (act.category == category && act.updateStatus) 
                    act.updateStatus(prms[1],prms[2],prms[3],prms[4],prms[5],prms[6]);
                return true;
            });
        },
      
		//get action instance by itemId
        getAction: function(itemId){
            var res;
            Ext.Array.each(ActionBase.actions, function(act, index){
                if (act.itemId == itemId) {
                    res = act;                    
                    return false;
                }
                return true;
            });
            return res;
        }
    },
    setChecked : function(v){        
        this.callEach('setChecked', [v]);
    },
    setWidth : function(v){        
        this.callEach('setWidth', [v]);
    },
    
    constructor: function(config){
        config.scope = this;
        this.callParent([config]);
		if(config.updateStatus)
			this.updateStatus = config.updateStatus;
        WS.action.Base.actions.push(this);
    }
});
