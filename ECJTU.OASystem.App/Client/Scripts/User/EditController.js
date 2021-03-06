﻿(function (app) {
    var EditController = function ($scope, userService) {
        //点击取消
        $scope.cancel = function () {
            $scope.$root.edit.user = null;
        };
        //点击保存
        $scope.save = function () {

            if ($scope.edit.user.Id) {
                updateUser();
            }
            else {
                createUser();
            }
        };
        //添加数据
        var createUser = function () {
            var useradd = $scope.$root.edit.user;
            userService.create(useradd).then(function (result) {
                var backdata = result.data;
                console.log(backdata);
                useradd.Id = backdata.Id;
                useradd.UserRegisterDate = backdata.UserRegisterDate;
                addUser(useradd);
            });
        };
        //更新数据
        var updateUser = function () {
            var userupdate = $scope.$root.edit.user;
            userService.update(userupdate).then(function (result) {
                editUser(userupdate);
            });
        };
        //向列表添加数据
        var addUser = function (user) {
            $scope.$root.users.push(user);
            $scope.$root.edit.user = null;
        }
        //更新列表数据
        var editUser = function (user) {
            for (var i = 0; i < $scope.$root.users.length; i++) {
                if ($scope.$root.users[i].ClassId == user.Id) {
                    $scope.$root.users.splice(i, 1, user);
                    break;
                }
            }
            $scope.$root.edit.user = null;
        };
    };
    app.controller("EditController", EditController);
}(angular.module("atTheUser")));