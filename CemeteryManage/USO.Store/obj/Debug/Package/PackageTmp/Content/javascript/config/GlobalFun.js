//ExtJs 4.1 Bug修正
GlobalFun.fixedBugs = function () {
    //修正FireFox 18 以上Bug
    if (Ext.firefoxVersion >= 18) {
        var noArgs = [];

        Ext.override(Ext.Base, {
            callParent: function (args) {

                var method,
				superMethod = (method = this.callParent.caller) && (method.$previous ||
					((method = method.$owner ? method : method.caller) &&
						method.$owner.superclass[method.$name]));


                try {
                    if (!superMethod) {
                        method = this.callParent.caller;
                        var parentClass, methodName;

                        if (!method.$owner) {
                            if (!method.caller) {
                                throw new Error("Attempting to call a protected method from the public scope, which is not allowed");
                            }

                            method = method.caller;
                        }

                        parentClass = method.$owner.superclass;
                        methodName = method.$name;

                        if (!(methodName in parentClass)) {
                            throw new Error("this.callParent() was called but there's no such method (" + methodName +
							") found in the parent class (" + (Ext.getClassName(parentClass) || 'Object') + ")");
                        }
                    }

                } catch (e) {

                }
                return superMethod.apply(this, args || noArgs);


            }
        });


    }
    //修正Grid LoadMaskBug
    Ext.override(Ext.view.AbstractView, {
        onRender: function () {
            var me = this;
            me.callParent(arguments);
            if (me.loadMask && Ext.isObject(me.store)) {
                me.setMaskBind(me.store);
            }
        }
    });
    // 设置弹出对话框的文字
    GlobalConfig.newMessageBox.buttonText.yes = '是';
    GlobalConfig.newMessageBox.buttonText.no = '否';
    GlobalConfig.newMessageBox.buttonText.ok = '确定';
    GlobalConfig.newMessageBox.buttonText.cancel = '取消';

    // 重写alert
    Ext.override(Ext.window.MessageBox, {
        alert: function (cfg, msg, fn, scope) {
            if (Ext.isString(cfg)) {
                cfg = {
                    title: cfg,
                    msg: msg,
                    buttons: this.OK,
                    fn: fn,
                    scope: scope,
                    minWidth: this.minWidth < 330
                            ? 330
                            : this.minWidth
                };
            }
            return this.show(cfg);
        }
    });
};


//获取Ext.util.Cookies.get("sessiontoken")
GlobalFun.getSeesionToken = function () {
    return Ext.util.Cookies.get("sessiontoken");
};
//设置Ext.util.Cookies.set("sessiontoken",day)
GlobalFun.setSeesionToken = function (sessiontoken, day) {
    var myday = 30;
    if (day) {
        myday = day;
    }
    Ext.util.Cookies.set("sessiontoken", sessiontoken,
									new Date(new Date().getTime()
											+ (1000 * 60 * 60 * 24 * myday)));
};

// 取字段状态用width hidden
GlobalFun.resumeGridColumns = function (orcArr, colMap) {
    // 不用排序，取下width,hidden
    var sMap = new Ext.util.MixedCollection();
    Ext.Array.each(orcArr, function (item, index, alls) {
        sMap.add(item.id, item);
    });
    sMap.each(function (item, index, alls) {
        var rid = item.id.substring(item.id.indexOf('-') + 1,
            item.id.length);

        var tmItem = colMap.get(rid);
        if (!tmItem)
            return;

        if (item.width) {
            tmItem.width = item.width;
        }

        if (item.hidden || item.hidden == false) {
            tmItem.hidden = item.hidden;
        }
    });
};
//表格渲染通用方法
GlobalFun.UpdateRecord = function (value, metaData, record,rowIndex,colIndex,store,view) {
    return value;
};
//表格渲染实体类方法
GlobalFun.UpdateRecordForEntity = function (value, metaData, record,rowIndex,colIndex,store,view, disName) {
    if (value && value != null) {
        if (disName) {
            return GlobalFun.UpdateRecord(value[disName], metaData, record);
        } else {
            return GlobalFun.UpdateRecord(value["Name"], metaData, record);
        }
    }
    return GlobalFun.UpdateRecord("", metaData, record);
};
// 处理日期
//UTC转换本地时间
GlobalFun.UTCtoLocal = function (dateString) {
    var date = Ext.Date.parse(dateString + '+0000', 'Y-m-d H:i:sO');
    var m = Ext.util.Format.date(date, 'Y-m-d H:i:s');
    return m;
};
//本地转化为UTC时间
GlobalFun.LocalToUTCTime = function (i, endTime) {
    var now = new Date();
    var nowStr = Ext.util.Format.date(now, 'Y-m-d');
    var offset = now.getTimezoneOffset() * 60 * 1000;
    var resDate = new Date();
    if (endTime) {
        var nowDay = Ext.Date.parse(nowStr + " 23:59:59", "Y-m-d H:i:s");
        resDate.setTime(nowDay.getTime() + offset);

    } else {
        var nowDay = Ext.Date.parse(nowStr + " 00:00:00", "Y-m-d H:i:s");
        resDate.setTime(nowDay.getTime() + offset - (i * 24 * 3600 * 1000));
    }
    return Ext.util.Format.date(resDate, 'Y-m-d H:i:s');
};

