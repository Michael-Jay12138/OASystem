(function (app) {
    var EditController = function ($scope, workitemService) {
        //当前页面默认为1显示10行数据
        $scope.pageIndex = 1;
        $scope.pageSize = 5;
        //判断是否已登录
        $scope.haveLogin = function () {
            return true;
        }
        //获取页面总数
        function getPageCount() {
            $.get("../material/GetDataCount?workItemId=" + $scope.edit.workitem.Id).then(function (result) {
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
            $.get("../material/GetMaterialListByPage?" + "pageIndex=" + pageIndex + "&pageSize=" + $scope.pageSize + "&workItemId=" + $scope.$root.edit.workitem.Id).then(function (result) {
                var materials = JSON.parse(result);
                $scope.materials = materials;
                $scope.$apply();
                $("#table").trigger("update");
            })
        }
        getByPage(1);
        getPageCount();
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
        //点击新增按钮
        $scope.upload = function () {
            $.get("../material/GetMaterialTempInstIdByProjectId?projectId=" + $scope.edit.workitem.ProjectId).then(function (result) {
                $scope.edit.material = {
                    Name: '',
                    Path: '',
                    MaterialTempInstId: result
                };
                $scope.edit.material.model = '新增';
                $scope.$apply();
            })
        }
        $scope.fileSelected = function (element) {
            $scope.edit.material.Name = element.files[0].name;
            $scope.$apply();
        }
        //上传文件
        $scope.createMaterial = function () {
            var files = document.getElementById("file").files;


            var fd = new FormData();
            fd.append("file", files[0]);

            var xhr = new XMLHttpRequest();
            xhr.addEventListener("load", uploadComplete, false);
            xhr.open("POST", "../Service/FileUploadService.ashx");
            xhr.send(fd);
        };
        $scope.download = function (material) {
            $.post("../material/GetMaterial", material).then(function (result) {
                //$.post("../Service/FileDownloadService.ashx", { tempFilePath: result }).then(function (data) {

                //})
                window.open("../Temp/" + result);
            })
        }
        //添加数据
        function uploadComplete(result) {
            var materialadd = $scope.edit.material;
            $.post("../material/AddMaterial", { materialTempInstId: $scope.edit.material.MaterialTempInstId, materialLocalPath: result.currentTarget.response }).then(function (backdata) {
                var obj=JSON.parse(backdata);
                materialadd.Id = obj.Id;
                materialadd.Path = obj.Path;
                getByPage(1);
                //addMaterial(materialadd);
            });
        }
        //向列表添加数据
        var addMaterial = function (material) {
            if ($scope.materials == null) {
                location.reload([true]);
            }
            else {
                $scope.materials.push(material);
            }
            $scope.edit.material = null;
        }
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
            var form =$("#formInst").html();
            $.post('../FormTemp/formHtml2htmlFile', { formHtml: form }).then(function (result) {
                console.log(result);
                alert("表单保存成功");
                sendWorkItem(workitem);
            });
            
        }
        function sendWorkItem(workitem) {
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