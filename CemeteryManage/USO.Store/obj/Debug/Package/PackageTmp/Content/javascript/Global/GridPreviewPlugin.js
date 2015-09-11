//根据类型判断显示内容
function getContentByNType(ntype,data,bodyFields) {
	var reStr="";
	if(ntype == 0) {
		reStr ='你有一封新传真'+'<br/>';
		for (var i=0; i < bodyFields.length; i++) {
			if(bodyFields[i] =="param2") {
				reStr += '发件人'+':'+data[bodyFields[i]]+"<br/>";
				continue;
			}
			if(bodyFields[i] =="param3") {
				reStr += '传真位置'+':'+owerInternational(data[bodyFields[i]])+"";
			}
		};
		return reStr;
	}

	if(ntype == 1) {
		return reStr;
	}
	if(ntype == 2) {
		reStr ='你有一条新短信'+'<br/>';
		for (var i=0; i < bodyFields.length; i++) {
			if(bodyFields[i] =="param2") {
				reStr += '发件人'+':'+data[bodyFields[i]]+"<br/>";
				continue;
			}
			if(bodyFields[i] =="param3") {
				reStr += '短信位置'+':'+owerInternational(data[bodyFields[i]])+"";
			}
		};
		return reStr;
	}
	if(ntype == 3) {
		reStr ='传真发送成功'+'<br/>';
		for (var i=0; i < bodyFields.length; i++) {
			if(bodyFields[i] =="param2") {
				reStr += '收件人'+':'+data[bodyFields[i]]+"<br/>";
				continue;
			}
			if(bodyFields[i] =="param3") {
				reStr += '主题'+':'+data[bodyFields[i]]+"";
			}
		};
		return reStr;
	}
	if(ntype == 4) {
		reStr ='传真发送失败'+'<br/>';
		for (var i=0; i < bodyFields.length; i++) {
			if(bodyFields[i] =="param2") {
				reStr += '收件人'+':'+data[bodyFields[i]]+"<br/>";
				continue;
			}
			if(bodyFields[i] =="param3") {
				reStr += '主题'+':'+data[bodyFields[i]]+"";
			}
		};
		return reStr;
	}
	if(ntype == 5) {
		reStr ='短信发送成功'+'<br/>';
		for (var i=0; i < bodyFields.length; i++) {
			if(bodyFields[i] =="param2") {
				var myparam2 = data[bodyFields[i]];
				if(myparam2.indexOf('#') == -1){
					reStr += '短信条数'+':'+myparam2+"<br/>";
				}else{
					myparam2 = myparam2.substring(1,myparam2.length);
					reStr += '收件人'+':'+myparam2+"<br/>";
				}	
				continue;
			}
			if(bodyFields[i] =="param3") {
				reStr += '主题'+':'+data[bodyFields[i]]+"";
			}
		};
		return reStr;
	}
	if(ntype == 6) {
		reStr ='短信发送失败'+'<br/>';
		for (var i=0; i < bodyFields.length; i++) {
			if(bodyFields[i] =="param2") {
				var myparam2 = data[bodyFields[i]];
				if(myparam2.indexOf('#') == -1){
					reStr += '短信条数'+':'+myparam2+"<br/>";
				}else{
					myparam2 = myparam2.substring(1,myparam2.length);
					reStr += '收件人'+':'+myparam2+"<br/>";
				}				
				continue;
			}
			if(bodyFields[i] =="param3") {
				reStr += '主题'+':'+data[bodyFields[i]]+"";
			}
		};
		return reStr;
	}
	if(ntype == 7) {
		reStr ='有新文档等待你的审批'+'<br/>';
		for (var i=0; i < bodyFields.length; i++) {
			if(bodyFields[i] =="param2") {
				reStr += '提交人'+':'+data[bodyFields[i]]+"<br/>";
				continue;
			}
		};
		return reStr;
	}
	if(ntype == 8) {
		reStr ='通过审批'+'<br/>';
		for (var i=0; i < bodyFields.length; i++) {
			if(bodyFields[i] =="param2") {
				reStr += '任务ID'+':'+data[bodyFields[i]]+"<br/>";
				continue;
			}
			if(bodyFields[i] =="param3") {
				reStr += '结束时间'+':'+data[bodyFields[i]]+"";
			}
		};
		return reStr;
	}
	if(ntype == 9) {
		reStr ='未通过审批'+'<br/>';
		for (var i=0; i < bodyFields.length; i++) {
			if(bodyFields[i] =="param2") {
				reStr += '任务ID'+':'+data[bodyFields[i]]+"<br/>";
				continue;
			}
			if(bodyFields[i] =="param3") {
				reStr += '结束时间'+':'+data[bodyFields[i]]+"";
			}
		};
		return reStr;
	}
	if(ntype == 10) {
		for (var i=0; i < bodyFields.length; i++) {
			if(bodyFields[i] =="param2") {
				reStr += ""+data[bodyFields[i]]+"<br/>";
				continue;
			}
			// if(bodyFields[i] =="param3"){
			// reStr += ""+data[bodyFields[i]]+"";
			// }
		};
		return reStr;
	}

	if(ntype == 11) {
		for (var i=0; i < bodyFields.length; i++) {
			if(bodyFields[i] =="param3") {
				var msgSt = '';
				if(data[bodyFields[i]] != 0) {
					var wrStr = '';
					if((data[bodyFields[i]] & 0x01) > 0) {
						wrStr +='接收传真,';
					}
					if((data[bodyFields[i]] & 0x40) > 0) {
						wrStr +='工作流审批,';
					}
					if((data[bodyFields[i]] & 0x20) > 0) {
						wrStr +='管理员,';
					}
					wrStr = wrStr.substring(0,wrStr.length-1);
					msgSt = '赋予您的职责是'+':'+wrStr;
				} else {
					msgSt = '已经收回委任给你的职责';
				}
				reStr += ""+msgSt+"<br/>";
				continue;
			}
			// if(bodyFields[i] =="param3"){
			// reStr += ""+data[bodyFields[i]]+"";
			// }
		};
		return reStr;
	}
}

//Grid ux
Ext.define('Ext.ux.PreviewPlugin', {
	extend: 'Ext.AbstractPlugin',
	alias: 'plugin.preview',
	requires: ['Ext.grid.feature.RowBody', 'Ext.grid.feature.RowWrap'],
	hideBodyCls: 'x-grid-row-body-hidden',
	bodyField: '',
	bodyFields:[],
	previewExpanded: true,
	constructor: function(config) {
		this.callParent(arguments);
		var bodyField   = this.bodyField,
		bodyFields = this.bodyFields,
		hideBodyCls = this.hideBodyCls,
		section     = this.getCmp(),
		features = [{
			ftype: 'rowbody',
			getAdditionalData: function(data, idx, record, orig, view) {
				var o = Ext.grid.feature.RowBody.prototype.getAdditionalData.apply(this, arguments);
				var type = data[bodyField];

				var rowBodyStr= getContentByNType(type,data,bodyFields);

				Ext.apply(o, {
					rowBody: rowBodyStr,
					rowBodyCls: section.previewExpanded ? '' : hideBodyCls
				});
				return o;
			}
		},{
			ftype: 'rowwrap'
		}];

		section.previewExpanded = this.previewExpanded;
		if (!section.features) {
			section.features = [];
		}
		section.features = features.concat(section.features);
	},
	toggleExpanded: function(expanded) {
		var view = this.getCmp();
		this.previewExpanded = view.previewExpanded = expanded;
		view.refresh();
	}
});