//基本表格初始化
GridManager.BaseGridCfg = function (itemId, stateId, columns) {
    var config = {
        itemId: itemId,
        columnLines: true,
        multiSelect: true,
        frame: false,
        stateful: stateId ? true : false,
        border: false
    };
    if (stateId) {
        config.stateId = stateId;
    }
    if (columns) {
        config.columns = columns;
    }
    return config;
};

// 处理接口错误返回值 code
GlobalFun.errorProcess = function (code, swfokfun) {
    if (code == GlobalConfig.ErrorCode.ResNoUserName) {
        Ext.Msg.alert('登录失败', '登录失败！无效用户名', function () {
            if (swfokfun) {
                swfokfun();
            }
        });
        return true;
    }
    if (code == GlobalConfig.ErrorCode.SeesionTimeOut) {
        Ext.Msg.alert('登录信息失效', '登录信息失效！请重新登录', function () {
            window.location.href = "http://" + window.location.host + "/UserLogin";
        });
        return true;
    } 

    return false;
    
};

//判断是否可拖拽物
// 判断是否为可拖放目标 DropTarget
GlobalFun.isDropTarget = function (record, treeComponent) {

    var treeSel = treeComponent.getSelectionModel().getSelection()[0];
    // 如果是本节点
    if (treeSel.data.id == record.data.id) {
        return false;
    }
    //---他人委托收件箱 没有此功能
    if (treeSel.data.id.toString().indexOf('trwr') != -1) {
        return false;
    }

    return true;
};

//树节点国际化
GlobalFun.treeInternational = function (treeId) {
    //个人
    if (treeId == 'gr') {
        return '个人';
    }
    if (treeId == 'tr') {
        return '他人';
    }
    return '';
};
//树节点国际化 替换处理
GlobalFun.owerInternational = function (srcStr) {
    srcStr = srcStr.replace('@shoujianxiang@', '收件箱')
    .replace('@gxsjx@', '共享收件箱')
    .replace('@gr@', '个人')
    .replace('@gxwjj@', '共享文件夹')
    .replace('@gx@', '共享');

    return srcStr;
}

//ComboStore默认选择Frist
GlobalFun.comboSelectFirstOrDefaultVal = function (com, defaultVal) {
    com.getStore().getProxy().extraParams.sessiontoken = GlobalFun.getSeesionToken();
    com.getStore().load(function (records, operation, success) {
        if (defaultVal) {
            com.setValue(parseInt(defaultVal));
        } else {
            if (success && records.length > 0) {
                com.setValue(records[0].data.Id);
            }
        }
    });
}


//表格过滤菜单项目初始化
/*param = {
    group 分组名称
    checked 是否选中
    text 显示文本
    myVal 项目值
    firstClear 是否为首项清除条件项
    componet 需要添加菜单的Menu控件
    target 目标Grid
    filterKey GridFilter的键值
}*/
GridManager.GridFilterMenuCheckItemInit = function (param) {
    var menuCheckItem = {
        group: param.group,
        text: param.text,
        myVal: param.myVal
    };

    if (param.checked) {
        menuCheckItem.checked = true;
    }

    if (param.firstClear) {
        menuCheckItem.handler = function (btn, eve, suppressLoad) {
            var panel = param.target;
            var menu = param.componet;
            menu.setText(param.text);
            menu.setTooltip(param.text);

            panel.getStore().filterMap.removeAtKey(param.filterKey);
            if (suppressLoad) {
                this.setChecked(suppressLoad);
                return;
            }

            panel.loadGrid();
        };
    } else {
        menuCheckItem.handler = function (btn, eve, suppressLoad) {
            var panel = param.target;
            var menu = param.componet;
            menu.setText(param.text);
            menu.setTooltip(param.text);

            if (!panel.getStore().filterMap.containsKey(param.filterKey)) {
                panel.getStore().filterMap.add(param.filterKey, param.myVal);
            } else {
                panel.getStore().filterMap.replace(param.filterKey, param.myVal);
            }

            if (suppressLoad) {
                this.setChecked(suppressLoad);
                return;
            }
            panel.loadGrid();
        };
    }

    return menuCheckItem;
};

