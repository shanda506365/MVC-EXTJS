
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
            GridManager.CreateGridSelectCancelMenu(componet,target);
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
        AreaId:0,
        currPngTotal: 1, //当前图形总数
        shiftImg: 0////shift选择记录位置
    },
    constructor: function(cfg) {
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
            sort:'SortNum',
            sessiontoken: GlobalFun.getSeesionToken()
        };
        WsCall.call(GlobalConfig.Controllers.TombstoneGrid.read, 'LoadTombstoneGrid', param, function (response, opts) {
            var data = response.dataset;
            Ext.Array.each(data, function (item, index, allItems) {
                var pngContainer = Ext.create('Ext.container.Container', {
                    height: 142,
                    width: 100,
                    tombstoneId: item.Id,
                    layout: {
                        type: 'vbox',
                        align: 'center'
                    },
                    items: [{
                        xtype: 'image',
                        cls: 'hospital-target',
                        sortNum: index,//item.SortNum,
                        tombstoneId:item.Id,
                        width: 60,
                        height: 80,
                        style: pngNStyle,
                        src: '../../Content/images/Tombstone.png?currpage=' + item.Id + '&randomTime=' + (new Date()).getTime(),
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
                        text: item.Name
                    }]
                });

                //加载png图片
                palPngContainer.add(pngContainer);
            });
        }, function (response, opts) {
            if (!GlobalFun.errorProcess(response.code)) {
                Ext.Msg.alert('失败', response.msg);
            }
        }, true, "请稍候...", palPngContainer.el);
       
        
    }
});