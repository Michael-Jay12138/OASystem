(function (app) {
    var EditController = function ($scope, roleService) {
        //点击取消
        $scope.cancel = function () {
            $scope.$root.edit.role = null;
        };
        //点击保存
        $scope.save = function () {

            if ($scope.edit.role.Id) {
                updateRole();
            }
            else {
                createRole();
            }
        };
        //更新数据
        var updateRole = function () {
            var roleupdate = $scope.$root.edit.role;
            roleService.update(roleupdate).then(function (result) {
                console.log(result);
                editRole(roleupdate);
                updateUsers(roleupdate.Id)
            });
        };
        //添加数据
        var createRole = function () {
            var roleadd = $scope.$root.edit.role;
            roleService.create(roleadd).then(function (result) {
                var backdata = result.data;
                console.log(backdata);
                roleadd.Id = backdata;
                addRole(roleadd);
                addUsers(roleadd.Id);
            });
        };
        //为角色添加用户
        function addUsers(roleId) {
            var userTree = $('#tree_1').jstree(true);
            var selUserIds = userTree.get_selected();
            $.post("../role/AddUsers", { roleId: roleId, userIds: selUserIds }).then(function (result) {
                if (result) {

                }
            })
        }
        //更新角色用户信息
        function updateUsers(roleId) {
            var userTree = $('#tree_1').jstree(true);
            var selUserIds = userTree.get_selected();
            $.post("../role/UpdateUsers", { roleId: roleId, userIds: selUserIds }).then(function (result) {
                if (result) {

                }
            })
        }
        //向列表添加数据
        var addRole = function (role) {
            $scope.$root.roles.push(role);
            $scope.$root.edit.role = null;
        }
        //更新列表数据
        var editRole = function (role) {
            for (var i = 0; i < $scope.$root.roles.length; i++) {
                if ($scope.$root.roles[i].Id == role.Id) {
                    $scope.$root.roles.splice(i, 1, role);
                    break;
                }
            }
            $scope.$root.edit.role = null;
        };
    };
    app.controller("EditController", EditController);
}(angular.module("atTheRole")));