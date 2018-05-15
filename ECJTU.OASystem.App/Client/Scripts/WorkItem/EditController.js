(function (app) {
    var EditController = function ($scope, workitemService) {
        //点击取消
        $scope.cancel = function () {
            $scope.$root.edit.workitem.NextUser = null;

            $scope.$root.edit.workitem.NextActivity = null;
        };
        //点击保存
        $scope.save = function () {

            if ($scope.edit.workitem.Id) {
                updateWorkItem();
            }
            else {
                createWorkItem();
            }
        };
        //添加数据
        var createWorkItem = function () {
            var workitemadd = $scope.$root.edit.workitem;
            workitemService.create(workitemadd).then(function (result) {
                var backdata = result.data;
                console.log(backdata);
                workitemadd.Id = backdata.Id;
                workitemadd.WorkItemRegisterDate = backdata.WorkItemRegisterDate;
                addWorkItem(workitemadd);
            });
        };
        //向列表添加数据
        var addWorkItem = function (workitem) {
            $scope.$root.workitems.push(workitem);
            $scope.$root.edit.workitem = null;
        }
        //更新列表数据
        var editWorkItem = function (workitem) {
            for (var i = 0; i < $scope.$root.workitems.length; i++) {
                if ($scope.$root.workitems[i].ClassId == workitem.Id) {
                    $scope.$root.workitems.splice(i, 1, workitem);
                    break;
                }
            }
            $scope.$root.edit.workitem = null;
        };
        //点击发送
        $scope.getNextActivityUsers = function () {
            $.get("../activity/GetNextActivity?activityInstId=" + $scope.$root.edit.workitem.ActivityInstId).then(function (result_a) {
                var nextActivity = JSON.parse(result_a);
                $scope.$root.edit.workitem.NextActivity = nextActivity;
                if (nextActivity.Name == "结束环节") {
                    $scope.$root.edit.workitem.NextUsers = [{
                        Id: 0,
                        Name: '结束'
                    }];
                    $scope.$root.edit.workitem.NextUserId = '0';
                    $scope.$apply();
                }
                else {
                    $.get("../user/GetNextActivityUsers?activityId=" + nextActivity.Id).then(function (result_u) {
                        var users = JSON.parse(result_u);
                        $scope.$root.edit.workitem.NextUsers = users;
                        $scope.$root.edit.workitem.NextUserId = users[0].Id.toString();
                        $scope.$apply();
                    })
                }
            })
        }
        //确认发送
        $scope.submit = function () {
            var workitem = $scope.$root.edit.workitem;
            $.post("../workitem/SendWorkItem",
                {
                    workItemId: workitem.Id,
                    receiveUserId: workitem.NextUserId,
                    preActivityInstId: workitem.ActivityInstId,
                    nextActivityId: workitem.NextActivity.Id
                }).then(function (result) {
                    if (result) {
                        $scope.$root.edit.workitem = null;
                        window.location.href = "#/workitem/list";
                    }
                })
        }
    };
    app.controller("EditController", EditController);
}(angular.module("atTheWorkItem")));