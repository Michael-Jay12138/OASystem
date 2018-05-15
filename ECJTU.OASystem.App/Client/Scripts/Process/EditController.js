(function (app) {
    var EditController = function ($scope, processService) {
        //点击取消
        $scope.cancel = function () {
            $scope.$root.edit.process = null;
        };
        //点击保存
        $scope.save = function () {

            if ($scope.edit.process.Id) {
                updateProcess();
            }
            else {
                createProcess();
            }
        };
        //更新数据
        var updateProcess = function () {
            var processupdate = $scope.$root.edit.process;
            processService.update(processupdate).then(function (result) {
                console.log(result);
                editMaterial(processupdate);
            });
        };
        //添加数据
        var createProcess = function () {
            var processadd = $scope.$root.edit.process;
            processService.create(processadd).then(function (result) {
                var backdata = result.data;
                console.log(backdata);
                processadd.Id = backdata;
                addProcess(processadd);
            });
        };
        //向列表添加数据
        var addProcess = function (process) {
            $scope.$root.processs.push(process);
            $scope.$root.edit.process = null;
        }
        //更新列表数据
        var editProcess = function (process) {
            for (var i = 0; i < $scope.$root.processs.length; i++) {
                if ($scope.$root.processs[i].ClassId == process.Id) {
                    $scope.$root.processs.splice(i, 1, process);
                    break;
                }
            }
            $scope.$root.edit.process = null;
        };
    };
    app.controller("EditController", EditController);
}(angular.module("atTheProcess")));