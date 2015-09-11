
WindowManager.CreateImportRightGridStore = function () {

};

// 字段名称左边gridstore
Ext.create('Ext.data.ArrayStore', {
    fields: [{
        name: 'cellName',
        type: 'string'
    }, {
        name: 'cellIndex',
        type: 'string'
    }],
    storeId: 'cellLeftStoreId'
});
// 字段名称grid左边
Ext.define('WS.address.CellLeftGrid', {
    alternateClassName: ['CellLeftGrid'],
    alias: 'widget.CellLeftGrid',
    extend: 'Ext.grid.Panel',
    store: 'cellLeftStoreId',
    height: 355,
    width: 250,
    columnLines: true,
    multiSelect: false,
    viewConfig: {
        loadingText: '<b>' + '正在加载数据...' + '</b>'
    },
    columns: [{
        header: '字段名',
        dataIndex: 'cellName',
        flex: 1
    }]
});

// 字段名称右边gridstore
var rightData = [['', '全名', 'FullName'], ['', '身份证号', 'IDNumber']];

Ext.create('Ext.data.ArrayStore', {
    fields: [{
        name: 'srcCellName',
        type: 'string'
    }, {
        name: 'targetCellName',
        type: 'string'
    }, {
        name: 'dataIndex',
        type: 'string'
    }],
    storeId: 'ImportRightGridStoreId',
    data: rightData
});
// 导入窗右边grid
Ext.define('chl.UniversalWindow.ImportRightGrid', {
    alternateClassName: ['ImportRightGrid'],
    alias: 'widget.ImportRightGrid',
    extend: 'Ext.grid.Panel',
    store: 'ImportRightGridStoreId',
    height: 355,
    width: 380,
    columnLines: true,
    multiSelect: false,
    viewConfig: {
        loadingText: '<b>' + '正在加载数据...' + '</b>'
    },
    columns: [{
        header: '字段名',
        dataIndex: 'cellName',
        flex: 0.5
    }, {
        header: '对应字段名',
        dataIndex: 'allCellName',
        flex: 0.5
    }]
});


