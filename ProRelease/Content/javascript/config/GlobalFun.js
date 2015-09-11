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
    //修正IE拖拽无效
    if (Ext.isIE) {
        Ext.override(Ext.dd.DragDropManager, {
            getLocation: function (oDD) {
                if (!this.isTypeOfDD(oDD)) {
                    return null;
                }
                if (oDD.getRegion) {
                    return oDD.getRegion();
                }

                var el = oDD.el, pos, x1, x2, y1, y2, t, r, b, l;

                try {
                    pos = Ext.Element.getXY(el);
                } catch (e) { }

                if (!pos) {
                    return null;
                }

                x1 = pos[0];
                x2 = x1 + 77;//根据img宽度设置
                y1 = pos[1];
                y2 = y1 + 60;//根据img高度设置

                t = y1 - oDD.padding[0];
                r = x2 + oDD.padding[1];
                b = y2 + oDD.padding[2];
                l = x1 - oDD.padding[3];

                return new Ext.util.Region(t, r, b, l);
            }
        });
    }
    Ext.ouveride
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

    //设置日期验证
    Ext.apply(Ext.form.field.VTypes, {
        daterange: function (val, field) {
            var date = field.parseDate(val);

            if (!date) {
                return false;
            }
            if (field.startDateField && (!field.dateRangeMax || (date.getTime() != field.dateRangeMax.getTime()))) {
                var start = field.up('panel').down('#' + field.startDateField);
                start.setMaxValue(date);
                start.validate();
                field.dateRangeMax = date;
            } else if (field.endDateField && (!field.dateRangeMin || (date.getTime() != field.dateRangeMin.getTime()))) {
                var end = field.up('panel').down('#' + field.endDateField);
                end.setMinValue(date);
                end.validate();
                field.dateRangeMin = date;
            }
            return true;
        },
        daterangeText: '起始时间不能大于截止时间'

    });
};
//跳转页面
GlobalFun.ReDirectUrl = function (url) {
    var protocol = window.location.protocol;
    var host = window.location.host;
    window.location.href = protocol + "//" + host + "/" + url;
};

