Ext.define('WS.lib.SearchField', {
    extend: 'Ext.form.field.Trigger',
    
    alias: 'widget.searchfield',
    
    trigger1Cls: Ext.baseCSSPrefix + 'form-clear-trigger',
    
    trigger2Cls: Ext.baseCSSPrefix + 'form-search-trigger',
    
    hasSearch : false,
    paramName: 'FullName',
    paramObject: false,
    paramNameArr: [],
    
    initComponent: function(){
    	var me = this;
        me.callParent(arguments);
        me.on('specialkey', function(f, e){
            if(e.getKey() == e.ENTER){
                this.onTrigger2Click();
            }
        }, me);
    },
    
    boxready: function(){
    	var me = this;
        me.callParent();    
        me.triggerEl.item(0).up('td').setWidth(0);   
        me.triggerEl.item(0).setDisplayed('none');  
    },
    
    onTrigger1Click : function(){
        var me = this,
            store = me.store,
            proxy = store.getProxy(),
            val;
            
        if (me.hasSearch) {
            me.setValue('');
            proxy.extraParams.filter = Ext.JSON.decode(proxy.extraParams.filter);
            if (me.paramObject) {
                Ext.Array.each(me.paramNameArr, function (key) {
                    delete proxy.extraParams.filter[key];
                    //移除filterMap
                    store.filterMap.removeAtKey(key);
                });
            } else {
                delete proxy.extraParams.filter[me.paramName];
                //移除filterMap
                store.filterMap.removeAtKey(me.paramName);
            }
            
            proxy.extraParams.filter = Ext.JSON.encode(proxy.extraParams.filter);
            //proxy.extraParams[me.paramName] = '';
            proxy.extraParams.start = 0;
	        proxy.extraParams.refresh = 1;
	        store.load();
	        proxy.extraParams.refresh = null;
            me.hasSearch = false;   
            me.triggerEl.item(0).up('td').setWidth(0);         
            me.triggerEl.item(0).setDisplayed('none');
            me.doComponentLayout();
        } 
        
    },

    onTrigger2Click : function(){
        var me = this,
            store = me.store,
            proxy = store.getProxy(),
            value = me.getValue();
            
        if (value.length < 1 ) {
            me.onTrigger1Click();
            return;
        }
        if (me.isValid()) {
            proxy.extraParams.filter = Ext.JSON.decode(proxy.extraParams.filter);
            if (me.paramObject) {
                Ext.Array.each(me.paramNameArr, function (key) {
                    var tempObj = {};
                    tempObj[me.paramName] = value;
                    proxy.extraParams.filter[key] = tempObj;
                    //加入filterMap
                    if (!store.filterMap.containsKey(key)) {
                        store.filterMap.add(key, tempObj);
                    } else {
                        store.filterMap.replace(key, tempObj);
                    }
                });
            } else {
                proxy.extraParams.filter[me.paramName] = value;
                //加入filterMap
                if (!store.filterMap.containsKey(me.paramName)) {
                    store.filterMap.add(me.paramName, value);
                } else {
                    store.filterMap.replace(me.paramName, value);
                }
            }
           

            proxy.extraParams.filter = Ext.JSON.encode(proxy.extraParams.filter);
            proxy.extraParams.start = 0;
            proxy.extraParams.refresh = 1;
            store.load();
            proxy.extraParams.refresh = null;
        }
      
       
        me.hasSearch = true;         
        me.triggerEl.item(0).up('td').setWidth(17);
        me.triggerEl.item(0).setDisplayed('block');
        me.doComponentLayout();   
    }
});