//表格过滤菜单创建
GridManager.CreateGridFilterMenu = function(componet, store, param) {
    componet.menu.add(GridManager.GridFilterMenuCheckItemInit({
        group: param.group,
        checked: true,
        text: param.text,
        myVal: 0,
        target: param.target,
        componet: param.componet,
        firstClear: true,
        filterKey: param.filterKey
    }));
    store.each(function(record, index, total) {
        componet.menu.add(GridManager.GridFilterMenuCheckItemInit({
            group: param.group,
            text: record.data.Name,
            myVal: record.data.Id,
            target: param.target,
            componet: param.componet,
            filterKey: param.filterKey
        }));
    });
};

//表格选择菜单创建
GridManager.CreateGridSelectCancelMenu = function (componet,grid) {
    componet.menu.add([{
        text: '全选/取消',
        handler : function(btn, eve, suppressLoad) {
            var sm = grid.getSelectionModel();
            if (sm.hasSelection()) {
                sm.deselectAll(true);
            } else {
                sm.selectAll(true);
            }
        }
    }, {
        text: '反选',
        handler: function (btn, eve, suppressLoad) {
            var sm = grid.getSelectionModel();
            for (var i = 0; i < grid.getStore().getCount() ; i++) {
                if (sm.isSelected(i)) {
                    sm.deselect(i, true);
                } else {
                    sm.select(i, true);
                }
            }
        }
    }]);
};


//设置click禁用标识
var stopClick = false;

//全局拖拽控件操作类
//设置可拖拽状态
DropDragControl.initializePatientDragZone = function (v,window) {
   
    v.dragZone = Ext.create('Ext.dd.DragZone', v.getEl(), {
        ddGroup: window.groupName,
        isTarget: false,
        getDragData: function(e) {
            //myDray = true;
            var sourceEl = e.getTarget(), d;

            //alert(Ext.fly(sourceEl).getXY()[0]);
            if (sourceEl) {
                //拖拽用图
                var selCount;
                selCount = window.pngClass.getPngSels().getCount();

                var oNewNode = document.createElement("div");
                oNewNode.innerHTML = "<span><img src='../../Content/images/pub/docDrag.png'/>x" + selCount + "</span>";
                //d = sourceEl.cloneNode(true);
                //d=oNewNode;
                oNewNode.id = Ext.id();
                return v.dragData = {
                    sourceEl: sourceEl,
                    repairXY: Ext.fly(sourceEl).getXY(),
                    ddel: oNewNode,
                    patientData: ''
                };
            }
        },
        getRepairXY: function() {
            //alert(this.dragData.repairXY);
            return this.dragData.repairXY;
        },
        endDrag: function() {
            stopClick = true;
            (new Ext.util.DelayedTask(function() {
                stopClick = false;
            })).delay(50);
        },
        //animRepair:false,
        onInvalidDrop: function(target, e, id) {

            var me = this;
            //myDray = false;
            //patientImg = me.id;
            this.beforeInvalidDrop(target, e, id);
            if (this.cachedTarget) {
                if (this.cachedTarget.isNotifyTarget) {
                    this.cachedTarget.notifyOut(this, e, this.dragData);
                }
                this.cacheTarget = null;
            }
            this.proxy.repair(this.getRepairXY(e, this.dragData), this.afterRepair, this);

            if (this.afterInvalidDrop) {
                this.afterInvalidDrop(e, id);
            }

        }
    });
};

//取消可拖拽状态
DropDragControl.uninitPatientDragZone = function(v) {
    if (v.dragZone) {
        v.dragZone.destroy();
    }
    ;
};

//取消可拖放状态
DropDragControl.uninitHospitalDropZone = function(v) {
    if (v.dropZone) {
        v.dropZone.destroy();
    };
};

