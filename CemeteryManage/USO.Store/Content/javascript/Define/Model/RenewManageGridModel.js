Ext.define('chl.Model.RenewManageGridModel', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    alternateClassName: ['RenewManageGridModel'],
    fields: [{
        name: 'Id',
        type: 'string'
    }, {
        name: 'Date',
        mapping: 'DateString',
        type: 'string'
    }, {
        name: 'Content',
        type: 'string'
    }, {
        name: 'Remark'
    }]
});