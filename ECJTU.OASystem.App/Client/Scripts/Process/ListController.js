(function (app) {
    var ListController = function ($scope, processService) {
        //当前页面默认为1显示10行数据
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        //获取页面总数
        //判断是否已登录
        $scope.haveLogin = function () {
            return true;
        }
        
        function getPageCount() {
            $.get("../process/GetDataCount").then(function (result) {
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
            $.get("../process/GetProcessListByPage?" + "pageIndex=" + pageIndex + "&pageSize=" + $scope.pageSize).then(function (result) {
                var processs = JSON.parse(result);
                $scope.$root.processs = processs;
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
                    process: {
                        Name: '',
                        BusinessId: '',
                        State: '',
                        Remark: '',
                        Businesses: businesses
                    },
                    model: '新增'
                };
                $scope.$root.edit.process.BusinessId = businesses[0].Id.toString();
                $scope.$apply();
            })
        };
        //点击编辑按钮
        $scope.edit = function (Process) {
            $.get("../business/GetBusinessList").then(function (result) {
                $scope.$root.edit = {
                    process: Process,
                    model: '编辑'
                };
                $scope.$root.edit.process.Businesses = JSON.parse(result);
                $scope.$root.edit.process.BusinessId = $scope.$root.edit.process.BusinessId.toString();
                $scope.$apply();
            })
            
        }
        //删除确认
        $scope.confirmdelete = function (Process) {
            $scope.$root.del = {
                process: Process
            }
        }
        //删除
        $scope.delete = function (Process) {
            processService.destroy(Process).then(function () {
                removeItemById(Process.Id);
            })
        }
        //从列表中把删除项移除
        var removeItemById = function (id) {
            for (var i = 0; i < $scope.processs.length; i++) {
                if ($scope.processs[i].Id == id) {
                    $scope.processs.splice(i, 1);
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
}(angular.module("atTheProcess")));