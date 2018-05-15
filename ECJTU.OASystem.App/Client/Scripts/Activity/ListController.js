(function (app) {
    var ListController = function ($scope, activityService) {
        //当前页面默认为1显示10行数据
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        //获取页面总数
        //判断是否已登录
        $scope.haveLogin = function () {
            return true;
        }
        
        function getPageCount() {
            $.get("../activity/GetDataCount").then(function (result) {
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
            $.get("../activity/GetActivityListByPage?" + "pageIndex=" + pageIndex + "&pageSize=" + $scope.pageSize).then(function (result) {
                var activitys = JSON.parse(result);
                $scope.$root.activitys = activitys;
                $scope.$apply();
                $("#table").trigger("update");
            })
        }
        getByPage(1);
        getPageCount();
        //点击新增按钮
        $scope.create = function () {
            getRoleTree(0);
            $.get("../process/GetProcessList").then(function (result) {
                var processes = JSON.parse(result);
                $scope.$root.edit = {
                    activity: {
                        Name: '',
                        Phone: '',
                        ActivityPwd: '',
                        Processes: processes,
                    },
                    model: '新增'
                };
                $scope.$root.edit.activity.ProcessId=processes[0].Id.toString();
                $scope.$apply();
            })
        }
        //点击编辑按钮
        $scope.edit = function (Activity) {
            getRoleTree(Activity.Id);
            $.get("../process/GetProcessList").then(function (result) {
                $scope.$root.edit = {
                    activity: Activity,
                    model: '编辑'
                };
                $scope.$root.edit.activity.Processes = JSON.parse(result);
                $scope.$root.edit.activity.ProcessId = $scope.$root.edit.activity.ProcessId.toString();
                $scope.$apply();
            })
        }
        function getRoleTree(activityId) {
            $.get("../role/GetRoleTree?activityId=" + activityId).then(function (result) {
                $('#tree_1').jstree("destroy");
                $("#tree_1").jstree({
                    "core": {
                        "data": JSON.parse(result)
                    },
                    "plugins": ["themes", "json_data", "search", "checkbox"]
                });
            })
        }
        //删除确认
        $scope.confirmdelete = function (Activity) {
            $scope.$root.del = {
                activity: Activity
            }
        }
        //删除
        $scope.delete = function (Activity) {
            activityService.destroy(Activity).then(function () {
                removeItemById(Activity.Id);
            })
        }
        //从列表中把删除项移除
        var removeItemById = function (id) {
            for (var i = 0; i < $scope.activitys.length; i++) {
                if ($scope.activitys[i].Id == id) {
                    $scope.activitys.splice(i, 1);
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
}(angular.module("atTheActivity")));