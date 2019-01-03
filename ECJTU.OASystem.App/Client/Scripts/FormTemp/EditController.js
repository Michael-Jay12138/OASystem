(function (app) {
    var EditController = function ($scope, formTempService) {
        //点击取消
        $scope.cancel = function () {
            $scope.$root.edit.formTemp = null;
        };
        //点击保存
        $scope.save = function () {

            if ($scope.edit.formTemp.Id) {
                updateFormTemp();
            }
            else {
                createFormTemp();
            }
        };
        //为表单模板添加关联业务
        function addRelationToBusiness(formTempId) {
            $.post("../business/AddFormTemp", { businessId: $scope.$root.edit.BusinessId, formTempId: formTempId }).then(function (result) {
                if (result) {

                }
            })
        }
        //为表单模板更新关联业务
        function updateRelationToBusiness(formTempId) {
            $.post("../business/UpdateFormTemp", { businessId: $scope.$root.edit.BusinessId, formTempId: formTempId }).then(function (result) {
                if (result) {

                }
            })
        }
        //更新数据
        var updateFormTemp = function () {
            var formTempupdate = $scope.$root.edit.formTemp;
            formTempService.update(formTempupdate).then(function (result) {
                editFormTemp(formTempupdate);
                updateRelationToBusiness(formTempupdate.Id);
            });
        };
        //添加数据
        var createFormTemp = function () {
            var formTempadd = $scope.$root.edit.formTemp;
            formTempService.create(formTempadd).then(function (result) {
                formTempadd.Id = result;
                addFormTemp(formTempadd);
                addRelationToBusiness(formTempadd.Id);
            });
        };
        //向列表添加数据
        var addFormTemp = function (formTemp) {
            $scope.$root.formTemps.push(formTemp);
            $scope.$root.edit.formTemp = null;
        }
        //更新列表数据
        var editFormTemp = function (formTemp) {
            for (var i = 0; i < $scope.$root.formTemps.length; i++) {
                if ($scope.$root.formTemps[i].ClassId == formTemp.Id) {
                    $scope.$root.formTemps.splice(i, 1, formTemp);
                    break;
                }
            }
            $scope.$root.edit.formTemp = null;
        };
    };
    app.controller("EditController", EditController);
}(angular.module("atTheFormTemp")));