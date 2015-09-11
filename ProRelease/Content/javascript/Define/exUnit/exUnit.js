
//表格过滤菜单按钮
Ext.define('chl.exUnit.GridFilterMenuButton', {
    alias: 'widget.GridFilterMenuButton',
    extend: 'Ext.button.Button',
    /*
         filterParam: {
                group: 选择分组名,
                text: 首项目文本,
                filterKey: 表格过滤键值,
                GridTypeName: 表格类型名称,
                store: 菜单项源数据Store
            }
    */
    filterParam: false,
    menu: {
        defaults: {
            xtype: 'menucheckitem'
        },
        items: []
    },
    initComponent: function () {
        var me = this;
        var filter = '';
        me.callParent(arguments);	// 调用父类方法
        me.on('boxready', function (componet, width, height, eOpts) {
            if (me.filterParam) {
                var target = componet.up(me.filterParam.GridTypeName);
                GridManager.CreateGridFilterMenu(componet, me.filterParam.store, {
                    group: me.filterParam.group,
                    text: me.filterParam.text,
                    target: target,
                    componet: componet,
                    filterKey: me.filterParam.filterKey
                });
            }
        });
    }
});

//表格选择菜单按钮
Ext.define('chl.exUnit.GridSelectCancelMenuButton', {
    alias: 'widget.GridSelectCancelMenuButton',
    extend: 'Ext.button.Button',

    targetName: false,
    menu: {
        defaults: {
            xtype: 'menuitem'
        },
        items: []
    },
    initComponent: function () {
        var me = this;
        var filter = '';
        me.callParent(arguments);	// 调用父类方法
        me.on('boxready', function (componet, width, height, eOpts) {
            var target = componet.up(me.targetName);
            GridManager.CreateGridSelectCancelMenu(componet, target);
        });
    }
});


//图形化展示铺放容器
Ext.define('chl.exUnit.MiniPngDDSortPanel', {
    alias: 'widget.MiniPngDDSortPanel',
    extend: 'Ext.container.Container',
    alternateClassName: ['MiniPngDDSortPanel'],
    itemId: 'MiniPngDDSortPanel',
    frame: false,
    layout: {
        type: 'auto',
        width: 100
    },
    bodyStyle: {
        background: '#DFE8F6'
    },
    autoScroll: true,
    defaults: {
        xtype: 'image',
        width: 100,
        height: 100,
        margin: '5 5 5 5',
        style: {
            'float': 'left'
        }
    },
    //title: 'Mini图',
    items: []
});