//导入窗口
Ext.define('chl.UniversalWindow.ImportDataWin', {
    extend: 'Ext.window.Window',
    title: '导入',
    height: 500,
    width: 720,
    defaultFocus: 'fileupId',
    iconCls: 'importAddrICON',
    //bodyCls : 'panelFormBg',
    border: false,
    resizable: false, // 窗口大小不能调整
    modal: true, // 设置window为模态
    listeners: {
        destroy: function (me, op) {
            Ext.data.StoreManager.lookup('cellLeftStoreId').loadData([]);
            Ext.data.StoreManager.lookup('cellRightStoreId').load();
        },
        show: function () {
            this.down('form').getForm().reset();
        }
    },
    items: [{
        xtype: 'form',
        //bodyCls : 'panelFormBg',
        border: false,
        padding: 10,
        items: [{
            layout: 'hbox',
            //bodyCls : 'panelFormBg',
            border: false,
            items: [{
                xtype: 'filefield',
                name: 'importAddr',
                fieldLabel: '请选择导入的文件',
                width: 400,
                labelWidth: 150,
                blankText: '请选择导入的文件',
                msgTarget: 'side',
                itemId: 'fileupId',
                buttonText: '...',
                listeners: {
                    change: function (me, val, op) {
                        var supType = new Array('xls', 'xlsx');
                        var fNmae = me.getValue();
                        var fType = fNmae.substring(
								fNmae.lastIndexOf('.') + 1,
								fNmae.length).toLowerCase();
                        var returnFlag = true;

                        Ext.Array.each(supType, function (rec) {
                            if (rec == fType) {
                                returnFlag = false;
                                return false;
                            }
                        });

                        if (returnFlag) {
                            Ext.Msg.alert('添加文件', '不支持的文件格式！');
                            return;
                        }
                        var f = me.up('form');
                        var outWin = me.up('window');
                        var form = f.getForm();
                        var urlStr = GlobalConfig.Controllers.CustomerGrid.uploadCustomerExcel
								+ "?req=call&callname=importcustomerUp&sessiontoken=" + GlobalFun.getSeesionToken();
                        form.submit({
                            url: urlStr,
                            waitMsg: '正在上传...',
                            waitTitle: '等待文件上传,请稍候...',
                            success: function (fp, o) {

                                var leftStore = f.down('#leftGridId').getStore();

                                Ext.create('Ext.data.Store', {
                                    fields: ['Id', 'Name'],
                                    storeId: 'excelUpStoreID',
                                    data: o.result.dataset
                                });

                                Ext.create('Ext.window.Window', {
                                    outWin: outWin,
                                    height: 105,
                                    width: 220,
                                    title: '选择工作表',
                                    iconCls: 'actionSelect',
                                    border: false,
                                    bodyPadding: 10,
                                    resizable: false,		//窗口大小不能调整
                                    modal: true,   //设置window为模态
                                    items: [{
                                        xtype: 'combobox',
                                        width: 180,
                                        itemId: 'sheetIdxID',
                                        store: 'excelUpStoreID',
                                        queryMode: 'local',
                                        displayField: 'Name',
                                        valueField: 'Id',
                                        value: '0',
                                        editable: false
                                    }],
                                    buttons: [{
                                        text: '确定',
                                        itemId: 'submit',
                                        formBind: true,
                                        handler: function () {

                                            var win = this.up('window');
                                            var idx = win.down('#sheetIdxID').getValue();
                                            if (idx >= o.result.data) {
                                                return;
                                            }
                                            var paramC = {
                                                sessiontoken: getSessionToken(),
                                                sheetIdx: idx
                                            };

                                            WsCall.call('readExcel', paramC, function (response, opts) {
                                                win.outWin.total = response.msg;
                                                if (response.msg > 0) {
                                                    win.outWin.sheetIdx = idx;
                                                    leftStore.loadData(Ext.JSON.decode(response.data));
                                                } else {
                                                    Ext.Msg.alert('错误', '导入的文件为空');
                                                }
                                            }, function (res) {
                                                if (!errorProcess(res.code)) {
                                                    Ext.Msg.alert('失败', res.msg);
                                                }
                                            }, true);
                                            win.close();
                                        }
                                    }, {
                                        text: '取消',
                                        handler: function () {
                                            var me = this;
                                            me.up('window').close();
                                        }
                                    }]

                                }).show();

                            },
                            failure: function (fp, o) {
                                if (!errorProcess(o.result.code)) {
                                    var w = Ext.create('Ext.window.Window', {
                                        title: '失败',
                                        modal: true,
                                        height: 100,
                                        width: 300,
                                        resizable: false,
                                        layout: 'vbox',
                                        border: false,
                                        bodyStyle: {
                                            background: '#AABBCC'
                                        },
                                        bodyBorder: false,
                                        items: [{
                                            xtype: 'label',
                                            margin: '15 0 0 4',
                                            text: o.result.msg
                                        }, {
                                            xtype: 'button',
                                            margin: '10 0 0 100',
                                            width: 100,
                                            text: '确定',
                                            handler: function () {
                                                w.close();
                                            }
                                        }]
                                    });
                                    w.show();
                                }
                            }
                        });
                    }
                }
            }, {
                width: 250,
                margin: '1 0 5 20',
                xtype: 'label',
                text: '(支持的文件格式:xls, xlsx)'
            }]
        }, {
            padding: '1 0 10 0',
            xtype: 'displayfield',
            labelWidth: 200,
            width: 500,
            fieldLabel: '请设置字段的对应关系'
        }, {
            layout: 'hbox',
            //bodyCls : 'panelFormBg',
            border: false,
            items: [{
                xtype: 'CellLeftGrid',
                itemId: 'leftGridId'
            }, {
                xtype: 'panel',
                width: 60,
                height: 350,
                layout: {
                    type: 'vbox',
                    align: 'center'
                },
                border: false,
                //bodyCls : 'panelFormBg',

                defaults: {
                    xtype: 'button',
                    width: 50
                },
                items: [{
                    margin: '10 0 0 0',
                    text: '>',
                    tooltip: '建立对应关系',
                    handler: function () {
                        var me = this;
                        var w = me.up('window');
                        var left = w.down('#leftGridId');
                        var right = w.down('#rightGridId');
                        var lsm = left.getSelectionModel();
                        var rsm = right.getSelectionModel();
                        if (lsm.hasSelection()
                                && rsm.hasSelection()) {
                            if (rsm.getSelection()[0].data.cellName != '') {
                                left.getStore().add({
                                    'cellName': rsm
                                            .getSelection()[0].data.cellName,
                                    'cellIndex': rsm
                                            .getSelection()[0].data.hideIndex
                                });
                            }
                            rsm.getSelection()[0]
                                    .set(
                                            'cellName',
                                            lsm.getSelection()[0].data.cellName);
                            rsm.getSelection()[0]
                                    .set(
                                            'hideIndex',
                                            lsm.getSelection()[0].data.cellIndex);
                            // right.getStore().load();
                            left.getStore().remove(lsm
                                    .getSelection());
                        }
                    }
                }, {
                    margin: '20 0 0 0',
                    text: '<',
                    tooltip: '删除对应关系',
                    handler: function () {
                        var me = this;
                        var w = me.up('window');
                        var left = w.down('#leftGridId');
                        var right = w.down('#rightGridId');
                        var rsm = right.getSelectionModel();
                        if (rsm.hasSelection()) {
                            left.getStore().add({
                                'cellName': rsm.getSelection()[0].data.cellName,
                                'cellIndex': rsm
                                        .getSelection()[0].data.hideIndex
                            });
                            rsm.getSelection()[0].set(
                                    'cellName', '');
                        }
                    }
                }]

            }, {
                xtype: 'ImportRightGrid',
                itemId: 'importRightGrid'
            }]
        }]
    }],
    buttons: [{
        text: '确定',
        itemId: 'submit',
        formBind: true,
        handler: function () {
            var store = Ext.data.StoreManager.lookup('excelUpStoreID');
            var me = this;
            var w = me.up('window');
            if (w.total == 0) {
                return;
            }
            var storeArr = w.down('#importRightGrid').getStore();
            var relation = '';
            var records = storeArr.data.items;
            var dispName = '';
            for (var p in records) {
                var cell = records[p].data.cellName;
                var index = records[p].data.hideIndex;
                var rex = records[p].data.hideName;
                if (cell.length > 0) {
                    if (relation.length <= 0) {
                        relation = rex + ',' + index;
                    } else {
                        relation += ';' + rex + ',' + index;
                    }
                }
                if (rex == 'dispName') {
                    dispName = cell;
                }
            }

            if (relation.length <= 0) {
                Ext.Msg.alert('错误', '请选择至少一个对应关系');
                return;
            }
            if (dispName.length <= 0) {
                Ext.Msg.alert('错误', '显示名必须选择对应关系');
                return;
            }
            var param = {
                sessiontoken: getSessionToken(),
                relation: relation,
                sheetIdx: w.sheetIdx,
                hidSelTree: w.selTreeNode
            }


            WsCall.call('startImpAddr', param, function (response, opts) {
                var total = w.total;

                var taskRunner = new Ext.util.TaskRunner();
                var progress = Ext.MessageBox.show({
                    title: '请稍候...',
                    msg: '需要导入的记录共' + ' ' + total + ' ' + '条',
                    progressText: '数据正在导入',
                    width: 300,
                    progress: true,
                    closable: false,
                    buttons: Ext.MessageBox.CANCEL,
                    fn: function (btn) {
                        if (btn == 'cancel') {
                            taskRunner.stopAll();
                            progress.hide();
                            var paramC = {
                                sessiontoken: getSessionToken()
                            };
                            WsCall.call('cancelImpAddr', paramC, function (response, opts) {
                                Ext.create('Ext.window.Window', {
                                    title: '导入结果',
                                    modal: true,
                                    height: 105,
                                    width: 300,
                                    resizable: false,
                                    border: false,
                                    bodyStyle: {
                                        background: '#AABBCC'
                                    },
                                    bodyPadding: 8,
                                    autoScroll: true,
                                    bodyBorder: false,
                                    html: '导入联系人个数' + ': ' + response.data
                                }).show();
                            }, function (res) {
                                if (!errorProcess(res.code)) {
                                    Ext.Msg.alert('失败', res.msg);
                                }
                            }, false);
                        }
                    }
                });

                var f = function () {
                    var param = {
                        sessiontoken: getSessionToken()
                    };
                    WsCall.call('impAddrEnd', param, function (response, opts) {
                        taskRunner.stopAll();

                        progress.updateProgress(1, '已完成 ' + total + '/' + total);
                        (new Ext.util.DelayedTask()).delay(200, function () {
                            progress.hide();
                            w.grid.loadGrid();
                            Ext.create('Ext.window.Window', {
                                title: '导入结果',
                                modal: true,
                                height: 105,
                                width: 300,
                                resizable: false,
                                border: false,
                                bodyStyle: {
                                    background: '#AABBCC'
                                },
                                bodyPadding: 8,
                                autoScroll: true,
                                bodyBorder: false,
                                html: '导入联系人个数' + ': ' + response.data
                            }).show();
                            //									Ext.Msg.alert('成功', '导入通讯录成功');

                        });
                    }, function (res) {
                        if (!errorProcess(res.code)) {
                            var i = res.data / total;
                            progress.updateProgress(i, '已完成 ' + res.data + '/' + total);
                        }
                    }, false);
                };
                (new Ext.util.DelayedTask(function () {
                    taskRunner.start({
                        run: function () {
                            f();
                        },
                        interval: 2000
                    });
                })).delay(200);

            }, function (res) {
                if (!errorProcess(res.code)) {
                    Ext.Msg.alert('失败', res.msg);
                }
            }, false);

            w.close();

        }
    }, {
        text: '取消',
        handler: function () {
            var me = this;
            me.up('window').close();
        }
    }]
});