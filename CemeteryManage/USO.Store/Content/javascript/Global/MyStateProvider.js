//状态保存临时变量
myStates = {
	infaxgridState: {},
	infaxgridFilter: {},
	outfaxgridState: {},
	outfaxgridFilter: {},
	succoutfaxgridState: {},
	succoutfaxgridFilter: {},
	draftgridState: {},
	addressgridState: {},
	docgridState: {},
	docgridFilter: {},
	taskgridState:{},
	taskgridFilter:{},
	wfrulegridState:{},
	smsinfaxgridState:{},
	smsinfaxgridFilter:{},	
	smsoutfaxgridState:{},
	smsoutfaxgridFilter:{},
	westPanelState: {},
	centerPanelState: {},
	centerCenterPanelState: {},
	centerSouthPanelState: {},
	southPanelState: {},
	printerSrc: {},//打印机使用来源
	printerType: {},//打印机类型
	//docprinterSrc:{},//doc打印机使用来源
	//docprinterType:{},//doc打印机类型
	faxtoolBtn: {},
	currtree: {},//当前所显示的树panel
	searchState: {},//当前 查找 及表单查找 state
	smscbState:{}//短信 回复及签名 checkbox状态	
};

var setSerStates = new Ext.util.DelayedTask();

var stateSorArr = ['infaxgridState','infaxgridFilter','outfaxgridState','outfaxgridFilter',
'succoutfaxgridState','succoutfaxgridFilter','draftgridState','addressgridState',
'docgridState','docgridFilter','taskgridState','taskgridFilter','wfrulegridState',
'smsinfaxgridState','smsinfaxgridFilter','smsoutfaxgridState','smsoutfaxgridFilter',
'westPanelState','centerPanelState','centerCenterPanelState'
,'centerSouthPanelState','southPanelState','printerSrc','printerType'
,'faxtoolBtn','currtree','searchState','smscbState'];

Ext.define('WS.lib.MyStateProvider', {
	extend: 'Ext.state.Provider',
	constructor : function(config) {
		var me = this;

		config = config || {};
		if (Ext.isDefined(config.sessiontoken)) {
			me.sessiontoken = config.sessiontoken;
		}
		//alert(config.userName);
		me.callParent(arguments);
		me.state = me.readMyStates();
	},
	// private
	set : function(name, value) {
		var me = this;

		if(typeof value == "undefined" || value === null) {
			me.clear(name);
			return;
		}
		me.setServerState(name, value);
		me.callParent(arguments);
	},
	// private
	clear : function(name) {
		this.clearServerState(name);
		this.callParent(arguments);
	},
	// private
	readMyStates : function() {
		//从服务器一次性读取配置
		var me = this;
		//调用call 读取数据
		var states = myStates;
		var param = {};
		param.sessiontoken = GlobalFun.getSeesionToken();
		param.key = Ext.JSON.encode(stateSorArr);
		//param.key = "infaxgridState,infaxgridFilter,outfaxgridState,outfaxgridFilter,succoutfaxgridState,succoutfaxgridFilter,draftgridState,addressgridState,westPanelState,centerPanelState,centerCenterPanelState,centerSouthPanelState,southPanelState";

		//调用
		WsCall.call('readmystates', param, function(response, opts) {
			//alert(response.data);
			var datas = Ext.JSON.decode(response.data);
			Ext.Array.each(datas, function(val,index,alls) {
				var tmpKey = stateSorArr[index];
				val.stateSaved = true;
				myStates[tmpKey] = val;
			});
			
			//states = myStates
		}, function(response, opts) {
		}, false,null,null,null,false);
		//alert(me.sessiontoken);

		return states;
	},
	// private
	setServerState : function(name, value) {
		//alert(name);
		var me = this;
		value.stateSaved = false;

		//上传服务器
		for (key in stateSorArr) {
			var ssArr = stateSorArr[key];
			if(ssArr == name) {
				//alert('be-'+stateSorArr[key]+"--"+name+"--"+Ext.JSON.encode(myStates[stateSorArr[key]]));
				var curState = myStates[ssArr];

				var tmpAr = [];
				if( curState && value.columns && curState.columns) {
					Ext.Array.each(value.columns, function(item) {
						var hasP = false;
						Ext.Array.each(curState.columns, function(it) {
							if(it.id == item.id) {
								item.HasV = true;
								hasP = true;
								it = Ext.Object.merge(it,item);
							}
						});
						if(hasP) {
							tmpAr.push(item);
						}
					});
					Ext.Array.each(value.columns, function(it) {
						if(!it.HasV) {
							tmpAr.push(it);
						}
					});
					curState.columns = tmpAr;
					//alert(Ext.JSON.encode(tmpAr));
					//alert(Ext.JSON.encode(value.columns));
					//alert(Ext.JSON.encode(curState.columns)+'kkkkkkkkkkkkkkkk');
					value.columns = curState.columns;
				}
				curState = value;
				//alert(Ext.JSON.encode(curState.columns));

				setSerStates.cancel();
				setSerStates.delay(6000, function() {
					for (key1 in stateSorArr) {
						//alert('set');
						var tmp = myStates[stateSorArr[key1]];

						if(tmp && !tmp.stateSaved) {
							var tmpStr = Ext.JSON.encode(tmp);
							if(tmpStr != "{}") {
								//alert('call');
								//alert(stateSorArr[key1]+"--"+tmpStr);
								//调用call 上传数据
								var param = {};
								param.sessiontoken = GlobalFun.getSeesionToken();
								param.key = stateSorArr[key1];
								param.value = tmpStr;
								//alert(tmpStr);
								// 调用
								tmp.stateSaved = true;
								WsCall.call('setmystates', param, function(response, opts) {
								}, function(response, opts) {
								}, false);
							}

						}

					}

				});
				//alert(name+"--"+Ext.JSON.encode(value));
				//alert(name+":"+value);
				break;
			}

		}

	},
	// private
	clearServerState : function(name) {
		var me = this;
		//alert('clear');

	}
});