//图形化展示显示类
Ext.define('MiniPngDDSortViewClass', {
    config: {
        selectAll: 0, //是否全选
        pngSels: new Ext.util.MixedCollection(), //所有选中的Png 数组 //缩略图数组
        pngAllMini: new Ext.util.MixedCollection(), //所有缩略图Png数组
        RowId: 0,
        AreaId: 0,
        currPngTotal: 1, //当前图形总数
        shiftImg: 0////shift选择记录位置
    },
    constructor: function (cfg) {
        this.initConfig(cfg);
        this.pngSels = new Ext.util.MixedCollection(); //所有选中的Png 数组 //缩略图数组
        this.pngSelBig = new Ext.util.MixedCollection(); //大图数组
        this.pngAllMini = new Ext.util.MixedCollection(); //所有缩略图Png数组
    },
    initPngPanel: function (palPngContainer) {

        var window = palPngContainer.up('window');
        var pngClass = window.pngClass;

        var pngNStyle = {
            border: '1px solid black',
            'background-color': '#FFFFFF',
            'background-image': 'none',
            'background-repeat': 'no-repeat'
        };
        var param = {
            limit: 0,
            page: 0,
            filter: '{ AreaId: ' + pngClass.AreaId + ',RowId:' + pngClass.RowId + ' }',
            dir: 'ASC',
            sort: 'SortNum',
            sessiontoken: GlobalFun.getSeesionToken()
        };
        WsCall.call(GlobalConfig.Controllers.TombstoneGrid.read, 'LoadTombstoneGrid', param, function (response, opts) {
            var data = response.dataset;
            window.el.mask('请稍候...');
            Ext.Array.each(data, function (item, index, allItems) {
                var src = GlobalFun.GetTomestoneImageSrc(item.PaymentStatusId, item.Id, item.TypeId);
                var pngContainer = Ext.create('Ext.container.Container', {
                    width: 100,//100,
                    height: 80,//140,
                    tombstoneId: item.Id,
                    layout: {
                        type: 'vbox',
                        align: 'center'
                    },
                    items: [{
                        xtype: 'image',
                        cls: 'hospital-target',
                        sortNum: index,//item.SortNum,
                        tombstoneId: item.Id,
                        width: 77,//90,
                        height: 60,//120,
                        style: pngNStyle,
                        src: src,
                        listeners: {
                            boxready: function (img, width, height, opts) {
                                //设置为可拖放状态
                                DropDragControl.initializeHospitalDropZone(img, window);
                                var imgDom = img.getEl();
                                //装载缩略图数组
                                pngClass.getPngAllMini().add(img.id, img);

                                imgDom.on('click', function (eve) {

                                    if (stopClick)
                                        return;
                                    //判断是否按下ctrl
                                    if (eve.ctrlKey) {
                                        pngClass.setShiftImg(img);
                                        var borderStyle = imgDom.getStyle('border', true);
                                        //选中
                                        if (borderStyle == '1px solid black' || borderStyle == 'black 1px solid') {
                                            imgDom.setStyle('border', '3px solid blue');
                                            if (!pngClass.getPngSels().contains(img)) {
                                                //设置为禁止拖放
                                                DropDragControl.uninitHospitalDropZone(img);
                                                //设置其为可拖拽
                                                DropDragControl.initializePatientDragZone(img, window);
                                                pngClass.getPngSels().add(img.id, img);
                                            }

                                        } else {//取消
                                            //patientImg= "";
                                            imgDom.setStyle('border', '1px solid black');
                                            if (pngClass.getPngSels().contains(img)) {
                                                //设置为禁止拖拽
                                                DropDragControl.uninitPatientDragZone(img);
                                                //设置为可拖放状态
                                                DropDragControl.initializeHospitalDropZone(img, window);
                                                pngClass.getPngSels().remove(img);
                                            }
                                        }
                                        return;
                                    }//判断是否按下ctrl end

                                    //判断是否按下shift
                                    if (eve.shiftKey) {
                                        var firstImg;
                                        var imageList = palPngContainer.query('image');
                                        //追加到后方
                                        Ext.Array.each(imageList, function (item, index, allItems) {
                                            if (index == 0) {
                                                if (pngClass.getShiftImg() != 0) {
                                                    firstImg = pngClass.getShiftImg();
                                                } else {
                                                    firstImg = item;
                                                }
                                            }
                                            //设置为禁止拖拽
                                            DropDragControl.uninitPatientDragZone(item);
                                            //设置为可拖放状态
                                            DropDragControl.initializeHospitalDropZone(item, window);
                                        });
                                        //清空选择数组
                                        pngClass.getPngSels().clear();
                                        Ext.Array.each(imageList, function (item, index, allItems) {
                                            var imgItem = item.getEl();
                                            imgItem.setStyle('border', '1px solid black'); //取消选中

                                            if (item.sortNum >= firstImg.sortNum && item.sortNum <= img.sortNum && !item.ownerCt.disabled) {
                                                imgItem.setStyle('border', '3px solid blue'); //选中
                                                //设置为禁止拖放
                                                DropDragControl.uninitHospitalDropZone(item);
                                                //设置其为可拖拽
                                                DropDragControl.initializePatientDragZone(item, window);
                                                pngClass.getPngSels().add(item.id, item);
                                            }
                                            if (item.sortNum <= firstImg.sortNum && item.sortNum >= img.sortNum && !item.ownerCt.disabled) {
                                                imgItem.setStyle('border', '3px solid blue'); //选中
                                                //设置为禁止拖放
                                                DropDragControl.uninitHospitalDropZone(item);
                                                //设置其为可拖拽
                                                DropDragControl.initializePatientDragZone(item, window);
                                                pngClass.getPngSels().add(item.id, item);
                                            }
                                        });
                                        return;
                                    }//判断是否按下shift end

                                    pngClass.setShiftImg(img);
                                    var imageList = palPngContainer.query('image');
                                    //追加到后方
                                    Ext.Array.each(imageList, function (item, index, allItems) {
                                        //设置为禁止拖拽
                                        DropDragControl.uninitPatientDragZone(item);
                                        //设置为可拖放状态
                                        DropDragControl.initializeHospitalDropZone(item, window);
                                    });
                                    //清空选择数组
                                    pngClass.getPngSels().clear();
                                    Ext.Array.each(imageList, function (item, index, allItems) {
                                        var imgItem = item.getEl();
                                        imgItem.setStyle('border', '1px solid black'); //取消选中
                                        if (item.id == img.id) {
                                            imgItem.setStyle('border', '3px solid blue'); //选中
                                            //设置为禁止拖放
                                            DropDragControl.uninitHospitalDropZone(item);
                                            //设置其为可拖拽
                                            DropDragControl.initializePatientDragZone(item, window);
                                            pngClass.getPngSels().add(item.id, item);
                                        }
                                    });

                                });
                                //双击
                                imgDom.on('dblclick', function () {

                                    var imageList = palPngContainer.query('image');
                                    //追加到后方
                                    Ext.Array.each(imageList, function (item, index, allItems) {
                                        //设置为禁止拖拽
                                        DropDragControl.uninitPatientDragZone(item);
                                        //设置为可拖放状态
                                        DropDragControl.initializeHospitalDropZone(item, window);
                                    });
                                    //清空选择数组
                                    pngClass.getPngSels().clear();
                                    Ext.Array.each(imageList, function (item, index, allItems) {
                                        var imgItem = item.getEl();
                                        imgItem.setStyle('border', '1px solid black'); //取消选中
                                    });
                                    //var borderStyle = imgDom.getStyle('border');
                                    var borderStyle = imgDom.dom.style['border'];
                                    if (borderStyle == '1px solid black' || borderStyle == 'black 1px solid') {
                                        imgDom.setStyle('border', '3px solid blue');
                                        if (!pngClass.getPngSels().contains(img)) {
                                            //设置为禁止拖放
                                            DropDragControl.uninitHospitalDropZone(img);
                                            //设置其为可拖拽
                                            DropDragControl.initializePatientDragZone(img, window);
                                            pngClass.getPngSels().add(img.id, img);
                                        }

                                    }

                                });

                                imgDom.dom.onload = function () {
                                    if (imgDom.dom && imgDom.dom.height > 1) {
                                        img.ownerCt.el.unmask();
                                    }
                                };
                            }
                        }
                    }, {
                        xtype: 'label',
                        text: item.Alias
                    }]
                });

                //加载png图片
                palPngContainer.add(pngContainer);
            });
            window.el.unmask();
        }, function (response, opts) {
            if (!GlobalFun.errorProcess(response.code)) {
                Ext.Msg.alert('失败', response.msg);
            }
        }, true, "请稍候...", palPngContainer.el);


    }
});


