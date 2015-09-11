/**
 * @author LynnChen
 */
Ext.define('WS.lib.WsCall', {
	alternateClassName: ['WsCall']
});

var constErrRes = {
	success:false,
	code:-1,
	msg:'response failed'
};
WsCall.addStatics({

	/**
	 * Call a server interface or function
	 * @fname {string} server interface/function name
	 * @param {object} call param
	 * @successCall {function} success callback function,callback params: result object {success:true,code:xxx,msg:xxx}, response: json call raw response object.
	 * @failureCall {function} success callback function,callback params: result object {success:false,code:xxx,msg:xxx}, response: json call raw response object.
	 * @showLoadMask {boolean} whether display loading mask
	 * @loadMsg {string} message displayed on loading mask
	 * @maskEl {element} element that loading mask displayed from
	 * @maskDelay {integer} delay time before loading mask displayed (ms)
	 */

	call: function(url,fname, param, successCall, failureCall, showLoadMask, loadMsg, maskEl,maskDelay,async) {
		//		 constErrRes = {
		//			success:false,
		//			code:-1,
		//			msg:'response failed'
		//		};		
		param.req = 'call';
		
		
		param.callname = fname;
		var isAsync = async==false?async:true;
		var doslm = (typeof showLoadMask == 'undefined' || showLoadMask === null) || showLoadMask == true;
		if (doslm) {
			var callMask;
			var taskWait = new Ext.util.DelayedTask( function() {

				if (loadMsg) {
					callMask = maskEl ? maskEl : Ext.getBody();
					callMask.mask(loadMsg);
					// callMask = new Ext.LoadMask(maskEl ? maskEl : Ext.getBody(), {
					// msg: loadMsg
					// });
				} else {
					callMask = maskEl ? maskEl : Ext.getBody();
					callMask.mask('请稍候...');
					// callMask = new Ext.LoadMask(maskEl ? maskEl : Ext.getBody(), {
					// msg: "请稍候..."
					// });
				}
				//callMask.show();
			});
			taskWait.delay(maskDelay?maskDelay:100);
		}
		Ext.Ajax.request({
		    url: url,
			method: 'POST',
			timeout:30000,
			async:isAsync ,			
			success: function(response, opts) {
				if (taskWait)
					taskWait.cancel();
				if (typeof callMask != 'undefined' && callMask !== null)
					callMask.unmask();
				//callMask.hide();
				if(response.responseText.length>0 && response.responseText != 'null') {
					var res = Ext.JSON.decode(response.responseText);
					if(res.success == true)
						successCall(res, response,opts);
					else
						failureCall(res,response, opts);
				} else {
					constErrRes.msg = 'response failed (call:' + fname+');' + response.responseText;
					failureCall(constErrRes,response, opts);
				}
			},
			failure: function(response, opts) {
				if (taskWait)
					taskWait.cancel();
				if (typeof callMask != 'undefined' && callMask !== null)
					callMask.unmask();
				//callMask.hide();
				constErrRes.msg = 'response failed(ajax) (call:' + fname+')';
				failureCall(constErrRes,response, opts);
			},
			headers: {
				'AJaxCall': 'true'
			},
			params: param
		});
	},
	downloadFile: function(url,rcName, param) {
		var me = this;
		var sParam = Ext.Object.toQueryString(param);
		if (sParam.length > 0)
			sParam = '&' + sParam;
		var url = url + '?req=rc&rcname=' + rcName + sParam;
		if (typeof(me.iframe) == "undefined") {
			var iframe = document.createElement("iframe");
			me.iframe = iframe;
			document.body.appendChild(me.iframe);
		}
		me.iframe.src = url;
		me.iframe.style.display = "none";
	},
	callchain: function(callname) {
		var host = window.location.host;
		//alert(host);
		var name = userInfoData.accountName;
		var iframe = document.getElementById('callchain');
		if(iframe) {
			document.body.removeChild(iframe);
		}
		iframe = document.createElement("script");
		iframe.id="callchain";
		if (iframe.attachEvent) {//Ext.isIE
			iframe.onreadystatechange  = function() {
				if (this.readyState == "loaded") {

				}
			}
		} else {
			iframe.onload = function() {

			};
			iframe.onerror  = function() {

			}
		}
		var ranid = Ext.id()+(new Date()).getTime();
		iframe.src = "http://"+localPt.ip+":"+localPt.port+"/ifram.html?call="+callname+"&hosturl="+host+"&sessiontoken="+sessionToken+"&username="+name+"&ranid="+ranid+"@";
		document.body.appendChild(iframe);
	},
	scoutlogin: function(callname,fileid,callback) {
		var href = window.location.href;
		//alert(host);
		var name = userInfoData.accountName;
		var iframe = document.getElementById('scoutlogin');
		if(iframe) {
			document.body.removeChild(iframe);
		}
		iframe = document.createElement("script");
		iframe.id="scoutlogin";
		if (iframe.attachEvent) {//Ext.isIE
			iframe.onreadystatechange  = function() {
				if (this.readyState == "loaded") {
					if(callback){
						(new Ext.util.DelayedTask()).delay(50, function() {
							callback();
						});
					}
				}
			}
		} else {
			iframe.onload = function() {
				if(callback){	
						(new Ext.util.DelayedTask()).delay(500, function() {
							callback();
						});
					}
			};
			iframe.onerror  = function() {
				if(callback){
						(new Ext.util.DelayedTask()).delay(500, function() {
							callback();
						});
					}
			}
		}
		var fid = '';
		if(fileid){
			fid = fileid;
		}
		var ranid = Ext.id()+(new Date()).getTime();
		iframe.src = "http://"+scoutLogin.ip+":"+scoutLogin.port+"/ifram1.html?call="+callname+"&fileid="+fid+"&hrefurl="+href+"&sessiontoken="+sessionToken+"&username="+name+"&ranid="+ranid+"@";
		document.body.appendChild(iframe);
	},
	callOtherDomain: function(absPath,fileId,winType) {
		//alert( userConfig.prTwindow.location.hostname;ype);

		var path = encodeURI(absPath);
		var iframe = document.getElementById('otherDomain');
		if(iframe) {
			document.body.removeChild(iframe);
		}
		var fid = fileId;

		var param = {};
		param.sessiontoken = sessionToken;

		// 调用
		WsCall.call('getjsessionid', param, function(response, opts) {
			var jsid = response.data;

			Ext.getBody().mask('正在使用本地打印机,请稍候...');
			//callMask.show();
			//JSESSIONID

			iframe = document.createElement("script");
			iframe.id="otherDomain";

			function afterifload() {
				var param = {};
				param.sessiontoken = sessionToken;
				//param.jsid = jsid;
				// 调用
				WsCall.call('getiframemsg', param, function(response, opts) {
					//callMask.hide();
					Ext.getBody().unmask();
					var data = response.data;
					iframeFileUp(data,winType);
				}, function(response, opts) {
					var fpa = winType.down('#filePath');
					if(fpa)
						fpa.reset();
					//callMask.hide();
					Ext.getBody().unmask();
					if(response.code == 0x6F000125) {
						return;
					}

					if(!errorProcess(response.code)) {
						Ext.Msg.alert('失败', response.msg);
					}
				}, false);
			}

			if (iframe.attachEvent) {//Ext.isIE
				// iframe.attachEvent("onload", function() {
				// afterifload();
				// });
				iframe.onreadystatechange  = function() {
					if (this.readyState == "loaded") {
						afterifload();
					}
				}
			} else {
				iframe.onload = function() {
					afterifload();
				};
				iframe.onerror  = function() {
					afterifload();
				}
			}
			var host = window.location.host;
			var ranid = Ext.id()+(new Date()).getTime();

			var isdoc = false;
			if(winType.pngGroup == 'docwin') {
				isdoc = true;
			}
			iframe.src = "http://"+localPt.ip+":"+localPt.port+"/ifram.html?call=file&localurl="+host+"&isdoc="+isdoc+"&pttype="+userConfig.prType+"&sessiontoken="+sessionToken+"&jsid="+jsid+"&fileid="+fid+"&ranid="+ranid+"@";
			document.body.appendChild(iframe);

		}, function(response, opts) {
			if(!errorProcess(response.code)) {
				Ext.Msg.alert('失败', response.msg);
			}
		}, true,'',Ext.getBody(),10);
	}
});
Ext.define('html5uploadclass', {
	config: {		
		xhr:null,
		fisrtLoad:true,
		file:null,
		formName:'',
		files:null,
		msgidArr:[],
		index:1,
		length:0,
		url:'',
		html5UploadInint: function(files,url,formName,eventFuns) {
			var me = this;
			if(me.index == 1){
				me.length = files.length;
			}		
			me.files = files;	
			me.file = files[me.index-1];
			me.formName = formName;
			me.xhr = new XMLHttpRequest();
			if(eventFuns){
				me.xhr.upload.addEventListener("progress", eventFuns.uploadProgress, false);
				me.xhr.addEventListener("load", eventFuns.uploadComplete, false);
				me.xhr.addEventListener("error", eventFuns.uploadFailed, false);
				me.xhr.addEventListener("abort", eventFuns.uploadCanceled, false);
			}
			
			me.url = url;
		},
		uploadStart:function(){
			var me = this;
			
			var fd = new FormData();							
			fd.append(me.formName, me.file);
			me.xhr.open("POST", me.url);
			//me.xhr.setRequestHeader("Content-type", me.file.type);
			//console.log(encodeURI(me.file.name));		
			//me.xhr.setRequestHeader("X_FILE_NAME", encodeURI(me.file.name));
			me.xhr.send(fd);
		}
	},
	constructor: function (cfg) {
		this.initConfig(cfg);			
		this.xhr=null;
		this.fisrtLoad=true;
		this.file=null;
		this.files=null;
		this.msgidArr=[];
		this.index=1;
		this.length=0;
		this.url='';
		
	}
});