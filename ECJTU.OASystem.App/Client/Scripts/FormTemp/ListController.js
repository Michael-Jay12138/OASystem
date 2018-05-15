(function (app) {
    var ListController = function ($scope, formTempService) {
        //当前页面默认为1显示10行数据
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        //获取页面总数
        //判断是否已登录
        $scope.haveLogin = function () {
            return true;
        }
        
        function getPageCount() {
            $.get("../formTemp/GetDataCount").then(function (result) {
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
            $.get("../formTemp/GetFormTempListByPage?" + "pageIndex=" + pageIndex + "&pageSize=" + $scope.pageSize).then(function (result) {
                var formTemps = JSON.parse(result);
                $scope.$root.formTemps = formTemps;
                $scope.$apply();
                $("#table").trigger("update");
            })
        }
        getByPage(1);
        getPageCount();
        //点击新增按钮
        $scope.create = function () {
            $.get("../business/GetBusinessList").then(function (result) {
                var businesses = JSON.parse(result);
                $scope.$root.edit = {
                    formTemp: {
                        Name: ''
                    },
                    Businesses: businesses,
                    model: '新增'
                };
                $scope.$root.edit.BusinessId = businesses[0].Id.toString();
                $scope.$apply();
            })
            
        }
        //点击编辑按钮
        $scope.edit = function (FormTemp) {
            $.get("../business/GetBusinessList").then(function (result_b) {
                var businesses = JSON.parse(result_b);
                $.get("../business/GetBusinessIdByFormTempId?formTempId=" + FormTemp.Id).then(function (result_id) {
                    $scope.$root.edit = {
                        formTemp: FormTemp,
                        Businesses: businesses,
                        BusinessId: result_id,
                        model: '编辑'
                    };
                    $scope.$root.edit.BusinessId = $scope.$root.edit.BusinessId.toString();
                    $scope.$apply();
                })
            })
        }
        //删除确认
        $scope.confirmdelete = function (FormTemp) {
            $scope.$root.del = {
                formTemp: FormTemp
            }
        }
        //删除
        $scope.delete = function (FormTemp) {
            formTempService.destroy(FormTemp).then(function () {
                removeItemById(FormTemp.Id);
            })
        }
        //从列表中把删除项移除
        var removeItemById = function (id) {
            for (var i = 0; i < $scope.formTemps.length; i++) {
                if ($scope.formTemps[i].Id == id) {
                    $scope.formTemps.splice(i, 1);
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
}(angular.module("atTheFormTemp")));