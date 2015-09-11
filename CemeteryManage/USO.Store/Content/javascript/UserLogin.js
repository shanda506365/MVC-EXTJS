Ext.onReady(function () {

    //登录窗口调用
    var LoginWin;
    function showTheLoginWin() {
        // 初始化登录窗口
        LoginWin = Ext.create('Ext.window.Window', {
            // title: '登录窗口',
            closable: false,
            modal: true,
            width: 416,
            border: false,
            resizable: false,
            shadow: false,
            plain: true,
            height: 280,
            preventHeader: true,
            layout: 'auto',
            baseCls: '',
            style: {
                'background-color': 'transparent',
                'padding': '0 0 0 0'
            },
            bodyStyle: {
                'background-image': 'url(../Content/images/loginbk.png)',
                'background-repeat': 'no-repeat',
                'background-color': 'transparent',
                'padding': '0 0 0 0',
                'margin': '-8 0 0 -8'
            },
            defaults: {
                border: false
            },
            listeners: {
                boxready: function (win, width, height, opts) {

                    win.down('#Username').setValue(login_username);
                    // 判断是否有cookie
                    if (Ext.util.Cookies.get("login_password") != null) {
                        win.down('#Password').setValue('·············');
                        win.down('#hpassword').setValue(decryptedString(RASkey, login_password));
                    }
                    if (login_checksavepass) {
                        win.down('#savePass').setValue(true);
                    }

                }
            },
            items: [{
                xtype: 'form',
                layout: {
                    type: 'table',
                    columns: 2,
                    tdAttrs: {
                        style: 'vertical-align:bottom'
                    }
                },
                width: 400,
                height: 220,
                style: {
                    'background-color': 'transparent',
                    'border': '0px'
                },
                bodyStyle: {
                    'background-color': 'transparent',
                    'border': '0px'
                },
                defaults: {
                    xtype: 'textfield',
                    labelAlign: 'right',
                    width: 300,
                    margin: '2 0 0 30',
                    colspan: 2
                },
                items: [{
                    fieldLabel: '用户名',
                    itemId: 'Username',
                    stateful: false,
                    value: '',
                    allowBlank: false,
                    blankText: '用户名不能为空',
                    margin: '80 0 0 30',
                    enableKeyEvents: true,
                    submitValue: false,
                    listeners: {
                        keypress: function (field, e, opts) {
                            if (e.getKey() == e.ENTER) {
                                field.up('window').down('#Login').fireEvent("click");
                            }
                        }
                    }
                }, {
                    fieldLabel: '登录密码',
                    itemId: 'Password',
                    stateful: false,
                    //colspan : 1,
                    value: '',
                    //width : 230,				
                    inputType: 'password',
                    allowBlank: true,
                    enableKeyEvents: true,
                    listeners: {
                        keypress: function (field, e, opts) {
                            if (e.getKey() == e.ENTER) {
                                field.up('window').down('#Login').fireEvent("click");
                            }
                        },
                        change: function (field, nVal, oVal, opts) {
                            field.up('window').down('#hpassword').setValue(nVal);
                        }
                    }
                    // blankText: "登录密码不能为空"
                }, {
                    xtype: 'hidden',
                    stateful: false,
                    itemId: 'hpassword',
                    value: ''
                }, {
                    xtype: 'container',
                    width: 400,
                    layout: 'hbox',
                    items: [{
                        xtype: 'checkboxfield',
                        margin: '2 0 0 105',
                        itemId: 'savePass',
                        boxLabel: '保存密码'
                    }, {
                        margin: '2 0 0 15',
                        xtype: 'label',
                        html: '<a href="javascript:void(0)"  onclick=' + "alert('请联系管理员');" + '>' + '忘记密码?' + '</a>'
                    }]
                }, {
                    xtype: 'container',
                    layout: {
                        type: 'table',
                        columns: 2
                    },
                    style: {
                        'background-color': 'transparent',
                        'border': '0px'
                    },
                    margin: '10 0 0 30',
                    bodyStyle: {
                        'background-color': 'transparent',
                        'border': '0px'
                    },
                    defaults: {
                        width: 80
                    },
                    items: [{
                        xtype: 'button',
                        text: '登录',
                        itemId: 'Login',
                        margin: '0 10 0 130',
                        listeners: {
                            click: function () {
                                var form = this.up('form').getForm();
                                var win = this.up('window');
                                var savepass = win.down('#savePass');

                                var passtext;
                                // 判断是否有cookie
                                passtext = win.down('#hpassword').getValue();

                                if (form.isValid()) {

                                    LoginWin.el.mask('正在验证登录信息，请稍候...');

                                    var param = {};
                                    param.myusername = win.down('#Username').value;
                                    param.password = encryptedString(RASkey, passtext);
                                    userLogin(param, savepass);
                                }
                            }
                        }
                    }, {
                        xtype: 'button',
                        text: '取消',
                        itemId: 'Cancel',
                        handler: function () {
                            Ext.util.Cookies.clear("login_sessiontoken");
                            window.close();
                            window.location.reload();

                        }
                    }]

                }]
            }]
        }).show();
    };

    //判断是否已登录
    var queryString = window.location.search;
    if (queryString.indexOf("RedirectPath=") == -1) {
        if (login_sessiontoken && login_sessiontoken != '') {
            //跳转页面
            Ext.getBody().mask("登录验证,请稍候...");
            GlobalFun.ReDirectUrl("Index");
        } else {
            showTheLoginWin();
        }
    } else {
        showTheLoginWin();
    }




    // 用户登录调用 --savepass : 保存密码控件
    function userLogin(param, savepass) {
        // 调用
        WsCall.call(GlobalConfig.Controllers.User.CheckUserPassword, 'CheckUserPassword', param, function (response, opts) {
            LoginWin.el.unmask();
            //Ext.getBody().mask('登录成功，正在加载用户数据，请稍候...');
            (new Ext.util.DelayedTask()).delay(50, function () {
                var data = response;
                Ext.util.Cookies.set('login_sessiontoken', data.msg);
                Ext.util.Cookies.set("login_username", data.ResultOutDto.LoginName, new Date(new Date().getTime() + (1000 * 60 * 60 * 24 * 30)));
                login_username = Ext.util.Cookies.get("login_username");
                if (data.msg != '') {
                    if (savepass) {
                        if (savepass.getValue()) {
                            Ext.util.Cookies.set('login_password', data.ResultOutDto.Password, new Date(new Date().getTime() + (1000 * 60 * 60 * 24 * 30)));
                            Ext.util.Cookies.set('login_checksavepass', 1, new Date(new Date().getTime() + (1000 * 60 * 60 * 24 * 30)));
                        } else {
                            Ext.util.Cookies.clear('login_password');
                            Ext.util.Cookies.clear('login_checksavepass');
                        }
                    }
                    //跳转页面
                    LoginWin.el.mask("请稍候...");
                    GlobalFun.ReDirectUrl("Index");
                } else {
                    LoginWin.el.unmask();
                    if (!GlobalFun.errorProcess(response.code)) {
                        Ext.Msg.alert('登录失败', response.msg);
                    }
                }

            });
        }, function (response, opts) {
            LoginWin.el.unmask();
            if (!GlobalFun.errorProcess(response.code)) {
                Ext.Msg.alert('登录失败', response.msg);
            }
        }, false);

    }
});