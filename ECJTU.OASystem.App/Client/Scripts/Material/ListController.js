﻿(function (app) {
    var ListController = function ($scope, materialService) {
        //当前页面默认为1显示10行数据
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        //获取页面总数
        //判断是否已登录
        $scope.haveLogin = function () {
            return true;
        }
        
        function getPageCount() {
            $.get("../material/GetDataCount").then(function (result) {
                if (result % $scope.pageSize == 0)
                    $scope.cn = result / $scope.pageSize;
                else
                    $scope.cn = parseInt(result / $scope.pageSize) + 1;
                var pages = "[{";
                for (var i = 1; i <= $scope.cn; i++) {
                    if (i == $scope.cn)
                        pages = pages + '"page":' + i;
                    else
                        pages = pages + '"page":' + i + "},{";
                }
                pages += "}]";
                var obj = eval(pages);
                $scope.pages = obj;
            })
        }
        //根据当前页面获取数据
        var getByPage = function (pageIndex) {
            if (pageIndex < 1 || pageIndex > $scope.cn)
                return;
            $.get("../material/GetMaterialListByPage?" + "pageIndex=" + pageIndex + "&pageSize=" + $scope.pageSize).then(function (result) {
                var materials = JSON.parse(result);
                $scope.$root.materials = materials;
                $scope.$apply();
                $("#table").trigger("update");
            })
        }
        getByPage(1);
        getPageCount();
        $scope.download = function (material) {
            $.post("../material/GetMaterial", material).then(function (result) {
                //$.post("../Service/FileDownloadService.ashx", { tempFilePath: result }).then(function (data) {
                    
                //})
                window.open("../Temp/" + result);
            })
        }
        //点击新增按钮
        $scope.create = function () {
            $scope.$root.edit = {
                material: {
                    Name: '',
                    Path: '',
                    MaterialTempInstId:''
                },
                model: '新增'
            };
        }
        //点击编辑按钮
        $scope.edit = function (Material) {
            $scope.$root.edit = {
                material: Material,
                model: '编辑'
            };
        }
        //删除确认
        $scope.confirmdelete = function (Material) {
            $scope.$root.del = {
                material: Material
            }
        }
        //删除
        $scope.delete = function (Material) {
            materialService.destroy(Material).then(function () {
                removeItemById(Material.Id);
            })
        }
        //从列表中把删除项移除
        var removeItemById = function (id) {
            for (var i = 0; i < $scope.materials.length; i++) {
                if ($scope.materials[i].Id == id) {
                    $scope.materials.splice(i, 1);
                    break;
                }
            }
        };
        //选择第几页
        $scope.selectPage = function (pageIndex) {
            $scope.pageIndex = pageIndex;
            getByPage(pageIndex);
        }
        //前一页
        $scope.Previous = function () {
            getByPage(--$scope.pageIndex);
        }
        //后一页
        $scope.Next = function () {
            getByPage(++$scope.pageIndex);
        }
        //判断是否当前页，用于突出显示
        $scope.isActivePage = function (pageIndex) {
            return $scope.pageIndex == pageIndex;
        }
    };
    app.controller("ListController", ListController);
}(angular.module("atTheMaterial")));