//图形化全区域滚动条展示铺放容器
Ext.define('chl.exUnit.AreaTombstoneImagePanel', {
    alias: 'widget.AreaTombstoneImagePanel',
    extend: 'Ext.panel.Panel',
    alternateClassName: ['AreaTombstoneImagePanel'],
    itemId: 'AreaTombstoneImagePanel',
    frame: false,
    bodyStyle: {
        background: '#DFE8F6'
    },
    border: false,
    autoScroll: true,
    style: {
        'overflow-x': 'auto',
        'white-space': 'nowrap'
    },
    dockedItems: [{
        xtype: 'container',
        itemId:'container',
        layout: {
            type: 'hbox',
            align: 'middle'
        },
        height:34,
        dock: 'top',
        defaults: {
            margin:'2px 2px 2px 2px'
        },
        items: [{
            xtype: 'textfield',
            itemId: 'tombCode',
            cls: 'big-fieldtext',
            fieldLabel: '区域或墓碑编码',
            labelWidth: 100,
            labelAlign: 'right',
            allowBlank: false,
            blankText: '不能为空',
            regex: GlobalConfig.RegexController.regexTombstoneCode,
            regexText: '请输入3位或5位或7位编码',
            enableKeyEvents: true,
            listeners: {
                keypress: function (field, e, opts) {
                    if (e.getKey() == e.ENTER) {
                        var toolbar = field.up('container');
                        var showTomb = toolbar.down('#showTomb');
                        showTomb.fireEvent("click", showTomb);
                    }
                    if (e.getKey() == e.ESC) {
                        var toolbar = field.up('container');
                        var reset = toolbar.down('#reset');
                        reset.fireEvent("click", reset);
                    }
                }
            }
        }, {
            xtype: 'button',
            text: '选择',
            handler: function (com) {
                var w = this.up('container');
                var field = w.down('#tombCode');
                WindowManager.SelectCemeteryAreaWin = WindowManager.ShowSelectCemeteryAreaWin();
                WindowManager.SelectCemeteryAreaWin.callComponent = field;
            }
        }, {
            text: '显示',
            xtype:'button',
            iconCls: 'search',
            width: 100,
            //height:24,
            cls: 'big-fieldtext',
            itemId: 'showTomb',
            listeners: {
                click: function (com) {
                    var toolbar = com.up('container');
                    var tombCode = toolbar.down('#tombCode');

                    if (tombCode.isValid()) {
                        GridManager.AreaTombstoneImagePanel.removeAll();
                        //获取墓碑
                        var param = {
                            limit: 0,
                            page: 0,
                            filter: '{}',
                            dir: 'ASC',
                            sort: 'SortNum',
                            sessiontoken: GlobalFun.getSeesionToken()
                        };
                        var codeVal = tombCode.getValue();
                        if (codeVal.length > 0) {
                            param.filter = '{ "Area": { "Alias": "' + codeVal + '" }, "Row": { "Alias": "' + codeVal + '" }, "Column": { "Alias": "' + codeVal + '" } }';
                        }
                        WsCall.call(GlobalConfig.Controllers.TombstoneGrid.read, 'LoadTombstoneGrid', param, function (response, opts) {
                            var data = response.dataset;
                            GridManager.AreaTombstoneImagePanel.el.mask('请稍候...');
                            //模拟加载
                            var pngNStyle = {
                                border: '1px solid black',
                                'background-color': '#FFFFFF',
                                'background-image': 'none',
                                'background-repeat': 'no-repeat',
                                'cursor': 'pointer'
                            };
                            var items = [];
                            var items1 = [];
                            var pngContainer = Ext.create('Ext.container.Container', {
                                style: {
                                    'white-space': 'nowrap',
                                    'margin': '5px 5px 5px 5px'
                                },
                                items: []
                            });
                            //行组信息
                            var rowMaps = new Ext.util.MixedCollection();
                            var areaSort = 'ASC';
                            Ext.Array.each(data, function (item, index, allItems) {
                                var key = item.RowId;
                                if (!rowMaps.containsKey(key)) {
                                    rowMaps.add(key, [item]);
                                } else {
                                    rowMaps.get(key).push(item);
                                }
                                if (item.AreaSort == "DESC") {
                                    areaSort = "DESC";
                                } 
                            });
                            rowMaps.sortByKey(areaSort);
                            rowMaps.each(function (row, index) {
                                items1 = [];
                                pngContainer = Ext.create('Ext.container.Container', {
                                    style: {
                                        'white-space': 'nowrap',
                                        'margin': '5px 5px 5px 5px',
                                    },
                                    items: []
                                });
                                Ext.Array.each(row, function (item, index1) {
                                    var src = GlobalFun.GetTomestoneImageSrc(item.PaymentStatusId, item.Id, item.TypeId);

                                    var pngContainer1 = Ext.create('Ext.container.Container', {
                                        width: 100,//100,
                                        height: 80,//140,
                                        tombstoneId: item.Id,
                                        layout: {
                                            type: 'vbox',
                                            align: 'center'
                                        },
                                        style: {
                                            'display': 'inline-block'
                                        },
                                        items: [{
                                            xtype: 'image',
                                            //cls: 'hospital-target',
                                            sortNum: item.SortNum,
                                            tombstoneId: item.Id,
                                            PaymentStatusId: item.PaymentStatusId,
                                            tombstoneInfo:item,
                                            width: 77,//90,
                                            height: 60,//120,
                                            style: pngNStyle,
                                            src: src,
                                            listeners: {
                                                boxready: function (img) {
                                                    var imgDom = img.getEl();
                                                    imgDom.on('click', function (eve) {
                                                        var tid = img.tombstoneId;
                                                        var tombstoneInfo = img.tombstoneInfo;
                                                        GlobalFun.JobTombPngClickFun(eve, tid, tombstoneInfo);
                                                    });
                                                }
                                            }
                                        }, {
                                            xtype: 'label',
                                            text: item.Alias
                                        }]
                                    });
                                    items1.push(pngContainer1);
                                });
                                //加载png图片
                                pngContainer.add(items1);
                                GridManager.AreaTombstoneImagePanel.add(pngContainer);
                            });
                            //加载png图片
                            //pngContainer.add(items1);
                            //GridManager.AreaTombstoneImagePanel.add(pngContainer);
                            GridManager.AreaTombstoneImagePanel.el.unmask();
                        }, function (response, opts) {
                            if (!GlobalFun.errorProcess(response.code)) {
                                Ext.Msg.alert('失败', response.msg);
                            }
                        }, true, "请稍候...", GridManager.AreaTombstoneImagePanel.el);

                    }
                }
            }

        }, {
            text: '重置',
            xtype: 'button',
            width: 100,
            cls: 'big-fieldtext',
            itemId:'reset',
            listeners: {
                click: function (field) {
                    var toolbar = field.up('container');
                    var tombCode = toolbar.down('#tombCode');
                    tombCode.setValue("");
                    tombCode.unsetActiveError();
                    tombCode.inputEl.focus(100);
                }
            }
           
        }]
    }],
    items: []
});