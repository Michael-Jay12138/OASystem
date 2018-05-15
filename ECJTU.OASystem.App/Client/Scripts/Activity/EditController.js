(function (app) {
    var EditController = function ($scope, activityService) {
        //点击取消
        $scope.cancel = function () {
            $scope.$root.edit.activity = null;
        };
        //点击保存
        $scope.save = function () {

            if ($scope.edit.activity.Id) {
                updateActivity();
            }
            else {
                createActivity();
            }
        };
        //更新数据
        var updateActivity = function () {
            var activityupdate = $scope.$root.edit.activity;
            activityService.update(activityupdate).then(function (result) {
                console.log(result);
                editActivity(activityupdate);
                updateRoles(activityupdate.Id);
            });
        };
        //添加数据
        var createActivity = function () {
            var activityadd = $scope.$root.edit.activity;
            activityService.create(activityadd).then(function (result) {
                var backdata = result.data;
                console.log(backdata);
                activityadd.Id = backdata;
                addActivity(activityadd);
                addRoles(activityadd.Id);
            });
        };
        //为环节添加角色
        function addRoles(activityId) {
            var roleTree = $('#tree_1').jstree(true);
            var selRoleIds = roleTree.get_selected();
            $.post("../activity/AddRoles", { activityId: activityId, roleIds: selRoleIds }).then(function (result) {
                if (result) {

                }
            })
        }
        //更新环节角色信息
        function updateRoles(activityId) {
            var roleTree = $('#tree_1').jstree(true);
            var selRoleIds = roleTree.get_selected();
            $.post("../activity/UpdateRoles", { activityId: activityId, roleIds: selRoleIds }).then(function (result) {
                if (result) {

                }
            })
        }
        //向列表添加数据
        var addActivity = function (activity) {
            $scope.$root.activitys.push(activity);
            $scope.$root.edit.activity = null;
        }
        //更新列表数据
        var editActivity = function (activity) {
            for (var i = 0; i < $scope.$root.activitys.length; i++) {
                if ($scope.$root.activitys[i].ClassId == activity.Id) {
                    $scope.$root.activitys.splice(i, 1, activity);
                    break;
                }
            }
            $scope.$root.edit.activity = null;
        };
    };
    app.controller("EditController", EditController);
}(angular.module("atTheActivity")));