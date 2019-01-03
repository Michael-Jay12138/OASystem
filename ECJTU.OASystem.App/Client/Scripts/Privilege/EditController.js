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
                editPrivilege(privilegeupdate);
                updateRoles(privilegeupdate.Id);
            });
        };
        //添加数据
        var createPrivilege = function () {
            var privilegeadd = $scope.$root.edit.privilege;
            privilegeService.create(privilegeadd).then(function (result) {
                privilegeadd.Id = result;
                addPrivilege(privilegeadd);
                addRoles(privilegeadd.Id);
            });
        };
        //为权限添加角色
        function addRoles(privilegeId) {
            var roleTree = $('#tree_1').jstree(true);
            var selRoleIds = roleTree.get_selected();
            $.post("../privilege/AddRoles", { privilegeId: privilegeId, roleIds: selRoleIds }).then(function (result) {
                if (result) {

                }
            })
        }
        //更新角色用户信息
        function updateRoles(privilegeId) {
            var roleTree = $('#tree_1').jstree(true);
            var selRoleIds = roleTree.get_selected();
            $.post("../privilege/UpdateRoles", { privilegeId: privilegeId, roleIds: selRoleIds }).then(function (result) {
                if (result) {

                }
            })
        }
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