//获取Ext.util.Cookies.get("sessiontoken")
GlobalFun.getSeesionToken = function () {
    return Ext.util.Cookies.get("login_sessiontoken");
};
//设置Ext.util.Cookies.set("sessiontoken",day)
GlobalFun.setSeesionToken = function (sessiontoken, day) {
    var myday = 30;
    if (day) {
        myday = day;
    }
    Ext.util.Cookies.set("login_sessiontoken", sessiontoken,
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
GlobalFun.UpdateRecord = function (value, metaData, record, rowIndex, colIndex, store, view) {
    return value;
};
//表格渲染短日期通用方法
GlobalFun.UpdateRecordForShortDate = function (value, metaData, record, rowIndex, colIndex, store, view) {
    var date = new Date(value.replace(/-/g, '/'));
    return Ext.util.Format.date(date, 'Y-m-d');
};
//表格渲染实体类方法
GlobalFun.UpdateRecordForEntity = function (value, metaData, record, rowIndex, colIndex, store, view, disName) {
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
    if (code == GlobalConfig.ErrorCode.LoginFail) {
        Ext.Msg.alert('登录失败', '登录失败！无效用户名或密码', function () {
            if (swfokfun) {
                swfokfun();
            }
        });
        return true;
    }
    if (code == GlobalConfig.ErrorCode.SeesionTimeOut) {
        Ext.getBody().unmask();
        Ext.Msg.alert('登录信息失效', '登录信息失效！请重新登录', function () {
            Ext.getBody().mask("请稍候...");
            GlobalFun.ReDirectUrl("UserLogin");
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
    //com.getStore().getProxy().extraParams.sessiontoken = GlobalFun.getSeesionToken();
    //com.getStore().load(function (records, operation, success) {
    var store = com.getStore();

    if (defaultVal) {
        (new Ext.util.DelayedTask()).delay(50, function () {
            com.setValue(parseInt(defaultVal));
        });
    } else {
        com.setValue(store.getRange()[0].data.Id);
        // if (success && records.length > 0) {
        //com.setValue(records[0].data.Id);
        //}
    }
};


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
GridManager.CreateGridFilterMenu = function (componet, store, param) {
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
    store.each(function (record, index, total) {
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
GridManager.CreateGridSelectCancelMenu = function (componet, grid) {
    componet.menu.add([{
        text: '全选/取消',
        handler: function (btn, eve, suppressLoad) {
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
DropDragControl.initializePatientDragZone = function (v, window) {

    v.dragZone = Ext.create('Ext.dd.DragZone', v.getEl(), {
        ddGroup: window.groupName,
        isTarget: false,
        getDragData: function (e) {
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
        getRepairXY: function () {
            //alert(this.dragData.repairXY);
            return this.dragData.repairXY;
        },
        endDrag: function () {
            stopClick = true;
            (new Ext.util.DelayedTask(function () {
                stopClick = false;
            })).delay(50);
        },
        //animRepair:false,
        onInvalidDrop: function (target, e, id) {

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
DropDragControl.uninitPatientDragZone = function (v) {
    if (v.dragZone) {
        v.dragZone.destroy();
    }
    ;
};

//取消可拖放状态
DropDragControl.uninitHospitalDropZone = function (v) {
    if (v.dropZone) {
        v.dropZone.destroy();
    };
};

//设置可拖放状态
DropDragControl.initializeHospitalDropZone = function (v, window) {

    v.dropZone = Ext.create('Ext.dd.DropZone', v.el, {
        ddGroup: window.groupName,
        getTargetFromEvent: function (e) {
            //alert(e.getTarget('.hospital-target'));
            return e.getTarget('.hospital-target');

        },
        onNodeOver: function (target, dd, e, data) {
            return Ext.dd.DropZone.prototype.dropAllowed;
        },
        onNodeDrop: function (target, dd, e, data) {
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
                pngClass.getPngSels().each(function (item, index, allItems) {
                    //insertImgsSortNum.push(item.sortNum);
                    insertImgs.push(item.tombstoneId);
                });
                //获取当前位置前的图 不包括已选择的图
                Ext.Array.each(imageList, function (item, index, allItems) {
                    if (item.sortNum < insertCount) {
                        if (!Ext.Array.contains(insertImgs, item.tombstoneId)) {
                            moveFrontImg.push(item.tombstoneId);
                        }
                    }
                });
                //获取当前位置后的图，包括当前位置  不包括已选择的图
                Ext.Array.each(imageList, function (item, index, allItems) {
                    if (item.sortNum >= insertCount) {
                        if (!Ext.Array.contains(insertImgs, item.tombstoneId)) {
                            moveBehindImgs.push(item.tombstoneId);
                        }
                    }
                });

            } else {
                //获取移动到该位置前的图
                pngClass.getPngSels().each(function (item, index, allItems) {
                    insertImgs.push(item.tombstoneId);
                });
                //获取当前位置前的图 不包括已选择的图
                Ext.Array.each(imageList, function (item, index, allItems) {
                    if (item.sortNum <= insertCount) {
                        if (!Ext.Array.contains(insertImgs, item.tombstoneId)) {
                            moveFrontImg.push(item.tombstoneId);
                        }
                    }
                });
                //获取当前位置后的图，包括当前位置  不包括已选择的图
                Ext.Array.each(imageList, function (item, index, allItems) {
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
            WsCall.call(GlobalConfig.Controllers.TombstoneGrid.SortTombstonePng, 'SortTombstonePng', param, function (response, opts) {

                //(new Ext.util.DelayedTask()).delay(2000, function () {

                var area = window.down('#AreaIdItemId');
                var row = window.down('#RowIdItemId');
                if (area.isValid() && row.isValid()) {
                    pngClass.setAreaId(area.getValue());
                    pngClass.setRowId(row.getValue());
                    var miniPngDDSortPanel = window.down('MiniPngDDSortPanel');
                    miniPngDDSortPanel.removeAll();
                    pngClass.initPngPanel(miniPngDDSortPanel);
                }

                //清空选择数组
                //pngClass.getPngSels().each(function (item, index, allitems) {
                //    item.getEl().setStyle('border', '1px solid black');
                //    //设置为禁止拖拽
                //    DropDragControl.uninitPatientDragZone(item);
                //    //设置为可拖放状态
                //    DropDragControl.initializeHospitalDropZone(item, window);
                //});
                ////清空选择数组
                //pngClass.getPngSels().clear();
                //重置墓碑排序,重置Index
                //alert(tombstoneIdList.join());
                //var newItemList = [];
                //var oldItemList = window.down('MiniPngDDSortPanel').items.items;
                //var tempOldItemList = Ext.Array.clone(oldItemList);
                //for (var i = 0; i < tombstoneIdList.length; i++) {
                //    Ext.Array.each(tempOldItemList, function (item, index, allItems) {
                //        if (item.tombstoneId == tombstoneIdList[i]) {
                //            //item.tombstoneId 
                //            //tombstoneId
                //            //src
                //            //item.down('image').sortNum = i;
                //            var newItem = Ext.clone(item);
                //            newItemList.push(newItem);
                //        }
                //    });
                //}

                //window.down('MiniPngDDSortPanel').removeAll(false);
                //window.down('MiniPngDDSortPanel').add(newItemList);
                //});
            }, function (response, opts) {
                if (!GlobalFun.errorProcess(response.code)) {
                    Ext.Msg.alert('失败', response.msg);
                }
            }, true, "请稍候...", window.el);

            return true;
        }
    });
};

//设置Grid标题
GlobalFun.SetGridTitle = function (cardPanel, store, title) {
    if (store.filterMap.getCount() > 0 || store.getProxy().extraParams.DateFilter) {
        cardPanel.setTitle('查找结果 { ' + title + ' }');
    } else {
        cardPanel.setTitle('' + title + '');
    }
};

//树选择切换Grid联动方法
GlobalFun.TreeSelChangeGrid = function (activeNum, gridPanel, title, isDfGrid) {
    var cardPanel = GlobalConfig.ViewPort.down('#centerGridDisplayContainer');
    cardPanel.getLayout().setActiveItem(activeNum);
    if (isDfGrid) {
        cardPanel.setTitle('' + title + '');
        return;
    }
    if (!gridPanel.FirstLoad) {
        gridPanel.loadGrid();
        gridPanel.FirstLoad = true;
    } else {
        GlobalFun.SetGridTitle(cardPanel, gridPanel.getStore(), title);
    }
    gridPanel.fireEvent("selectionchange", gridPanel.getView(), gridPanel.getSelectionModel().getSelection());
};

//起始截止日期验证
GlobalFun.ValidDateStartEnd = function (dateStarField, dateEndField) {
    var sVal = dateStarField.getValue();
    var eVal = dateEndField.getValue();
    if (sVal && !eVal) {
        dateEndField.allowBlank = false;
        dateEndField.blankText = '日期区间必须完整';//return;
    } else if (eVal && !sVal) {
        dateStarField.allowBlank = false;
        dateStarField.blankText = '日期区间必须完整';
        //return;
    } else {
        dateEndField.allowBlank = true;
        dateStarField.allowBlank = true;
    }
};


//自定义FieldSet CheckBox选择变化Function
GlobalFun.onCheckChange = function (cmp, checked) {
    var fieldset = cmp.up('fieldset');
    if (fieldset.fistCheck) {
        return;
    }
    var cblist = fieldset.query('checkbox');
    Ext.Array.each(cblist, function (item) {
        item.setValue(checked);
    });
};
//自定义FieldSet 创建CheckBox
GlobalFun.createCheckboxCmp = function () {
    var me = this,
        suffix = '-checkbox';
    me.checkboxCmp = Ext.widget({
        xtype: 'checkbox',
        hideEmptyLabel: true,
        name: me.checkboxName || me.id + suffix,
        cls: me.baseCls + '-header' + suffix,
        itemId: me.myItemId,
        id: me.id + '-legendChk',
        listeners: {
            change: me.onCheckChange,
            scope: me
        }
    });
    return me.checkboxCmp;
};

//心跳，防过期
GlobalFun.Heartbeat = function () {
    (new Ext.util.DelayedTask(function () {
        GlobalConfig.HeartbeatRunner.stopAll();
        GlobalConfig.HeartbeatRunner.start({
            run: function () {
                //调用接口取值
                var param = {
                    sessiontoken: GlobalFun.getSeesionToken()
                };
                //调用
                WsCall.call(GlobalConfig.Controllers.Heartbeat, 'Heartbeat', param, function (response, opts) {

                }, function (response, opts) {
                    //Ext.Msg.alert('失败', response.msg);
                }, false);
            },
            interval: 60 * 10 * 1000 //10分钟
        });
    })).delay(50);
};

//权限判断方法
GlobalFun.IsAllowFun = function (funName) {
    var flag = false;
    if (GlobalConfig.CurrUserInfo.RoleDtos[0].FunctionsString.indexOf(funName) != -1) {
        flag = true;
    }
    return flag;
};

//表格查询过滤条件初始化方法
GlobalFun.GridSearchInitFun = function (keyName, isDel, store, value) {
    if (isDel) {
        store.filterMap.removeAtKey(keyName);
    } else {
        //加入filterMap
        if (!store.filterMap.containsKey(keyName)) {
            store.filterMap.add(keyName, value);
        } else {
            store.filterMap.replace(keyName, value);
        }
    }
};

//根据墓碑状态取得图片路径
GlobalFun.GetTomestoneImageSrc = function (state, tombstoneId, typeId) {
    var imgaeName = 'alone';
    if (typeId == 2) {
        imgaeName = 'double';
    } else if (typeId == 3) {
        imgaeName = 'specil';
    }

    if (state == 1) {
        imgaeName += '-empty';
    } else if (state == 2) {
        imgaeName += '-order';
    } else if (state == 3) {
        imgaeName += '-sale';
    } else if (state == 4) {
        imgaeName += '-zbury1';
    } else if (state == 5) {
        if (typeId == 2) {
            imgaeName += '-zbury3';
        } else {
            imgaeName += '-zbury';
        }
    } else {
        imgaeName += '-empty';
    }

    return '../../Content/images/tombstone/gif/' + imgaeName + '.gif?currpage=' + tombstoneId + '&randomTime=' + (new Date()).getTime();
};


//自动恢复收缩状态
GlobalFun.RefreshDetailCollapseState = function () {
    //判断是否已经自动收缩
    if (GlobalConfig.DetailAutoCollapse) {
        var detailPanel = GlobalConfig.ViewPort.down('#centerGridDetailContainer');
        detailPanel.expand();
        GlobalConfig.DetailAutoCollapse = false;
    }
};
//自动收缩
GlobalFun.DetailCollapse = function () {
    //自动收缩
    var detailPanel = GlobalConfig.ViewPort.down('#centerGridDetailContainer');
    if (!detailPanel.collapsed) {
        detailPanel.collapse();
        GlobalConfig.DetailAutoCollapse = true;
    }
};


//业务操作-点击墓碑图片Click方法
GlobalFun.JobTombPngClickFun = function (eve, tombstoneId, tombstoneInfo) {
    var node = TreeManager.MainItemListTree.getSelectionModel().getSelection();
    var msg = "";
    var title = "";
    if (node[0] && node[0].data.id == 201) {
        msg = "您确定要预订选定的墓碑吗？";
        title = "墓碑预订";
        if (tombstoneInfo.PaymentStatusId >= 2) {
            Ext.Msg.alert('消息', '该墓碑不可操作预订');
            return;
        }
        GlobalFun.ShowOrderTombstoneWin(title, tombstoneId, tombstoneInfo);
    } else if (node[0] && node[0].data.id == 202) {
        msg = "您确定要维护选定的墓碑吗？";
        title = "墓碑维护";
        if (tombstoneInfo.PaymentStatusId >= 3 || tombstoneInfo.PaymentStatusId <= 1) {
            Ext.Msg.alert('消息', '该墓碑不可操作维护');
            return;
        }
        GlobalFun.ShowSaleTombstoneWin(title, tombstoneId, tombstoneInfo);
    } else if (node[0] && node[0].data.id == 203) {
        msg = "您确定要落葬选定的墓碑吗？";
        title = "墓碑落葬";
        if (tombstoneInfo.PaymentStatusId >= 5 || tombstoneInfo.PaymentStatusId <= 2) {
            //Ext.Msg.alert('消息', '该墓碑不可操作落葬');
            //显示所有落葬的信息及申请人信息，提供增加落葬人、维护落葬人
            //二次
            GlobalFun.ShowBuryTombstoneAgainWin(title, tombstoneId, tombstoneInfo);
            return;
        }
        //首次
        GlobalFun.ShowBuryTombstoneWin(title, tombstoneId, tombstoneInfo);
    }

    //GlobalConfig.newMessageBox.show({
    //    title: '提示',
    //    msg: msg,
    //    buttons: Ext.MessageBox.YESNO,
    //    closable: false,
    //    fn: function (btn) {
    //        if (btn == 'yes') {

    //}
    //    },
    //    icon: Ext.MessageBox.QUESTION
    //});
};



//业务操作-预订墓碑
GlobalFun.ShowOrderTombstoneWin = function (title, tombstoneId, tombstoneInfo, dfData) {

    var tempWin = Ext.create('Ext.window.Window', {
        title: title,
        defaultFocus: 'AliasItemId',
        iconCls: '',
        record: false,
        height: 380,
        width: 500,
        modal: true,
        resizable: false,
        layout: 'fit',
        items: [{
            xtype: 'form',
            itemId: 'formId',
            layout: {
                type: 'table',
                columns: 2
            },
            defaults: {
                xtype: 'textfield',
                margin: '5 5 5 5',
                labelAlign: 'right',
                labelPad: 15,
                width: 200,
                labelWidth: 80,
                maxLength: 200,
                maxLengthText: '最大长度为200'
            },
            items: [{
                xtype: 'displayfield',
                fieldLabel: '墓碑名称',
                submitValue: false,
                value: tombstoneInfo.Alias
            }, {
                xtype: 'displayfield',
                fieldLabel: '墓碑编号',
                submitValue: false,
                value: tombstoneInfo.AreaEntity.Alias + tombstoneInfo.RowEntity.Alias + tombstoneInfo.ColumnEntity.Alias
            }, {
                xtype: 'displayfield',
                fieldLabel: '录入时间',
                submitValue: false,
                colspan: 2,
                value: Ext.util.Format.date(new Date(), 'Y-m-d')
            }, {
                fieldLabel: '申请人',
                name: 'Applicanter',
                itemId: 'ApplicanterItemId',
                allowBlank: false,
                blankText: '申请人不能为空'
            }, {
                fieldLabel: '电话',
                name: 'Telephone',
                itemId: 'TelephoneItemId',
                allowBlank: false,
                blankText: '电话不能为空'
            }, {
                fieldLabel: '身份证号',
                name: 'IDNumber',
                itemId: 'IDNumberItemId',
                colspan: 2
            }, {
                fieldLabel: '补交时间',
                colspan: 2,
                xtype: 'datetimefield',
                width: 340,
                name: 'LastPaymentDate',
                itemId: 'LastPaymentDateItemId',
                mySqlType: 'date',
                value: new Date()
            }, {
                xtype: 'textareafield',
                colspan: 2,
                name: 'Remark',
                height: 80,
                width: 360,
                fieldLabel: '备注',
                itemId: 'RemarkItemId',
                maxLength: 800,
                maxLengthText: '最大长度为800'
            }, {
                fieldLabel: '付款金额',
                name: 'Money',
                itemId: 'MoneyItemId',
                regex: GlobalConfig.RegexController.regexMoney2Fixed,
                regexText: '请输入正确的金额(11111 或 11111.11)'
            }, {
                xtype: 'checkbox',
                name: 'IsFullMoney',
                itemId: 'IsFullMoneyItemId',
                boxLabel: '是否交齐全款',
                listeners: {
                    change: function (com, nVal, oVal) {
                        var win = com.up('window');
                        var LastPaymentDateItemId = win.down('#LastPaymentDateItemId');
                        if (nVal) {
                            LastPaymentDateItemId.setDisabled(true);
                        } else {
                            LastPaymentDateItemId.setDisabled(false);
                        }
                    }
                }
            }]
        }],
        buttons: [{
            text: '确定',
            listeners: {
                click: function (com) {
                    var win = com.up('window');
                    var form = win.down('#formId').getForm();
                    if (form.isValid()) {
                        //预定
                        var url = GlobalConfig.Controllers.JobManage.OrderTombstone;
                        form.submit({
                            url: url,
                            params: {
                                req: 'dataset',
                                dataname: 'OrderTombstone', // dataset名称，根据实际情况设置,数据库名
                                restype: 'json',
                                Id: tombstoneId,
                                sessiontoken: GlobalFun.getSeesionToken()
                            },
                            success: function (form, action) {
                                var showTomb = GridManager.AreaTombstoneImagePanel.down('#showTomb');
                                var tombCode = GridManager.AreaTombstoneImagePanel.down('#tombCode');
                                if (tombCode.isValid()) {
                                    showTomb.fireEvent("click", showTomb);
                                }
                                win.close();
                            },
                            failure: function (form, action) {
                                if (!GlobalFun.errorProcess(action.result.code)) {
                                    Ext.Msg.alert('失败', action.result.msg);
                                }
                            }

                        });
                    }
                }
            }
        }]
    });
    tempWin.show(null, function (win) {
        if (dfData) {
            //tempWin.down('#formId').getForm().loadRecord(dfData);
            //ApplicanterItemId,TelephoneItemId,IDNumberItemId,LastPaymentDateItemId,MoneyItemId
            var ApplicanterItemId = tempWin.down('#ApplicanterItemId');
            ApplicanterItemId.setValue(dfData.Applicanter);
            var TelephoneItemId = tempWin.down('#TelephoneItemId');
            TelephoneItemId.setValue(dfData.Telephone);
            var IDNumberItemId = tempWin.down('#IDNumberItemId');
            IDNumberItemId.setValue(dfData.IDNumber);
            var LastPaymentDateItemId = tempWin.down('#LastPaymentDateItemId');
            LastPaymentDateItemId.setValue(dfData.LastPaymentDateString.replace(' ', 'T'), true);
            var MoneyItemId = tempWin.down('#MoneyItemId');
            MoneyItemId.setValue(dfData.Money);
            var RemarkItemId = tempWin.down('#RemarkItemId');
            RemarkItemId.setValue(dfData.Remark);
            // LastPaymentDate
        }
    });
};


//业务操作-维护墓碑
GlobalFun.ShowSaleTombstoneWin = function (title, tombstoneId, tombstoneInfo) {
    //if (paymentStatusId == 2) {//已订
    //取预订的值
    var param = {
        Id: tombstoneId,
        controllName: '预订墓碑',
        sessiontoken: GlobalFun.getSeesionToken()
    };

    WsCall.call(GlobalConfig.Controllers.JobManage.GetOrderJobInfoTombstone, 'GetOrderJobInfoTombstone', param, function (response, opts) {
        var data = response.ResultOutDto;
        if (data) {
            GlobalFun.ShowOrderTombstoneWin(title, tombstoneId, tombstoneInfo, data);
        }
    }, function (response, opts) {
        if (!GlobalFun.errorProcess(response.code)) {
            Ext.Msg.alert('失败', response.msg);
        }
    }, true, "请稍候...");
    //}
};


//业务操作-落葬墓碑-首次
GlobalFun.ShowBuryTombstoneWin = function (title, tombstoneId, tombstoneInfo) {

    var tempShowWinFun = function (title, tombstoneId, tombstoneInfo, dfData) {
        var tempWin = Ext.create('Ext.window.Window', {
            title: title,
            defaultFocus: 'AliasItemId',
            iconCls: '',
            record: false,
            height: 470,
            width: 500,
            modal: true,
            resizable: false,
            layout: 'fit',
            items: [{
                xtype: 'form',
                itemId: 'formId',
                layout: {
                    type: 'table',
                    columns: 2
                },
                defaults: {
                    xtype: 'textfield',
                    margin: '5 5 5 5',
                    labelAlign: 'right',
                    labelPad: 15,
                    width: 200,
                    labelWidth: 80,
                    maxLength: 200,
                    maxLengthText: '最大长度为200'
                },
                items: [{
                    xtype: 'displayfield',
                    fieldLabel: '墓碑名称',
                    submitValue: false,
                    value: tombstoneInfo.Alias
                }, {
                    xtype: 'displayfield',
                    fieldLabel: '墓碑编号',
                    submitValue: false,
                    value: tombstoneInfo.AreaEntity.Alias + tombstoneInfo.RowEntity.Alias + tombstoneInfo.ColumnEntity.Alias
                }, {
                    xtype: 'displayfield',
                    fieldLabel: '录入时间',
                    submitValue: false,
                    colspan: 2,
                    value: Ext.util.Format.date(new Date(), 'Y-m-d')
                }, {
                    fieldLabel: '申请人',
                    name: 'Applicanter',
                    itemId: 'ApplicanterItemId',
                    allowBlank: false,
                    blankText: '申请人不能为空'
                }, {
                    fieldLabel: '电话',
                    name: 'Telephone',
                    itemId: 'TelephoneItemId',
                    allowBlank: false,
                    blankText: '电话不能为空'
                }, {
                    fieldLabel: '身份证号',
                    name: 'IDNumber',
                    itemId: 'IDNumberItemId',
                    colspan: 2
                }, {
                    xtype: 'textareafield',
                    colspan: 2,
                    name: 'Remark',
                    height: 80,
                    width: 360,
                    fieldLabel: '申请人备注',
                    itemId: 'RemarkItemId',
                    maxLength: 800,
                    maxLengthText: '最大长度为800'
                }, {
                    fieldLabel: '逝者',
                    name: 'BuryMan',
                    itemId: 'BuryManItemId',
                    allowBlank: false,
                    blankText: '逝者不能为空'
                }, {
                    fieldLabel: '落葬时间',
                    xtype: 'datetimefield',
                    width: 340,
                    name: 'BuryDate',
                    itemId: 'BuryDateItemId',
                    mySqlType: 'date',
                    value: new Date()
                }, {
                    xtype: 'textareafield',
                    colspan: 2,
                    name: 'Remark2',
                    height: 80,
                    width: 360,
                    fieldLabel: '落葬备注',
                    itemId: 'Remark2ItemId',
                    maxLength: 800,
                    maxLengthText: '最大长度为800'
                }, {
                    fieldLabel: '管理年限',
                    xtype: 'numberfield',
                    name: 'ManageLimit',
                    itemId: 'ManageLimitItemId',
                    allowBlank: false,
                    blankText: '管理年限不能为空',
                    value: 1,
                    maxValue: 30,
                    maxText: '管理年限最大为30年',
                    minValue: 1,
                    minText: '管理年限最小为1年'
                }, {
                    xtype: 'checkbox',
                    name: 'SupperManage',
                    disabled: GlobalFun.IsAllowFun('无限期管理年限') ? false : true,
                    itemId: 'SupperManageItemId',
                    boxLabel: '无限期管理年限'
                }]
            }],
            buttons: [{
                text: '确定',
                listeners: {
                    click: function (com) {
                        var win = com.up('window');
                        var form = win.down('#formId').getForm();
                        if (form.isValid()) {
                            //落葬
                            var url = GlobalConfig.Controllers.JobManage.BuryTombstone;
                            form.submit({
                                url: url,
                                params: {
                                    req: 'dataset',
                                    dataname: 'BuryTombstone', // dataset名称，根据实际情况设置,数据库名
                                    restype: 'json',
                                    Id: tombstoneId,
                                    FirstBury: true,
                                    sessiontoken: GlobalFun.getSeesionToken()
                                },
                                success: function (form, action) {
                                    var showTomb = GridManager.AreaTombstoneImagePanel.down('#showTomb');
                                    var tombCode = GridManager.AreaTombstoneImagePanel.down('#tombCode');
                                    if (tombCode.isValid()) {
                                        showTomb.fireEvent("click", showTomb);
                                    }
                                    win.close();
                                },
                                failure: function (form, action) {
                                    if (!GlobalFun.errorProcess(action.result.code)) {
                                        Ext.Msg.alert('失败', action.result.msg);
                                    }
                                }
                            });
                        }
                    }
                }
            }]
        });
        tempWin.show(null, function (win) {
            if (dfData) {
                //tempWin.down('#formId').getForm().loadRecord(dfData);
                //ApplicanterItemId,TelephoneItemId,IDNumberItemId,LastPaymentDateItemId,MoneyItemId
                var ApplicanterItemId = tempWin.down('#ApplicanterItemId');
                ApplicanterItemId.setValue(dfData.Applicanter);
                var BuryManItemId = tempWin.down('#BuryManItemId');
                BuryManItemId.setValue(dfData.BuryMan);
                var TelephoneItemId = tempWin.down('#TelephoneItemId');
                TelephoneItemId.setValue(dfData.Telephone);
                var IDNumberItemId = tempWin.down('#IDNumberItemId');
                IDNumberItemId.setValue(dfData.IDNumber);
                var RemarkItemId = tempWin.down('#RemarkItemId');
                RemarkItemId.setValue(dfData.Remark);
                var BuryDateItemId = tempWin.down('#BuryDateItemId');
                BuryDateItemId.setValue(dfData.BuryDateString.replace(' ', 'T'), true);

                // LastPaymentDate
            }
        });
    };

    var param = {
        Id: tombstoneId,
        controllName: '',
        sessiontoken: GlobalFun.getSeesionToken()
    };

    WsCall.call(GlobalConfig.Controllers.JobManage.GetTombstoneJobInfoLog, 'GetTombstoneJobInfoLog', param, function (response, opts) {
        var data = response.ResultOutDtos;
        if (data && data[0]) {
            tempShowWinFun(title, tombstoneId, tombstoneInfo, data[0]);
        }
    }, function (response, opts) {
        if (!GlobalFun.errorProcess(response.code)) {
            Ext.Msg.alert('失败', response.msg);
        }
    }, true, "请稍候...");


};

//业务操作-落葬墓碑-再次
GlobalFun.ShowBuryTombstoneAgainWin = function (title, tombstoneId, tombstoneInfo) {

    var loadRenewHistoryFun = function (myWin) {
        var ReNewManageHistory = myWin.down('#ReNewManageHistory');
        ReNewManageHistory.getStore().getProxy().extraParams.Id = tombstoneId;
        ReNewManageHistory.loadGrid();
        ReNewManageHistory.getStore().getProxy().extraParams.Id = 0;
        return;

        //读取续交历史信息
        var param = {
            Id: tombstoneId,
            sessiontoken: GlobalFun.getSeesionToken()
        };

        WsCall.call(GlobalConfig.Controllers.JobManage.GetTombstoneRenewManangeLog, 'GetTombstoneRenewManangeLog', param, function (response, opts) {
            var data = response.ResultOutDtos;
            //var html = "<table style='width:100%;border:1px solid black;'>";
            //html += '<th>时间</th>';
            //html += '<th>操作</th>';
            //html += '<th>备注</th>';
            //Ext.Array.each(data, function (item, index) {
            //    html += '<tr><td style="width:100px;">'+item.DateString+'</td>';
            //    html += '<td style="width:300px;">' + item.Content + '</td>';
            //    html += '<td style="width:250px;">' + item.Remark + '</td></tr>';
            //});
            //html += "</table>";
            //ReNewManageHistory.getEl().setHTML(html);
        }, function (response, opts) {
            //ReNewManageHistory.getEl().setHTML("无记录");
            //if (!GlobalFun.errorProcess(response.code)) {
            //    Ext.Msg.alert('失败', response.msg);
            //}
        }, true, "请稍候...", ReNewManageHistory.el);
    };

    var tempShowWinFun = function (title, tombstoneId, tombstoneInfo, dfData) {
        //GridManager.CreateBuryManGrid({ needLoad: false });


        var tempWin = Ext.create('Ext.window.Window', {
            title: title,
            defaultFocus: 'AliasItemId',
            iconCls: '',
            record: false,
            height: 560,
            width: 720,
            modal: true,
            resizable: false,
            autoScroll: true,
            //layout: 'fit',
            items: [{
                xtype: 'form',

                itemId: 'formId',
                layout: {
                    type: 'table',
                    columns: 3
                },
                defaults: {
                    xtype: 'displayfield',
                    margin: '5 5 5 5',
                    labelAlign: 'right',
                    labelPad: 15,
                    width: 200,
                    labelWidth: 80,
                    maxLength: 200,
                    maxLengthText: '最大长度为200'
                },
                items: [{
                    xtype: 'displayfield',
                    fieldLabel: '墓碑名称',
                    submitValue: false,
                    value: tombstoneInfo.Alias
                }, {
                    xtype: 'displayfield',
                    fieldLabel: '墓碑编号',
                    submitValue: false,
                    value: tombstoneInfo.AreaEntity.Alias + tombstoneInfo.RowEntity.Alias + tombstoneInfo.ColumnEntity.Alias
                }, {
                    xtype: 'displayfield',
                    fieldLabel: '录入时间',
                    submitValue: false,
                    //colspan: 2,
                    value: Ext.util.Format.date(new Date(), 'Y-m-d')
                }, {
                    fieldLabel: '申请人',
                    name: 'Applicanter',
                    itemId: 'ApplicanterItemId',
                    allowBlank: false,
                    blankText: '申请人不能为空'
                }, {
                    fieldLabel: '电话',
                    name: 'Telephone',
                    itemId: 'TelephoneItemId',
                    allowBlank: false,
                    blankText: '电话不能为空',
                    colspan: 2
                }, {
                    fieldLabel: '身份证号',
                    name: 'IDNumber',
                    itemId: 'IDNumberItemId',
                    colspan: 3
                }, {
                    xtype: 'textareafield',
                    colspan: 2,
                    name: 'Remark',
                    height: 80,
                    width: 420,
                    fieldLabel: '备注',
                    itemId: 'RemarkItemId',
                    maxLength: 800,
                    maxLengthText: '最大长度为800'
                }, {
                    width: 100,
                    xtype: 'button',
                    text: '修改申请人',
                    listeners: {
                        click: function () {

                            var tWin = Ext.create('Ext.window.Window', {
                                title: "修改申请人",
                                defaultFocus: 'BuryManItemId',
                                iconCls: '',
                                record: false,
                                height: 280,
                                width: 400,
                                layout: {
                                    type: 'table',
                                    columns: 1
                                },
                                defaults: {
                                    margin: '5 5 5 5'
                                },
                                modal: true,
                                resizable: false,
                                defaultType: 'textfield',
                                items: [{
                                    fieldLabel: '申请人',
                                    itemId: 'editApplicanter',
                                    value: dfData.Applicanter
                                }, {
                                    fieldLabel: '电话',
                                    itemId: 'editTelephone',
                                    value: dfData.Telephone
                                }, {
                                    fieldLabel: '身份证',
                                    itemId: 'editIDNumber',
                                    value: dfData.IDNumber
                                }, {
                                    xtype: 'textareafield',
                                    colspan: 2,
                                    name: 'Remark',
                                    height: 80,
                                    width: 360,
                                    fieldLabel: '备注',
                                    itemId: 'editRemark',
                                    maxLength: 800,
                                    maxLengthText: '最大长度为800',
                                    value: dfData.Remark
                                }, {
                                    xtype: 'hidden',//Id
                                    itemId: 'editControllTid',
                                    value: dfData.ControllTid
                                }],
                                buttons: [{
                                    text: '确定',
                                    handler: function () {
                                        //修改申请人，增加记录
                                        var param = {
                                            Id: tWin.down('#editControllTid').getValue(),
                                            Applicanter: tWin.down('#editApplicanter').getValue(),
                                            Telephone: tWin.down('#editTelephone').getValue(),
                                            IDNumber: tWin.down('#editIDNumber').getValue(),
                                            Remark: tWin.down('#editRemark').getValue(),
                                            sessiontoken: GlobalFun.getSeesionToken()
                                        };

                                        WsCall.call(GlobalConfig.Controllers.JobManage.EditApplicanter, 'EditApplicanter', param, function (response, opts) {
                                            //成功后修改原窗口信息
                                            var ApplicanterItemId = tempWin.down('#ApplicanterItemId');
                                            ApplicanterItemId.setValue(param.Applicanter);
                                            var TelephoneItemId = tempWin.down('#TelephoneItemId');
                                            TelephoneItemId.setValue(param.Telephone);
                                            var IDNumberItemId = tempWin.down('#IDNumberItemId');
                                            IDNumberItemId.setValue(param.IDNumber);
                                            var RemarkItemId = tempWin.down('#RemarkItemId');
                                            RemarkItemId.setValue(param.Remark);
                                            tWin.close();

                                        }, function (response, opts) {
                                            if (!GlobalFun.errorProcess(response.code)) {
                                                Ext.Msg.alert('失败', response.msg);
                                            }
                                        }, true, "请稍候...");
                                    }
                                }, {
                                    text: '关闭',
                                    handler: function () {
                                        tWin.close();
                                    }
                                }]
                            });
                            tWin.show();
                        }
                    }
                }, {
                    xtype: 'BuryManGrid',
                    colspan: 3,
                    border: true,
                    width: 680,
                    height: 180
                }, {
                    fieldLabel: '管理年限',
                    //xtype: 'numberfield',
                    name: 'ManageLimit',
                    itemId: 'ManageLimitItemId',
                    allowBlank: false,
                    blankText: '管理年限不能为空',
                    maxValue: 30,
                    maxText: '管理年限最大为30年',
                    minValue: 1,
                    minText: '管理年限最小为1年',
                    value: 1
                }, {
                    fieldLabel: '管理费时间',
                    width: 260,
                    //xtype: 'numberfield',
                    name: 'ManageDate',
                    itemId: 'ManageDateItemId'
                }, {
                    //xtype: 'checkbox',
                    //fieldLabel: '无限期管理年限',
                    labelWidth: 100,
                    fieldLabel: '交管理费催缴',
                    name: 'SupperManage',
                    //disabled: GlobalFun.IsAllowFun('无限期管理年限') ? false : true,
                    itemId: 'SupperManageItemId'//,
                    //colspan: 2
                    //boxLabel: '无限期管理年限'
                }, {
                    xtype: 'RenewManageGrid',
                    itemId: 'ReNewManageHistory',
                    title: '管理费续交历史',
                    colspan: 3,
                    height: 180,
                    width: 680,
                }
                    //, {
                    //xtype: 'fieldset',
                    //title: '管理费续交历史',
                    //itemId:'ReNewManageHistory',
                    //collapsible: true,
                    //padding:'2 2 2 5',
                    //colspan: 3,
                    //height: 80,
                    //width: 680,
                    //autoScroll: true//,
                    ////html: '<table><th>续交年限</th><th>续交时间</th><th>期间</th><tr><td>5</td><td>2011-01-01</td><td>2011-01至2015-04</td></tr><tr><td>5</td><td>2011-01-01</td><td>2011-01至2015-04</td></tr><tr><td>5</td><td>2011-01-01</td><td>2011-01至2015-04</td></tr><tr><td>5</td><td>2011-01-01</td><td>2011-01至2015-04</td></tr><tr><td>5</td><td>2011-01-01</td><td>2011-01至2015-04</td></tr><tr><td>5</td><td>2011-01-01</td><td>2011-01至2015-04</td></tr><tr><td>5</td><td>2011-01-01</td><td>2011-01至2015-04</td></tr></table>'
                    //}
                ]
            }],
            buttons: [{
                text: '续交管理费',
                hidden: !GlobalFun.IsAllowFun('续交管理费'),
                handler: function (com) {
                    var win = com.up('window');

                    var renewWin = Ext.create('Ext.window.Window', {
                        title: '续交管理费',
                        defaultFocus: 'ManageLimitItemId',
                        iconCls: '',
                        record: false,
                        height: 260,
                        width: 380,
                        modal: true,
                        resizable: false,
                        items: [{
                            fieldLabel: '管理年限',
                            width: 280,
                            xtype: 'numberfield',
                            name: 'rewinManageLimit',
                            itemId: 'rewinManageLimitItemId',
                            allowBlank: false,
                            blankText: '管理年限不能为空',
                            maxValue: 30,
                            maxText: '管理年限最大为30年',
                            minValue: 1,
                            minText: '管理年限最小为1年',
                            value: 1
                        }, {
                            xtype: 'textareafield',
                            name: 'Remark',
                            width: 320,
                            fieldLabel: '备注',
                            itemId: 'RemarkId',
                            maxLength: 800,
                            maxLengthText: '最大长度为800'
                        }],
                        buttons: [{
                            text: '确定',
                            listeners: {
                                click: function (com) {
                                    //renewWin
                                    var manageLimit = renewWin.down('#rewinManageLimitItemId');
                                    var remark = renewWin.down('#RemarkId');
                                    if (manageLimit.isValid() && remark.isValid()) {
                                        var param = {
                                            Id: tombstoneId,
                                            manageLimit: manageLimit.getValue(),
                                            remark: remark.getValue(),
                                            sessiontoken: GlobalFun.getSeesionToken()
                                        };

                                        WsCall.call(GlobalConfig.Controllers.JobManage.RenewManageLimit, 'RenewManageLimit', param, function (response, opts) {
                                            var data = response.ResultOutDtos;
                                            var arr = response.msg.split(';');
                                            win.down("#ManageDateItemId").setValue(arr[0]);
                                            win.down("#ManageLimitItemId").setValue(arr[1]);
                                            //读取续交历史信息
                                            loadRenewHistoryFun(win);
                                            renewWin.close();
                                        }, function (response, opts) {
                                            if (!GlobalFun.errorProcess(response.code)) {
                                                Ext.Msg.alert('失败', response.msg);
                                            }
                                        }, true, "请稍候...", renewWin.el);
                                    }

                                }
                            }
                        }, {
                            text: '关闭',
                            listeners: {
                                click: function (com) {
                                    var win = com.up('window');
                                    win.close();
                                }
                            }
                        }]
                    });
                    renewWin.show();

                }
            }, {
                text: '关闭',
                listeners: {
                    click: function (com) {
                        var win = com.up('window');
                        win.close();
                    }
                }
            }]
        });
        tempWin.show(null, function (win) {
            if (dfData) {
                var ApplicanterItemId = tempWin.down('#ApplicanterItemId');
                ApplicanterItemId.setValue(dfData.Applicanter);
                var ManageLimitItemId = tempWin.down('#ManageLimitItemId');
                ManageLimitItemId.setValue(dfData.ManageLimit);
                var ManageDateItemId = tempWin.down('#ManageDateItemId');
                ManageDateItemId.setValue(dfData.ManageDate);
                var SupperManageItemId = tempWin.down('#SupperManageItemId');
                SupperManageItemId.setValue(dfData.SupperManage == 1 ? "否" : "是");
                var TelephoneItemId = tempWin.down('#TelephoneItemId');
                TelephoneItemId.setValue(dfData.Telephone);
                var IDNumberItemId = tempWin.down('#IDNumberItemId');
                IDNumberItemId.setValue(dfData.IDNumber);
                var RemarkItemId = tempWin.down('#RemarkItemId');
                RemarkItemId.setValue(dfData.Remark);

            }
            var grid = tempWin.down('BuryManGrid');
            var store = grid.getStore();
            //加入filterMap
            if (!store.filterMap.containsKey('ControllTid')) {
                store.filterMap.add('ControllTid', tombstoneId);
            } else {
                store.filterMap.replace('ControllTid', tombstoneId);
            }
            if (!store.filterMap.containsKey('Type')) {
                store.filterMap.add('Type', 5);
            } else {
                store.filterMap.replace('Type', 5);
            }
            if (!store.filterMap.containsKey('ControlName')) {
                store.filterMap.add('ControlName', '落葬墓碑');
            } else {
                store.filterMap.replace('ControlName', '落葬墓碑');
            }

            grid.loadGrid();

            //读取续交历史信息
            loadRenewHistoryFun(tempWin);
        });
    };

    var param = {
        Id: tombstoneId,
        controllName: '',
        sessiontoken: GlobalFun.getSeesionToken()
    };

    WsCall.call(GlobalConfig.Controllers.JobManage.GetTombstoneJobInfoLog, 'GetTombstoneJobInfoLog', param, function (response, opts) {
        var data = response.ResultOutDtos;
        if (data && data[0]) {
            tempShowWinFun(title, tombstoneId, tombstoneInfo, data[0]);
        }
    }, function (response, opts) {
        if (!GlobalFun.errorProcess(response.code)) {
            Ext.Msg.alert('失败', response.msg);
        }
    }, true, "请稍候...");


};

