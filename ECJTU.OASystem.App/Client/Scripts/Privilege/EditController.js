(function (app) {
    var EditController = function ($scope, privilegeService) {
        //点击取消
        $scope.cancel = function () {
            $scope.$root.edit.privilege = null;
        };
        //点击保存
        $scope.save = function () {

            if ($scope.edit.privilege.Id) {
                updatePrivilege();
            }
            else {
                createPrivilege();
            }
        };
        //更新数据
        var updatePrivilege = function () {
            var privilegeupdate = $scope.$root.edit.privilege;
            privilegeService.update(privilegeupdate).then(function (result) {
                console.log(result);
                editMaterial(privilegeupdate);
            });
        };
        //添加数据
        var createPrivilege = function () {
            var privilegeadd = $scope.$root.edit.privilege;
            privilegeService.create(privilegeadd).then(function (result) {
                var backdata = result.data;
                console.log(backdata);
                privilegeadd.Id = backdata.Id;
                privilegeadd.PrivilegeRegisterDate = backdata.PrivilegeRegisterDate;
                addPrivilege(privilegeadd);
            });
        };
        //向列表添加数据
        var addPrivilege = function (privilege) {
            $scope.$root.privileges.push(privilege);
            $scope.$root.edit.privilege = null;
        }
        //更新列表数据
        var editPrivilege = function (privilege) {
            for (var i = 0; i < $scope.$root.privileges.length; i++) {
                if ($scope.$root.privileges[i].ClassId == privilege.Id) {
                    $scope.$root.privileges.splice(i, 1, privilege);
                    break;
                }
            }
            $scope.$root.edit.privilege = null;
        };
    };
    app.controller("EditController", EditController);
}(angular.module("atThePrivilege")));