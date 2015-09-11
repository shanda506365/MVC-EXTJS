// MainItemListTree类
Ext.define('chl.tree.BaseTree', {
    alias: 'widget.BaseTree',
    extend: 'Ext.tree.Panel',
    clearOnLoad: false,
    alternateClassName: ['BaseTree'],
    ddGroup: 'FileDDGp',
    animate: false,
    rootVisible: false,
    frame: false,
    frameHeader: false,
    border: false,
    initComponent: function () {

        var me = this;
        me.on({
            load: function (store, records, models, suc, opts) {
                if (!models || models.length <= 0)
                    return;
                Ext.Array.each(models, function (item, index, alls) {
                    var interStr = GlobalFun.treeInternational(item.data.id);
                    if (interStr != '') {
                        item.data.text = interStr;
                    } else {
                        item.data.text = GlobalFun.owerInternational(item.data.text);
                    }

                });
            },
            render: function () {

                var overItemHandle = function (view, record) {
                    me.activeRecord = record;
                };
                me.dropZone = Ext.create('Ext.dd.DropTarget', me.body.dom, {
                    ddGroup: me.ddGroup,//'FileDDGp',
                    notifyEnter: function (ddSource, e, data) {
                        me.activeRecord = null;
                        me.on('itemmouseenter', overItemHandle);
                        me.body.stopAnimation();
                        me.body.highlight();
                        this.callParent(arguments);
                    },
                    notifyOut: function (ddSource, e, data) {
                        me.activeRecord = null;
                        me.un('itemmouseenter', overItemHandle);
                        this.callParent(arguments);
                    },
                    notifyOver: function (ddSource, e, data) {
                        if (me.activeRecord != null) {
                            if (GlobalFun.isDropTarget(me.activeRecord,me)) {
                                return Ext.dd.DropTarget.prototype.dropAllowed;
                            } else {
                                return Ext.dd.DropTarget.prototype.dropNotAllowed;
                            }
                        } else {
                            return Ext.dd.DropTarget.prototype.dropNotAllowed;
                        }
                    },
                    notifyDrop: function (ddSource, e, data) {
                        if (me.activeRecord != null) {
                            if (GlobalFun.isDropTarget(me.activeRecord,me)) {
                                var treeRcd = me.activeRecord;
                                me.un('itemmouseenter', overItemHandle);
                                var sel = ddSource.dragData.records;
                                if (sel.length == 0 || !treeRcd)
                                    return false;
                                var faxids = [];
                                Ext.Array.each(data.records, function (rec, index,allrec) {
                                    if (rec.data.inFaxID) {
                                        faxids.push(rec.data.inFaxID);
                                    }
                                    if (rec.data.outFaxID) {
                                        faxids.push(rec.data.outFaxID);
                                    }

                                });
                                // 调用接口取值
                                var param = {
                                    faxIds: faxids.join(),
                                    sessiontoken: GlobalFun.getSeesionToken(),
                                    folderId: me.activeRecord.data.id
                                };

                                // 共享 或 个人 回收站
                                if (1 == 1) {
                                    // 刷新对应的Grid
                                    var tbStore = getCurrGrid().getStore();
                                    tbStore.remove(data.records);
                                    var extraP = tbStore.getProxy().extraParams;
                                    extraP.idList = param.faxIds;
                                    tbStore.getProxy().extraParams.toTrash = '0'; // '0'
                                    // 删除到回收站,'1'彻底删除
                                    tbStore.sync();
                                    (new Ext.util.DelayedTask(function () {
                                        tbStore.load();
                                    })).delay(500);
                                } else {// 共享文件箱
                                    var tarRec = me.activeRecord;
                                   
                                    WsCall.call('url','movefax', param, function (response, opts) {
                                        // 刷新对应的Grid
                                        var tbStore = getCurrGrid().getStore();
                                        tbStore.remove(data.records);

                                    }, function (response, opts) {
                                        if (!errorProcess(response.code)) {
                                            Ext.Msg.alert('失败', response.msg);
                                        }
                                    }, true);
                                }
                                me.activeRecord = null;

                                return true;

                            }
                        }
                        return false;
                    }
                });
            }
        });
        
        me.callParent(arguments);
    }
});