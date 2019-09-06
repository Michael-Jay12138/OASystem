(function (app) {
    var EditController = function ($scope, projectService) {
        $scope.pageIndex = 1;
        $scope.showByIndex = function (array) {
            for (var i = 0; i < array.length; i++) {
                if (array[i] == $scope.pageIndex)
                    return true;
            }
            return false;
        }
        $.get("../business/GetBusinessList").then(function (result) {
            var businesses = JSON.parse(result);
            $scope.$root.edit = {
                project: {
                    Name: '',
                    ProjectNo: '',
                    CreateUserId: '',
                    CurrentUserId: '',
                    State: '',
                    BusinessId: '',
                    WorkItemId: '',
                    Remark: '',
                    Businesses: businesses
                },
                model: '新增'
            };

            $scope.$root.edit.project.BusinessId = businesses[0].Id.toString();
            $scope.$apply();
            //window.location = "#/project/create";
        })
        //点击下一步
        $scope.continue = function () {
            $scope.pageIndex++;
            if ($scope.pageIndex == 2) {
                //根据第一步选择的业务获取要填写的表单
                $.get("../formTemp/GetFormByBusinessId?businessId=" + $scope.$root.edit.project.BusinessId).then(function (result) {
                    $("#tab2-form").html(result);
                })
            }
            if ($scope.pageIndex == 3) {
                //获取第二步填写的表单展现在第三步
                var newNode = document.getElementById("tab2-form").cloneNode(true);
                newNode.id = "tab3-form";
                var oldNode = document.getElementById("tab3-form");
                oldNode.parentNode.replaceChild(newNode, oldNode);
                $("#tab3-form input").each(function () { $(this).attr("value", this.value) })
                //$.get("../user/GetFAUsersByBusinessId?businessId=" + $scope.$root.edit.project.BusinessId).then(function (result) {
                //    $scope.$root.edit.project.NextUsers = JSON.parse(result);
                //    $scope.$apply();
                //})
            }
        }
        //点击确认创建
        $scope.submit = function () {
            var form = $("#tab3-form").html();
            $.post('../FormTemp/formHtml2htmlFile', { formHtml: form }).then(function (result) {
                console.log(result);
                //alert("表单保存成功");
                createProject();
            });
        }
        $scope.back = function () {
            $scope.pageIndex--;
        }
        //点击取消
        $scope.cancel = function () {
            $scope.$root.edit.project = null;
        };
        //点击保存
        $scope.save = function () {

            if ($scope.edit.project.Id) {
                updateProject();
            }
            else {
                createProject();
            }
        };
        //添加数据
        var createProject = function () {
            var projectadd = $scope.$root.edit.project;
            projectService.create(projectadd, getCookie("userName")).then(function (result) {
                var backdata = result.data;
                //console.log(backdata);
                //projectadd.Id = backdata.Id;
                //projectadd.CreateTime = backdata.CreateTime;
                //addProject(projectadd);
                window.location = "#/project/list";
            });
        };
        //向列表添加数据
        var addProject = function (project) {
            $scope.$root.projects.push(project);
            $scope.$root.edit.project = null;
        }
        //更新列表数据
        var editProject = function (project) {
            for (var i = 0; i < $scope.$root.projects.length; i++) {
                if ($scope.$root.projects[i].ClassId == project.Id) {
                    $scope.$root.projects.splice(i, 1, project);
                    break;
                }
            }
            $scope.$root.edit.project = null;
        };
        //获取cookie
        function getCookie(name) {
            var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if (arr = document.cookie.match(reg))
                return unescape(arr[2]);
            else
                return null;
        }
    };
    app.controller("EditController", EditController);
}(angular.module("atTheProject")));