//设置可拖放状态
DropDragControl.initializeHospitalDropZone = function (v, window) {
  
    v.dropZone = Ext.create('Ext.dd.DropZone', v.el, {
        ddGroup: window.groupName,
        getTargetFromEvent: function(e) {
            //alert(e.getTarget('.hospital-target'));
            return e.getTarget('.hospital-target');

        },
        onNodeOver: function(target, dd, e, data) {
            return Ext.dd.DropZone.prototype.dropAllowed;
        },
        onNodeDrop: function(target, dd, e, data) {
            //myDray = false;
            //patientImg = "";
            //alert('onNodeDrop');
            //操作PngContiner 做处理
            //var img;
            var curImg;//= compontPal.down('#' + target.id);
            var pngClass = window.pngClass;
           

            pngClass.getPngAllMini().each(function (item, index, allItems) {
                if (item.getEl() && (item.getEl().id == target.id)) {
                    curImg = item;
                    return;
                }
            });
            
            var sortNum = curImg.sortNum;
            var tombstoneId = curImg.tombstoneId;
            //var insertImgsSortNum = [];
            var insertImgs = [];
            var moveFrontImg = [];
            var moveBehindImgs = [];
            //插入到前方
            //当前位置
            var insertCount = parseInt(sortNum);
            var imageList = window.down('MiniPngDDSortPanel').query('image');
            if ((e.getPoint()[0] - curImg.getEl().getXY()[0]) <= (target.offsetWidth / 2)) {

                //获取移动到该位置前的图
                pngClass.getPngSels().each(function(item, index, allItems) {
                    //insertImgsSortNum.push(item.sortNum);
                    insertImgs.push(item.tombstoneId);
                });
                //获取当前位置前的图 不包括已选择的图
                Ext.Array.each(imageList, function(item, index, allItems) {
                    if (item.sortNum < insertCount) {
                        if (!Ext.Array.contains(insertImgs, item.tombstoneId)) {
                            moveFrontImg.push(item.tombstoneId);
                        }
                    }
                });
                //获取当前位置后的图，包括当前位置  不包括已选择的图
                Ext.Array.each(imageList, function(item, index, allItems) {
                    if (item.sortNum >= insertCount) {
                        if (!Ext.Array.contains(insertImgs, item.tombstoneId)) {
                            moveBehindImgs.push(item.tombstoneId);
                        }
                    }
                });
               
            } else {
                //获取移动到该位置前的图
                pngClass.getPngSels().each(function(item, index, allItems) {
                    insertImgs.push(item.tombstoneId);
                });
                //获取当前位置前的图 不包括已选择的图
                Ext.Array.each(imageList, function(item, index, allItems) {
                    if (item.sortNum <= insertCount) {
                        if (!Ext.Array.contains(insertImgs, item.tombstoneId)) {
                            moveFrontImg.push(item.tombstoneId);
                        }
                    }
                });
                //获取当前位置后的图，包括当前位置  不包括已选择的图
                Ext.Array.each(imageList, function(item, index, allItems) {
                    if (item.sortNum > insertCount) {
                        if (!Ext.Array.contains(insertImgs, item.tombstoneId)) {
                            moveBehindImgs.push(item.tombstoneId);
                        }
                    }
                });
            }
            var tombstoneIdList = moveFrontImg.concat(insertImgs, moveBehindImgs);
            //alert(pageList.join());
            //调用接口
            //调用
            var param = {};
            param.tombstoneIdList = tombstoneIdList.join();
            param.sessiontoken = Ext.util.Cookies.get("sessiontoken");
            //param.AreaId = pngClass.AreaId;
            //param.RowId = pngClass.RowId;
            WsCall.call(GlobalConfig.Controllers.TombstoneGrid.SortTombstonePng,'SortTombstonePng', param, function (response, opts) {
                (new Ext.util.DelayedTask()).delay(200, function () {
                    //清空选择数组
                    pngClass.getPngSels().each(function (item, index, allitems) {
                        item.getEl().setStyle('border', '1px solid black');
                        //设置为禁止拖拽
                        DropDragControl.uninitPatientDragZone(item);
                        //设置为可拖放状态
                        DropDragControl.initializeHospitalDropZone(item, window);
                    });
                    //清空选择数组
                    pngClass.getPngSels().clear();
                    //重置墓碑排序,重置Index
                    //alert(tombstoneIdList.join());
                    var newItemList = [];
                    var oldItemList = window.down('MiniPngDDSortPanel').items.items;
                    for (var i = 0; i < tombstoneIdList.length; i++) {
                        Ext.Array.each(oldItemList, function (item, index, allItems) {
                            if (item.tombstoneId == tombstoneIdList[i]) {
                                item.down('image').sortNum = i;
                                newItemList.push(item);
                            }
                        });
                    }
                    window.down('MiniPngDDSortPanel').removeAll(false);
                    window.down('MiniPngDDSortPanel').add(newItemList);
                    // window.down('MiniPngDDSortPanel')
                });
            }, function (response, opts) {
               if (!GlobalFun.errorProcess(response.code)) {
                    Ext.Msg.alert('失败', response.msg);
               }
            }, true, "请稍候...", window.el);

            return true;
        }
    });
};

