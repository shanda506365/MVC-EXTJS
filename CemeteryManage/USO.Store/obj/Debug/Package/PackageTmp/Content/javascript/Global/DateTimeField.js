Ext.define('Ext.ux.form.field.DateTime', {
	extend:'Ext.form.FieldContainer',
	mixins: {
		field: 'Ext.form.field.Field'
	},
	alias: 'widget.datetimefield',
	layout: 'hbox',
	width: 200,
	height: 22,
	combineErrors: true,
	//msgTarget :'side',

	//类型
	mySqlType:'time',
	//后缀
	tmpcId:'',
	//是否查询
	doSearch:false,
	dateCfg: {},
	timeHCfg: {},
	timeMCfg: {},
	timeICfg: {},
	//hiddenCfg: {},

	initComponent: function() {
		var me = this;
		me.buildField();
		me.callParent();
	    this.dateField = this.down('datefield');
		this.timeHField = this.down('#sendTimeHouer'+this.tmpcId);
		this.timeMField = this.down('#sendTimeMin'+this.tmpcId);
		this.timeIField = this.down('#sendTimeSec'+this.tmpcId);
		 
		me.initField();
	},
	//@private
	buildField: function() {
		var me = this;
		me.items = [];
		if(me.mySqlType == 'date' || me.mySqlType == 'datetime') {
			me.items.push(
			Ext.apply({
				xtype: 'datefield',
				itemId:'sendDate'+me.tmpcId,
				name: 'sendDate'+me.tmpcId,
				format: 'Y-m-d',
				width: 90,
				//value: doSearch?Ext.util.Format.date(new Date(),'Y-m-d'):tmpDf[0],
				submitValue:false,
				blankText: '日期不能为空',
				allowBlank:false
			},me.dateCfg)
			);
		}
		if(me.mySqlType == 'time' || me.mySqlType == 'datetime') {
			me.items.push(
			Ext.apply({
				itemId:'sendTimeHouer'+me.tmpcId,
				name: 'sendTimeHouer'+me.tmpcId,
				xtype: 'numberfield',
				maxText: '时最大为23',
				minText: '时最小为0',
				submitValue:false,
				allowDecimals:false,
				//value: doSearch?0:timeDf[0],
				width: 40,
				minValue: 0,
				maxValue: 23,
				blankText: '不能为空',
				allowBlank: false
			},me.timeHCfg),
			Ext.apply({
				itemId:'sendTimeMin'+me.tmpcId,
				name: 'sendTimeMin'+me.tmpcId,
				xtype: 'numberfield',
				maxText: '分最大为59',
				minText: '分最小为0',
				submitValue:false,
				allowDecimals:false,
				//value: doSearch?0:timeDf[1],
				width: 40,
				minValue: 0,
				maxValue: 59,
				blankText: '不能为空',
				allowBlank: false
			},me.timeMCfg),
			Ext.apply({
				itemId:'sendTimeSec'+me.tmpcId,
				name: 'sendTimeSec'+me.tmpcId,
				xtype: 'numberfield',
				maxText: '秒最大为59',
				minText: '秒最小为0',
				submitValue:false,
				allowDecimals:false,
				//value: doSearch?0:timeDf[2],
				width: 40,
				minValue: 0,
				maxValue: 59,
				blankText: '不能为空',
				allowBlank: false
			},me.timeICfg)
			);
		}

		if(me.doSearch) {
			var serTmp = Ext.clone(me.items);

			if(me.mySqlType == 'date') {
				me.items.push({
					margin:'0 0 0 5',
					xtype: 'displayfield',
					width: 20,
					value: '--'
				});
			} else {
				me.items.push({
					margin:'0 0 0 5',
					xtype: 'displayfield',
					width: 20,
					value: '--'
				});
			}

			Ext.Array.each(serTmp, function(item,index,alls) {
				if(item.xtype == 'numberfield' || item.xtype == 'datefield') {
					item.name = 'end_' + item.name;
					item.itemId = 'end_' + item.itemId;
				}
				if(item.xtype == 'hidden') {
					item.name = 'end_' + item.name;
					item.myXtype = 'end_' + item.myXtype;
					item.itemId = 'end_' + item.itemId;
					item.submitValue= false;
				}
				me.items.push(item);
			});
		}

	},
	getValue: function() {
		var me = this,strTime='';

		if(me.mySqlType == 'date' ) {
			var strTime = Ext.util.Format.date(me.down('#sendDate'+me.tmpcId).getValue(), 'Y-m-d');
			if(me.doSearch) {
				var endTime = Ext.util.Format.date(me.down('#end_sendDate'+me.tmpcId).getValue(), 'Y-m-d');
				strTime += ','+endTime;
			}
			return strTime;
		}
		if(me.mySqlType == 'datetime') {
			var strTime = Ext.util.Format.date(me.down('#sendDate'+me.tmpcId).getValue(), 'Y-m-d');
			var tmpH = 	me.down('#sendTimeHouer'+me.tmpcId).getValue();
			tmpH = tmpH<10?('0'+tmpH):tmpH;
			var tmpM = 	me.down('#sendTimeMin'+me.tmpcId).getValue();
			tmpM = tmpM<10?('0'+tmpM):tmpM;
			var tmpS = me.down('#sendTimeSec'+me.tmpcId).getValue();
			tmpS = tmpS<10?('0'+tmpS):tmpS;
			strTime += " "+tmpH+":"+tmpM+":"+tmpS;

			if(me.doSearch) {
				var endTime = Ext.util.Format.date(me.down('#end_sendDate'+me.tmpcId).getValue(), 'Y-m-d');
				var tmpH1 =	me.down('#end_sendTimeHouer'+me.tmpcId).getValue();
				var tmpM1 = me.down('#end_sendTimeMin'+me.tmpcId).getValue();
				var tmpS1 = me.down('#end_sendTimeSec'+me.tmpcId).getValue();
				tmpH1 = tmpH1<10?('0'+tmpH1):tmpH1;
				tmpM1 = tmpM1<10?('0'+tmpM1):tmpM1;
				tmpS1 = tmpS1<10?('0'+tmpS1):tmpS1;
				endTime += " "+tmpH1+":"+tmpM1+":"+tmpS1;

				strTime += ','+endTime;
			}

			return strTime;
		}
		if(me.mySqlType == 'time') {
			var tmpH = 	me.down('#sendTimeHouer'+me.tmpcId).getValue();
			tmpH = tmpH<10?('0'+tmpH):tmpH;
			var tmpM = 	me.down('#sendTimeMin'+me.tmpcId).getValue();
			tmpM = tmpM<10?('0'+tmpM):tmpM;
			var tmpS = me.down('#sendTimeSec'+me.tmpcId).getValue();
			tmpS = tmpS<10?('0'+tmpS):tmpS
			var strTime = tmpH+":"+tmpM+":"+tmpS;

			if(me.doSearch) {
				var tmpH1 =	me.down('#end_sendTimeHouer'+me.tmpcId).getValue();
				var tmpM1 = me.down('#end_sendTimeMin'+me.tmpcId).getValue();
				var tmpS1 = me.down('#end_sendTimeSec'+me.tmpcId).getValue();
				tmpH1 = tmpH1<10?('0'+tmpH1):tmpH1;
				tmpM1 = tmpM1<10?('0'+tmpM1):tmpM1;
				tmpS1 = tmpS1<10?('0'+tmpS1):tmpS1;
				var endTime = tmpH1+":"+tmpM1+":"+tmpS1;
				strTime += ','+endTime;
			}

			return strTime
		}
		return null;
	},
	setValue: function(value,useval) {
		var me = this;
		//var timeDf = value.split(':');
		if(me.mySqlType == 'date' || me.mySqlType == 'datetime') {
			me.dateField.setValue(me.dateCfg.value?me.dateCfg.value:Ext.util.Format.date(new Date(),'Y-m-d'));
		}
		if(me.mySqlType == 'time' || me.mySqlType == 'datetime') {
			me.timeHField.setValue(me.timeHCfg.value?me.timeHCfg.value:0);
			me.timeMField.setValue(me.timeMCfg.value?me.timeMCfg.value:0);
			me.timeIField.setValue(me.timeICfg.value?me.timeICfg.value:0);	
			if(useval){				
				var timeH = value.split(':')[0];
				var timeM = value.split(':')[1];
				var timeI = value.split(':')[2];
				var date;
				if(me.mySqlType == 'datetime'){
					date = value.split('T')[0];
				}
				if(date){
					date = timeH.split('T')[0];
					timeH = timeH.split('T')[1];
					me.dateField.setValue(date);
				}
				me.timeHField.setValue(timeH);
				me.timeMField.setValue(timeM);
				me.timeIField.setValue(timeI);	
			}		
		}
	},
	isFileUpload: function () {
		return this.inputType === 'file';
	},
	getSubmitData: function() {
		var me = this;
		var value = me.getValue();
		var name = me.name;
		var data = {};		
		if (!me.disabled && me.submitValue && !me.isFileUpload()) {
			if(value && me.mySqlType == 'datetime'){
				var re = /\s/g; 
				value = value.replace(re,'T')
			}
			data[''+name+'']=value;
			return (value&&name) ? data : null;
		}

	},
	setFieldStyle: function(style) {
        var me = this,
            inputEl = me.timeHField?me.timeHField.inputEl:null,
            inputEl1 = me.timeMField?me.timeMField.inputEl:null,
            inputEl2 = me.timeIField?me.timeIField.inputEl:null,
            inputEl3 = me.dateField?me.dateField.inputEl:null;
        if (inputEl) {
            inputEl.applyStyles(style);
        }
         if (inputEl1) {
            inputEl1.applyStyles(style);
        }
         if (inputEl2) {
            inputEl2.applyStyles(style);
        }
          if (inputEl3) {
            inputEl3.applyStyles(style);
        }
        me.fieldStyle = style;
        me.timeHField.fieldStyle = style;
        me.timeMField.fieldStyle = style;
        me.timeMField.fieldStyle = style;
    },
	setReadOnly: function(readOnly) {
        var me = this,
            inputEl = me.timeHField?me.timeHField.inputEl:null,
            inputEl1 = me.timeMField?me.timeMField.inputEl:null,
            inputEl2 = me.timeIField?me.timeIField.inputEl:null,
            inputEl3 = me.dateField?me.dateField.inputEl:null;
           	
        readOnly = !!readOnly;
        me[readOnly ? 'addCls' : 'removeCls'](me.readOnlyCls);
        me.readOnly = readOnly;
        if (inputEl) {
            inputEl.dom.readOnly = readOnly;
            me.timeHField.setReadOnly(readOnly);
             //me.timeHField.spinUpEl.setVisible(false);
            //me.timeHField.spinDownEl.setVisible(false);
            
        } else if (me.rendering) {
            me.timeHField.setReadOnlyOnBoxReady = true;
        }
        if (inputEl1) {
            inputEl1.dom.readOnly = readOnly;
             me.timeMField.setReadOnly(readOnly);
           // me.timeMField.spinUpEl.setVisible(false);
           // me.timeMField.spinDownEl.setVisible(false);
        } else if (me.rendering) {
            me.timeMField.setReadOnlyOnBoxReady = true;
        }
        if (inputEl2) {
            inputEl2.dom.readOnly = readOnly;
            me.timeIField.setReadOnly(readOnly);
            //me.timeIField.spinUpEl.setVisible(false);
           // me.timeIField.spinDownEl.setVisible(false);
        } else if (me.rendering) {
            me.timeIField.setReadOnlyOnBoxReady = true;
        }
        if (inputEl3) {
            inputEl3.dom.readOnly = readOnly;   
            me.dateField.setReadOnly(readOnly);        
           // me.dateField.triggerEl.setVisible(false);           
        } else if (me.rendering) {
            me.dateField.setReadOnlyOnBoxReady = true;
        }
        
        me.updateLayout();
        me.fireEvent('writeablechange', me, readOnly);
    }
})