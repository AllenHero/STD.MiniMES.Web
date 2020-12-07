﻿/**
* 模块名：com viewModel
* 程序名: com.viewModel.search.js

**/
var com = com || {};
com.viewModel = com.viewModel || {};

com.viewModel.search = function (data) {
    var self = this;
    this.idField = data.idField || "BillNo";
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    delete this.form.__ko_mapping__;

    this.grid = {
        size: { w: 4, h: 94 },
        url: self.urls.query,
        queryParams: ko.observable(),
        pagination: true,
        customLoad: false
    };

    this.grid.queryParams(data.form);

    this.searchClick = function () {
        var param = ko.toJS(this.form);
        this.grid.queryParams(param);
    };

    this.clearClick = function () {
        $.each(self.form, function () { this(''); });
        this.searchClick();
    };

    this.refreshClick = function () {
        window.location.reload();
    };

    //新增
    this.addClick = function () {
        com.ajax({
            type: 'GET',
            url: self.urls.billno,
            success: function (d) {
                com.openTab(self.resx.detailTitle, self.urls.edit+"/");
            }
        });
    };
    //删除
    this.deleteClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        com.message('confirm', self.resx.deleteConfirm, function (b) {
            if (b) {
                com.ajax({
                    type: 'DELETE', url: self.urls.remove + row[self.idField], success: function () {
                        com.message('success', self.resx.deleteSuccess);
                        self.searchClick();
                    }
                });
            }
        });
    };
    //编辑
    this.editClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        com.openTab(self.resx.detailTitle, self.urls.edit+"?ID="+row[self.idField]);
    };

    this.downloadClick = function (vm, event) {
        com.exporter(self.grid).download($(event.currentTarget).attr("suffix"));
    };
};
 