﻿Ext.define('chl.Model.BuryManGridModel', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    alternateClassName: ['BuryManGridModel'],
    fields: [{
        name: 'Id',
        type: 'string'
    }, {
        name: 'Date',
        mapping: 'DateString',
        type: 'string'
    }, {
        name: 'Applicanter',
        type: 'string'
    }, {
        name: 'IDNumber',
        type: 'string'
    }, {
        name: 'Telephone',
        type: 'string'
    }, {
        name: 'ControllTid',
        type: 'string'
    }, {
        name: 'BuryMan',
        type: 'string'
    }, {
        name: 'BuryDate',
        mapping: 'BuryDateString',
        type: 'string'
    }, {
        name: 'SupperManage',
        type: 'string'
    }, {
        name: 'ManageLimit',
        type: 'string'
    }, {
        name:'Remark'
    }, {
        name: 'Remark2'
    }]
});