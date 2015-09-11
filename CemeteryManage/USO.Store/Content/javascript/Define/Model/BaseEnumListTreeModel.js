﻿//BaseEnumListTree
// BaseEnumListTree Model
Ext.define('chl.Model.BaseEnumListTreeModel', {
    extend: 'Ext.data.Model',
    fields: [{
        name: 'id',
        type: 'string',
        mapping: 'Id'
    }, {
        name: 'text',
        type: 'string',
        mapping: 'Text'
    }, {
        name: 'leaf',
        type: 'boolean',
        mapping: 'IsLeaf'
    }, {
        name: 'iconCls',
        type: 'string',
        mapping: 'Iconcls'
    }, {
        name: 'expanded',
        type: 'boolean',
        mapping: 'Expanded'
    }, {
        name: 'linksrc',
        type: 'string',
        mapping: 'LinkSrc'
    }, {
        name: 'parent',
        mapping: 'Parent'
    }, {
        name: 'children',
        mapping: 'Children'
    }, {
        name: 'cls',
        mapping: 'Cls'
    }]
});