(function (app) {
    var ListController = function ($scope, organizationService) {
        //当前页面默认为1显示10行数据
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        //获取页面总数
        //判断是否已登录
        $scope.haveLogin = function () {
            return true;
        }
        
        function getPageCount() {
            $.get("../organization/GetDataCount").then(function (result) {
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
            $.get("../organization/GetOrganizationListByPage?" + "pageIndex=" + pageIndex + "&pageSize=" + $scope.pageSize).then(function (result) {
                var organizations = JSON.parse(result);
                $scope.$root.organizations = organizations;
                $scope.$apply();
                $("#table").trigger("update");
            })
        }
        getByPage(1);
        getPageCount();
        //点击新增按钮
        $scope.create = function () {
            $scope.$root.edit = {
                organization: {
                    Name: '',
                    Phone: '',
                    OrganizationPwd:''
                },
                model: '新增'
            };
        }
        //点击编辑按钮
        $scope.edit = function (Organization) {
            $scope.$root.edit = {
                organization: Organization,
                model: '编辑'
            };
        }
        //删除确认
        $scope.confirmdelete = function (Organization) {
            $scope.$root.del = {
                organization: Organization
            }
        }
        //删除
        $scope.delete = function (Organization) {
            organizationService.destroy(Organization).then(function () {
                removeItemById(Organization.Id);
            })
        }
        //从列表中把删除项移除
        var removeItemById = function (id) {
            for (var i = 0; i < $scope.organizations.length; i++) {
                if ($scope.organizations[i].Id == id) {
                    $scope.organizations.splice(i, 1);
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
}(angular.module("atTheOrganization")));